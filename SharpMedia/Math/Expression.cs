// This file constitutes a part of the SharpMedia project, (c) 2007 by the SharpMedia team
// and is licensed for your use under the conditions of the NDA or other legally binding contract
// that you or a legal entity you represent has signed with the SharpMedia team.
// In an event that you have received or obtained this file without such legally binding contract
// in place, you MUST destroy all files and other content to which this lincese applies and
// contact the SharpMedia team for further instructions at the internet mail address:
//
//    legal@sharpmedia.com
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using SharpMedia.AspectOriented;

namespace SharpMedia.Math
{


    /// <summary>
    /// Exception thrown if expression cannot be compiled.
    /// </summary>
    public class InvalidMathExpression : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidMathExpression(string message)
            : base(message)
        {
        }

        public InvalidMathExpression()
            : base("Invalid expression.")
        {
        }

        public InvalidMathExpression(string message, Exception inner)
            : base(message, inner)
        {
        }

        public InvalidMathExpression(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    


    /// <summary>
    /// A mathematical expression. Expression is represented as a string and is compiled
    /// into IL form, thus making a very fast execution through delegate. Mathematical
    /// expression can be written either as a realx4 -> realx4 calls (any combination
    /// of four inputs and four outputs). The number and name of named parameters is not
    /// limited.
    /// </summary>
    /// <remarks>
    /// Calling a GetFunction* always creates a new delegate expression. Expression compiling
    /// will check for constant expressions (13*15 will be precomputed, 23*k*m will be also
    /// precomputed if k and m are parameters, not variables), the compiler also precompute
    /// constant function calls, the division by the constant will be converted to multiplication
    /// by a constant and some expressions will be automatically set to zero; x^0 => 0 whatever
    /// x is (in case of x==0, this is wrong, but x==0 must not occur), 0^x is always ==> (again
    /// not true for x == 0) ...
    /// 
    /// In future, you may expect other optimisations to be written on top of this library.
    /// </remarks>
    public sealed partial class Expression
    {

        #region Private Data
        private IElement[] roots;
        private string name;
        private IElement root
        {
            get { return roots[0]; }
        }
        #endregion

        #region Expression Parsing and Validating
        internal enum ParseCodeId
        {
            None = 0,
            /// <summary>
            /// Begin block operation, or "(" symbol.
            /// </summary>
            Begin = 1,
            /// <summary>
            /// End block operation, or ")" symbol.
            /// </summary>
            End = 2,
            /// <summary>
            /// Function begin block; identifier followed by "(".
            /// </summary>
            BeginFunction = 3,
            /// <summary>
            /// The power, or "^" symbol.
            /// </summary>
            Power = 4,
            /// <summary>
            /// Division operator, or "/" symbol.
            /// </summary>
            Divide = 5,
            /// <summary>
            /// The multiply operator, or "*" symbol.
            /// </summary>
            Multiply = 6,
            /// <summary>
            /// The addition operator, or "+" symbol.
            /// </summary>
            Plus = 7,
            /// <summary>
            /// The substraction operation, or "-" symbol.
            /// </summary>
            Minus = 8,
            /// <summary>
            /// Identifier, can be a constant or value.
            /// </summary>
            Identifier = 9,
            /// <summary>
            /// A comma, only for multiple arguments.
            /// </summary>
            Comma = 10
        }

        /// <summary>
        /// A helper parse code class.
        /// </summary>
        internal class ParseCode
        {
            public ParseCodeId Id;
            public string Identifier = null;

            public ParseCode(ParseCodeId id) { Id = id; }
            public ParseCode(string id) { Id = ParseCodeId.Identifier; Identifier = id; }
            public ParseCode(ParseCodeId code, string id) { Id = code; Identifier = id; }
        }

        /// <summary>
        /// Converts expression to stream.
        /// </summary>
        private List<ParseCode> ConvertToTokenStream(string expr)
        {
            // We hint number of elements.
            List<ParseCode> code = new List<ParseCode>(expr.Length / 3);

            // We start parsing.
            string currentIdentifier = null;
            for (int i = 0; i < expr.Length; i++)
            {
                char c = expr[i];
                if (Char.IsWhiteSpace(c)) continue;
                switch (c)
                {
                    case '+':
                        if (currentIdentifier != null)
                        {
                            code.Add(new ParseCode(currentIdentifier));
                            currentIdentifier = null;
                        }
                        code.Add(new ParseCode(ParseCodeId.Plus));
                        break;
                    case '-':
                        if (currentIdentifier != null)
                        {
                            code.Add(new ParseCode(currentIdentifier));
                            currentIdentifier = null;
                        }
                        code.Add(new ParseCode(ParseCodeId.Minus));
                        break;
                    case '/':
                        if (currentIdentifier != null)
                        {
                            code.Add(new ParseCode(currentIdentifier));
                            currentIdentifier = null;
                        }
                        code.Add(new ParseCode(ParseCodeId.Divide));
                        break;
                    case '*':
                        if (currentIdentifier != null)
                        {
                            code.Add(new ParseCode(currentIdentifier));
                            currentIdentifier = null;
                        }
                        code.Add(new ParseCode(ParseCodeId.Multiply));
                        break;
                    case '^':
                        if (currentIdentifier != null)
                        {
                            code.Add(new ParseCode(currentIdentifier));
                            currentIdentifier = null;
                        }
                        code.Add(new ParseCode(ParseCodeId.Power));
                        break;
                    case '(':
                        if (currentIdentifier != null)
                        {
                            code.Add(new ParseCode(ParseCodeId.BeginFunction, currentIdentifier));
                            currentIdentifier = null;
                        }
                        else
                        {
                            code.Add(new ParseCode(ParseCodeId.Begin));
                        }
                        break;
                    case ')':
                        if (currentIdentifier != null)
                        {
                            code.Add(new ParseCode(currentIdentifier));
                            currentIdentifier = null;
                        }
                        code.Add(new ParseCode(ParseCodeId.End));
                        break;
                    case ',':
                        if (currentIdentifier != null)
                        {
                            code.Add(new ParseCode(currentIdentifier));
                            currentIdentifier = null;
                        }
                        code.Add(new ParseCode(ParseCodeId.Comma));
                        break;
                    default:
                        currentIdentifier += c;
                        break;
                }
            }

            // Not forget the last one.
            if (currentIdentifier != null)
            {
                code.Add(new ParseCode(currentIdentifier));
            }

            return code;
        }

        /// <summary>
        /// Removes code if in such form "(expr)" to "expr".
        /// </summary>
        private void RemoveUnneededGroups(List<ParseCode> code)
        {
            if (code.Count < 2) return;
            if (code[0].Id == ParseCodeId.Begin && code[code.Count - 1].Id == ParseCodeId.End)
            {
                uint depth = 1;

                // Only chance to delete, may actually mean sth like (x*y)*z*(f*x).
                for (int i = 1; i < (code.Count-1); i++)
                {
                    if (code[i].Id == ParseCodeId.Begin || code[i].Id == ParseCodeId.BeginFunction) depth++;
                    if (code[i].Id == ParseCodeId.End) depth--;

                    if (depth == 0) return;
                }

                // We can remove them.
                code.RemoveAt(code.Count - 1);
                code.RemoveAt(0);
            }
            
        }

        /// <summary>
        /// Finds least binding parameter, taking ( and ) into account.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private int FindLeastBinding(List<ParseCode> code)
        {
            // Make sure no additional ()s.
            RemoveUnneededGroups(code);

            if (code.Count == 1)
            {
                if (code[0].Id != ParseCodeId.Identifier) throw 
                    new InvalidMathExpression("Cannot compile expression, identifier expected.");
                return 0;
            }

            int index = -1;
            ParseCodeId id = ParseCodeId.None;
            int depth = 0;

            for (int i = 0; i < code.Count; i++)
            {
                ParseCode c = code[i];
                if (depth == 0 && c.Id != ParseCodeId.Identifier && c.Id != ParseCodeId.Comma)
                {
                    if ((int)c.Id > (int)id)
                    {
                        index = i;
                        id = c.Id;
                    }
                }

                // We advance depth.
                if (c.Id == ParseCodeId.Begin || c.Id == ParseCodeId.BeginFunction) depth++;
                if (c.Id == ParseCodeId.End) depth--;


            }

            if (depth != 0) throw new InvalidMathExpression("The number of '(' and ')' does not match.");
            if (index == -1) throw new InvalidMathExpression("Multiple identifiers, cannot compile.");

            // Least bound returned.
            return index;
        }

        /// <summary>
        /// We split to multiple elements on comma.
        /// </summary>
        private IElement[] SplitOnComma(List<ParseCode> code)
        {
            RemoveUnneededGroups(code);

            // We now split on ","s, only taking not nested into accout.
            List<int> splitPos = new List<int>();
            int depth = 0;
            for (int i = 0; i < code.Count; i++)
            {
                ParseCode c = code[i];
                if (depth == 0 && c.Id == ParseCodeId.Comma) splitPos.Add(i);

                if (c.Id == ParseCodeId.Begin || c.Id == ParseCodeId.BeginFunction) depth++;
                if (c.Id == ParseCodeId.End) depth--;
            }

            // Allocate elements.
            IElement[] elements = new IElement[splitPos.Count + 1];

            // We go split by split.
            int prev = -1;
            for(int j = 0; j < splitPos.Count; j++)
            {
                elements[j] = Parse(code.GetRange(prev + 1, splitPos[j] - prev - 1));
                prev = splitPos[j];
            }

            // We process last one.
            elements[splitPos.Count] = Parse(code.GetRange(prev + 1, code.Count - prev - 1));

            // We return elements parsed.
            return elements;
        }

        /// <summary>
        /// Parses hierachy into element (quick code generation) structure.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private IElement Parse(List<ParseCode> code)
        {
            if (code.Count < 1) throw new InvalidMathExpression("Cannot compile 'null' expression part, expression expected.");

            // We split on least binding.
            int splitPos = FindLeastBinding(code);
            ParseCode c = code[splitPos];
            
            // We switch based on value.
            switch (c.Id)
            {
                case ParseCodeId.Identifier:
                    return new IdElement(code[splitPos].Identifier);
                case ParseCodeId.Minus:
                    if (splitPos != 0)
                    {
                        return new Substract(Parse(code.GetRange(0, splitPos)),
                                             Parse(code.GetRange(splitPos + 1, code.Count - splitPos - 1)));
                    }
                    else
                    {
                        // This is unary operator then.
                        return new Multiply(new IdElement(-1.0), Parse(code.GetRange(splitPos + 1, code.Count - splitPos - 1)));
                    }
                case ParseCodeId.Plus:
                    return new Add(Parse(code.GetRange(0, splitPos)),
                                         Parse(code.GetRange(splitPos + 1, code.Count - splitPos - 1)));
                case ParseCodeId.Multiply:
                    return new Multiply(Parse(code.GetRange(0, splitPos)),
                                         Parse(code.GetRange(splitPos + 1, code.Count - splitPos - 1)));
                case ParseCodeId.Divide:
                    return new Divide(Parse(code.GetRange(0, splitPos)),
                                        Parse(code.GetRange(splitPos + 1, code.Count - splitPos - 1)));
                case ParseCodeId.BeginFunction:
                    {
                        List<ParseCode> remaining = code.GetRange(splitPos + 1, code.Count - splitPos - 2);

                        // We split on all ","s in the same scope.
                        return new Function(c.Identifier, SplitOnComma(remaining));
                    }
                case ParseCodeId.Power:
                    return new Power(Parse(code.GetRange(0, splitPos)),
                                     Parse(code.GetRange(splitPos + 1, code.Count - splitPos - 1)));
                default:
                    throw new InvalidMathExpression("Expression not supported or something wrong with expression.");
            }
        }

        internal interface IElement
        {
            /// <summary>
            /// Generates for single precision.
            /// </summary>
            /// <param name="generator">The generator.</param>
            void GenerateSingle(ILGenerator generator, FunctionSet set, FunctionParams pars);

            /// <summary>
            /// Generates for double precision.
            /// </summary>
            /// <param name="generator">The generator.</param>
            void GenerateDouble(ILGenerator generator, FunctionSet set, FunctionParams pars);

            /// <summary>
            /// Useful in conjuction with constant expression, can immediatelly validate it.
            /// </summary>
            /// <param name="pars"></param>
            /// <returns></returns>
            bool HasConstantValue(FunctionSet set, FunctionParams pars, out double val);

            /// <summary>
            /// Adds named values.
            /// </summary>
            void AddNamedValues(List<string> values);
        }

        internal class IdElement : IElement
        {
            private string value;
            private bool isNumeric = false;
            private double numericValue;

            public IdElement(string v) 
            {
                value = v;
                if (double.TryParse(v, out numericValue))
                {
                    isNumeric = true;
                }
            }

            public IdElement(double v)
            {
                isNumeric = true;
                numericValue = v;
            }

            #region IElement Members

            public void GenerateSingle(ILGenerator generator, FunctionSet set, FunctionParams pars)
            {
                
                if (isNumeric)
                {
                    // We push argument on stack.
                    generator.Emit(OpCodes.Ldc_R4, (float)numericValue);
                }
                else
                {
                    // Check for binding.
                    if (pars.HasBinding(value))
                    {
                        VariableBinding binding;
                        pars.GetBinding(value, out binding);
                        switch (binding.Position)
                        {
                            case 0:
                                generator.Emit(OpCodes.Ldarg_0);
                                break;
                            case 1:
                                generator.Emit(OpCodes.Ldarg_1);
                                break;
                            case 2:
                                generator.Emit(OpCodes.Ldarg_2);
                                break;
                            case 3:
                                generator.Emit(OpCodes.Ldarg_3);
                                break;
                            default:
                                throw new InvalidMathExpression("Trying non-used position.");
                        }
                    }
                    else
                    {
                        // Must succeed.
                        double v = pars[value];
                        generator.Emit(OpCodes.Ldc_R4, (float)v);   
                    }
                }
            }

            public void GenerateDouble(ILGenerator generator, FunctionSet set, FunctionParams pars)
            {
                if (isNumeric)
                {
                    // We push argument on stack.
                    generator.Emit(OpCodes.Ldc_R8, numericValue);
                }
                else
                {
                    // Check for binding.
                    if (pars.HasBinding(value))
                    {
                        VariableBinding binding;
                        pars.GetBinding(value, out binding);
                        switch (binding.Position)
                        {
                            case 0:
                                generator.Emit(OpCodes.Ldarg_0);
                                break;
                            case 1:
                                generator.Emit(OpCodes.Ldarg_1);
                                break;
                            case 2:
                                generator.Emit(OpCodes.Ldarg_2);
                                break;
                            case 3:
                                generator.Emit(OpCodes.Ldarg_3);
                                break;
                            default:
                                throw new InvalidMathExpression("Trying non-used position.");
                        }
                    }
                    else
                    {
                        // Must succeed.
                        double v = pars[value];
                        generator.Emit(OpCodes.Ldc_R8, v);
                    }
                }
            }

            public bool HasConstantValue(FunctionSet set, FunctionParams pars, out double val)
            {
                if (isNumeric)
                {
                    val = numericValue;
                    return true;
                }

                // Parameters (not variables) are also constants.
                if (pars.HasBinding(value))
                {
                    val = 0.0;
                    return false;
                }
                else
                {
                    val = pars[value];
                    return true;
                }
             
            }

            public void AddNamedValues(List<string> values)
            {
                if (!isNumeric)
                {
                    if (!values.Contains(value)) values.Add(value);
                }
            }

            #endregion
        }
        internal class Function : IElement
        {
            private string Name;
            private IElement[] arguments;
            public Function(string n, IElement[] arg) { Name = n; arguments = arg; }

            #region IElement Members

            public void GenerateSingle(ILGenerator generator, FunctionSet set, FunctionParams pars)
            {
                // We check if we have const value expression.
                double constValue;
                if (HasConstantValue(set, pars, out constValue))
                {
                    generator.Emit(OpCodes.Ldc_R4, (float)constValue);
                    return;
                }

                foreach (IElement el in arguments)
                {
                    el.GenerateSingle(generator, set, pars);
                }

                // We perform the call of this function. The function is prototyped
                // float F(float x).
                Delegate s = null;
                switch(arguments.Length)
                {
                    case 1:
                        s = set.Findf(Name);
                        break;
                    case 2:
                        s = set.Find2f(Name);
                        break;
                    case 3:
                        s = set.Find3f(Name);
                        break;
                    case 4:
                        s = set.Find4f(Name);
                        break;
                    default:
                        throw new InvalidMathExpression("Trying to call method with more then 4 arguments, unsuported.");
                }   
                if (s == null)
                {
                    throw new InvalidMathExpression("We cannot compile expression because the function " +
                                                         Name + " does not exist in set.");
                }

                // Call the method with argument.
                generator.EmitCall(OpCodes.Call, s.Method, null);
            }

            public void GenerateDouble(ILGenerator generator, FunctionSet set, FunctionParams pars)
            {
                // We check if we have const value expression.
                double constValue;
                if (HasConstantValue(set, pars, out constValue))
                {
                    generator.Emit(OpCodes.Ldc_R8, constValue);
                    return;
                }

                foreach (IElement el in arguments)
                {
                    el.GenerateDouble(generator, set, pars);
                }

                // We perform the call of this function. The function is prototyped
                // float F(float x).
                Delegate s = null;
                switch (arguments.Length)
                {
                    case 1:
                        s = set.Findd(Name);
                        break;
                    case 2:
                        s = set.Find2d(Name);
                        break;
                    case 3:
                        s = set.Find3d(Name);
                        break;
                    case 4:
                        s = set.Find4d(Name);
                        break;
                    default:
                        throw new InvalidMathExpression("Trying to call method with more then 4 arguments, unsuported.");
                }
                if (s == null)
                {
                    throw new InvalidMathExpression("We cannot compile expression because the function " +
                                                         Name + " does not exist in set.");
                }

                // Call the method with argument.
                generator.EmitCall(OpCodes.Call, s.Method, null);
            }


            public bool HasConstantValue(FunctionSet set, FunctionParams pars, out double val)
            {
                // We have constant value if and only if subelement is constant.
                double[] v = new double[arguments.Length];
                int index = 0;
                foreach (IElement el in arguments)
                {
                    if (el.HasConstantValue(set, pars, out v[index]))
                    {
                        continue;
                    }
                    val = 0.0;
                    return false;
                }

                // We find delegate
                Delegate s = null;
                switch (arguments.Length)
                {
                    case 1:
                        s = set.Findd(Name);
                        break;
                    case 2:
                        s = set.Find2d(Name);
                        break;
                    case 3:
                        s = set.Find3d(Name);
                        break;
                    case 4:
                        s = set.Find4d(Name);
                        break;
                    default:
                        throw new InvalidMathExpression("Trying to call method with more then 4 arguments, unsuported.");
                }

                if (s == null)
                {
                    throw new InvalidMathExpression("We cannot compile expression because the function " +
                                                         Name + " does not exist in set.");
                }

                // We execute it.
                val = (double)s.DynamicInvoke(v[0]);
                return true;
            }

            public void AddNamedValues(List<string> values)
            {
                foreach (IElement el in arguments)
                {
                    el.AddNamedValues(values);
                }
            }

            #endregion
        }


        internal class Divide : IElement
        {
            IElement src1;
            IElement src2;
            public Divide(IElement s1, IElement s2)
            {
                src1 = s1;
                src2 = s2;
            }

            #region IElement Members

            public void GenerateSingle(ILGenerator generator, FunctionSet set, FunctionParams pars)
            {
                // We check constants.
                double s1, s2;
                if (src2.HasConstantValue(set, pars, out s2))
                {
                    if (src1.HasConstantValue(set, pars, out s1))
                    {
                        generator.Emit(OpCodes.Ldc_R4, (float)(s1 / s2));
                        return;
                    }
                    else
                    {
                        // Because division sucks, we convert it to multiplication.
                        double inv_s2 = 1.0 / s2;
                        src1.GenerateSingle(generator, set, pars);
                        generator.Emit(OpCodes.Ldc_R4, (float)inv_s2);
                        generator.Emit(OpCodes.Mul);
                        return;
                    }
                }

                src1.GenerateSingle(generator, set, pars);
                src2.GenerateSingle(generator, set, pars);

                // We just generate add.
                generator.Emit(OpCodes.Div);
            }

            public void GenerateDouble(ILGenerator generator, FunctionSet set, FunctionParams pars)
            {
                // We check constants.
                double s1, s2;
                if (src2.HasConstantValue(set, pars, out s2))
                {
                    if (src1.HasConstantValue(set, pars, out s1))
                    {
                        generator.Emit(OpCodes.Ldc_R8, s1 / s2);
                        return;
                    }
                    else
                    {
                        // Because division sucks, we convert it to multiplication.
                        double inv_s2 = 1.0 / s2;
                        src1.GenerateDouble(generator, set, pars);
                        generator.Emit(OpCodes.Ldc_R8, inv_s2);
                        generator.Emit(OpCodes.Mul);
                        return;
                    }
                }

                src1.GenerateDouble(generator, set, pars);
                src2.GenerateDouble(generator, set, pars);

                // We just generate add.
                generator.Emit(OpCodes.Div);
            }


            public bool HasConstantValue(FunctionSet set, FunctionParams pars, out double val)
            {
                double s1, s2;
                if (src2.HasConstantValue(set, pars, out s2) && src1.HasConstantValue(set, pars, out s1))
                {
                    val = s1 / s2;
                    return true;
                }
                val = 0.0;
                return false;
            }

            public void AddNamedValues(List<string> values)
            {
                src1.AddNamedValues(values);
                src2.AddNamedValues(values);
            }

            #endregion
        }

        internal class Multiply : IElement
        {
            IElement src1;
            IElement src2;
            public Multiply(IElement s1, IElement s2) 
            { 
                src1 = s1;
                src2 = s2; 
            }

            #region IElement Members

            public void GenerateSingle(ILGenerator generator, FunctionSet set, FunctionParams pars)
            {
                double val;
                if (HasConstantValue(set, pars, out val))
                {
                    generator.Emit(OpCodes.Ldc_R4, (float)val);
                    return;
                }

                src1.GenerateSingle(generator, set, pars);
                src2.GenerateSingle(generator, set, pars);

                // We just generate add.
                generator.Emit(OpCodes.Mul);
            }

            public void GenerateDouble(ILGenerator generator, FunctionSet set, FunctionParams pars)
            {
                double val;
                if (HasConstantValue(set, pars, out val))
                {
                    generator.Emit(OpCodes.Ldc_R8, val);
                    return;
                }

                src1.GenerateDouble(generator, set, pars);
                src2.GenerateDouble(generator, set, pars);

                // We just generate add.
                generator.Emit(OpCodes.Mul);
            }


            public bool HasConstantValue(FunctionSet set, FunctionParams pars, out double val)
            {
                double s1, s2;
                if (src1.HasConstantValue(set, pars, out s1))
                {
                    if (src2.HasConstantValue(set, pars, out s2))
                    {
                        val = s1 * s2;
                        return true;
                    }

                    // We check if src is 0.0.
                    if (s1 == 0.0)
                    {
                        val = 0.0;
                        return true;
                    }

                }

                // Src2 may be 0.0.
                if (src2.HasConstantValue(set, pars, out s2))
                {
                    if (s2 == 0.0)
                    {
                        val = 0.0;
                        return true;
                    }
                }

                val = 0.0;
                return false;
            }

            public void AddNamedValues(List<string> values)
            {
                src1.AddNamedValues(values);
                src2.AddNamedValues(values);
            }

            #endregion
        }

        internal class Add : IElement
        {
            IElement src1;
            IElement src2;
            public Add(IElement s1, IElement s2) 
            { 
                src1 = s1;
                src2 = s2; 
            }

            #region IElement Members

            public void GenerateSingle(ILGenerator generator, FunctionSet set, FunctionParams pars)
            {
                double val;
                if (HasConstantValue(set, pars, out val))
                {
                    generator.Emit(OpCodes.Ldc_R4, (float)val);
                    return;
                }

                src1.GenerateSingle(generator, set, pars);
                src2.GenerateSingle(generator, set, pars);

                // We just generate add.
                generator.Emit(OpCodes.Add);
            }

            public void GenerateDouble(ILGenerator generator, FunctionSet set, FunctionParams pars)
            {
                double val;
                if (HasConstantValue(set, pars, out val))
                {
                    generator.Emit(OpCodes.Ldc_R8, val);
                    return;
                }

                src1.GenerateDouble(generator, set, pars);
                src2.GenerateDouble(generator, set, pars);

                // We just generate add.
                generator.Emit(OpCodes.Add);
            }


            public bool HasConstantValue(FunctionSet set, FunctionParams pars, out double val)
            {
                double s1, s2;
                if (src1.HasConstantValue(set, pars, out s1))
                {
                    if (src2.HasConstantValue(set, pars, out s2))
                    {
                        val = s1 + s2;
                        return true;
                    }
                }
                val = 0.0;
                return false;
            }

            public void AddNamedValues(List<string> values)
            {
                src1.AddNamedValues(values);
                src2.AddNamedValues(values);
            }

            #endregion
        }

        internal class Substract : IElement
        {
            IElement src1;
            IElement src2;
            public Substract(IElement s1, IElement s2)
            {
                src1 = s1;
                src2 = s2;
            }

            #region IElement Members

            public void GenerateSingle(ILGenerator generator, FunctionSet set, FunctionParams pars)
            {
                double val;
                if (HasConstantValue(set, pars, out val))
                {
                    generator.Emit(OpCodes.Ldc_R4, (float)val);
                    return;
                }

                src1.GenerateSingle(generator, set, pars);
                src2.GenerateSingle(generator, set, pars);

                // We just generate add.
                generator.Emit(OpCodes.Sub);

            }

            public void GenerateDouble(ILGenerator generator, FunctionSet set, FunctionParams pars)
            {
                double val;
                if (HasConstantValue(set, pars, out val))
                {
                    generator.Emit(OpCodes.Ldc_R8, val);
                    return;
                }

                src1.GenerateDouble(generator, set, pars);
                src2.GenerateDouble(generator, set, pars);

                // We just generate add.
                generator.Emit(OpCodes.Sub);
            }


            public bool HasConstantValue(FunctionSet set, FunctionParams pars, out double val)
            {
                double s1, s2;
                if (src1.HasConstantValue(set, pars, out s1) && src2.HasConstantValue(set, pars, out s2))
                {
                    val = s1 - s2;
                    return true;
                }
                val = 0.0;
                return false;
            }

            public void AddNamedValues(List<string> values)
            {
                src1.AddNamedValues(values);
                src2.AddNamedValues(values);
            }

            #endregion
        }

        internal class Power : IElement
        {
            private delegate double PowDelegate(double x, double a);

            IElement src1;
            IElement src2;
            public Power(IElement s1, IElement s2)
            {
                src1 = s1;
                src2 = s2;
            }

            #region IElement Members

            public void GenerateSingle(ILGenerator generator, FunctionSet set, FunctionParams pars)
            {
                double val;
                if (HasConstantValue(set, pars, out val))
                {
                    generator.Emit(OpCodes.Ldc_R4, (float)val);
                    return;
                }

                // We must make sure all params are in double form!
                src1.GenerateSingle(generator, set, pars);
                generator.Emit(OpCodes.Conv_R8);
                src2.GenerateSingle(generator, set, pars);
                generator.Emit(OpCodes.Conv_R8);

                // We generate the call on those two params.
                PowDelegate pow = global::System.Math.Pow;
                generator.EmitCall(OpCodes.Call, pow.Method, null);

                // We convert back to float, which is expected.
                generator.Emit(OpCodes.Conv_R4);

            }

            public void GenerateDouble(ILGenerator generator, FunctionSet set, FunctionParams pars)
            {
                double val;
                if (HasConstantValue(set, pars, out val))
                {
                    generator.Emit(OpCodes.Ldc_R8, val);
                    return;
                }

                // We must make sure all params are in double form!
                src1.GenerateDouble(generator, set, pars);
                src2.GenerateDouble(generator, set, pars);

                // We generate the call on those two params.
                PowDelegate pow = global::System.Math.Pow;
                generator.EmitCall(OpCodes.Call, pow.Method, null);
            }


            public bool HasConstantValue(FunctionSet set, FunctionParams pars, out double val)
            {
                double s1, s2;
                if (src1.HasConstantValue(set, pars, out s1))
                {
                    if(src2.HasConstantValue(set, pars, out s2))
                    {
                        val = global::System.Math.Pow(s1, s2);
                        return true;
                    }

                    // Anything 1.0^x is 1.0.
                    if (s1 == 1.0)
                    {
                        val = 1.0;
                        return true;
                    }
                }

                if (src2.HasConstantValue(set, pars, out s2))
                {
                    // Anything x^0.0 is 1.0 (except 0.0^0.0 what is not defined ...).
                    if (s2 == 0.0)
                    {
                        val = 1.0;
                        return true;
                    }
                }

                val = 0.0;
                return false;
            }

            public void AddNamedValues(List<string> values)
            {
                src1.AddNamedValues(values);
                src2.AddNamedValues(values);
            }

            #endregion
        }
        #endregion

        #region Private Members

        private Expression(string n, string expr)
        {
            name = n;
            List<ParseCode> stream = ConvertToTokenStream(expr);
            roots = SplitOnComma(stream);
        }

        /// <summary>
        /// Expression from root element.
        /// </summary>
        /// <param name="r">The root element.</param>
        internal Expression(IElement r)
        {
            roots = new IElement[] { r };
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Maximum number of variables possible.
        /// </summary>
        public const uint MaxVariableCount = 4;

        /// <summary>
        /// Maximum number of dimensions supported, e.g. resolution of
        /// output parameter.
        /// </summary>
        public const uint MaxDimensionCount = 4;

        /// <summary>
        /// Returns the argument binding, used by function parameters.
        /// </summary>
        /// <param name="pos">The position of parameter. Must not exceed maximum.</param>
        /// <returns></returns>
        public VariableBinding Variable(uint pos)
        {
            if (pos >= MaxVariableCount) throw new ArgumentException("The variable id must be in range [0, " 
                + MaxVariableCount.ToString() + ")");

            return new VariableBinding(pos, this);
        }

        /// <summary>
        /// Obtains function parameters. Parameters are by default auto bound. The binding is
        /// at follows:
        /// - x is bound to variable(0)
        /// - y is bound to variable(1)
        /// - z is bound to variable(2)
        /// - w is bound to variable(3)
        /// - e is bound to e constant, pi is bound to pi constant.
        /// </summary>
        public FunctionParams Params
        {
            get
            {
                lock (root)
                {
                    // We add values.
                    List<string> namedVals = new List<string>();
                    root.AddNamedValues(namedVals);
                    
                    // We construct params.
                    return new FunctionParams(namedVals, this, true);
                }
            }
        }

        /// <summary>
        /// Obtains function parameters. 
        /// <param name="autoBind">Are parameters auto bound. The binding is
        /// at follows:
        /// - x is bound to variable(0)
        /// - y is bound to variable(1)
        /// - z is bound to variable(2)
        /// - w is bound to variable(3)
        /// - e is bound to e constant, pi is bound to pi constant.</param>
        /// </summary>
        public FunctionParams GetParams(bool autoBind)
        {
            lock (root)
            {
                // We add values.
                List<string> namedVals = new List<string>();
                root.AddNamedValues(namedVals);

                // We construct params.
                return new FunctionParams(namedVals, this, autoBind);
            }
        }


        /// <summary>
        /// Constructs expression from string.
        /// </summary>
        /// <param name="expr">The expression.</param>
        /// <returns>Expression instance.</returns>
        public static Expression Parse([NotEmpty] string expr)
        {
            return new Expression(string.Empty, expr);
        }

        /// <summary>
        /// Constructs expression from string and name. 
        /// </summary>
        /// <param name="name">Name is only symbolic and can be used when debugging.</param>
        /// <param name="expr">The expression.</param>
        /// <returns>Expression instance.</returns>
        public static Expression Parse([NotNull] string name, [NotEmpty] string expr)
        {
            return new Expression(name, expr);
        }

        #endregion
    }


}
