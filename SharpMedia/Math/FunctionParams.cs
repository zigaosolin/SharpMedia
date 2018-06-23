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

using SharpMedia.Testing;
using SharpMedia.AspectOriented;
using SharpMedia.Math.Integration;

namespace SharpMedia.Math
{
    public partial class Expression 
    {
        #region Function Parameter Implementation


        /// <summary>
        /// Variable binding, e.g. the position of argument within delegate. The
        /// position 0 denotes first argument, position 1 second argument. This
        /// class is never directly exposed, only through "object".
        /// </summary>
        public struct VariableBinding
        {
            private uint id;
            private Expression parent;

            /// <summary>
            /// Read-only position of argument.
            /// </summary>
            public uint Position
            {
                get { return id; }
            }

            /// <summary>
            /// Expression this binding is child of.
            /// </summary>
            public Expression Parent
            {
                get { return parent; }
            }

            /// <summary>
            /// The argument binding constructor, actually only id. Never
            /// call this constructor directly because only expression 
            /// </summary>
            /// <param name="i">The id of argument.</param>
            internal VariableBinding(uint i, Expression p)
            {
                id = i;
                parent = p;
            }
        }

        /// <summary>
        /// Parameter resolving and variables of the expression. You should never
        /// constuct the function parameters directly, use Expression.Params.
        /// </summary>
        public class FunctionParams : IEnumerable<string>
        {
            #region Private Members
            private Expression expression;
            private List<string> sortedKeys;
            private SortedList<string, object> values;
            #endregion

            #region Internal Methods

            /// <summary>
            /// Never call this directly, use <see cref="Expression.Params"/>.
            /// </summary>
            /// <param name="keys">Mappable elements.</param>
            internal FunctionParams(List<string> keys, Expression ex, bool autoBind)
            {
                expression = ex;
                keys.Sort();
                sortedKeys = keys;
                values = new SortedList<string, object>(keys.Count);

                if (autoBind)
                {
                    // Some parameters are automatically bound if specified so.
                    foreach (string s in sortedKeys)
                    {
                        switch (s)
                        {
                            case "x":
                                SetBinding("x", ex.Variable(0));
                                break;
                            case "y":
                                SetBinding("y", ex.Variable(1));
                                break;
                            case "z":
                                SetBinding("z", ex.Variable(2));
                                break;
                            case "w":
                                SetBinding("w", ex.Variable(3));
                                break;
                            case "e":
                                this["e"] = global::System.Math.E;
                                break;
                            case "pi":
                                this["pi"] = global::System.Math.PI;
                                break;
                        }
                    }
                }
            }

            #endregion

            #region Public Members

            /// <summary>
            /// Parameter access.
            /// </summary>
            /// <param name="name">Parameter's name.</param>
            public double this[ [NotNull] string name]
            {
                set
                {
                    int index = sortedKeys.BinarySearch(name);
                    if (index >= 0)
                    {
                        values[name] = (object)value;
                    }
                    else
                    {
                        // Ignore, not needed.
                    }
                }
                get
                {
                    return (double)values[name];
                }
            }

            /// <summary>
            /// Sets variable.
            /// </summary>
            /// <param name="name">The name of variable.</param>
            /// <param name="index">Index parameter by
            /// <see cref="Expression.Variable(uint index)"/>.</param>
            public void SetBinding([NotNull] string name, VariableBinding index)
            {
                // Checks.
                if (index.Parent != this.expression) throw new ArgumentException("The index used to set variable was not created by the same expression" +
                                                                                   "this params are used with.");

                // We now add variable.
                int inx = sortedKeys.BinarySearch(name);
                if (inx >= 0)
                {
                    // May overwrite.
                    values[name] = (object)index;
                }
                else
                {
                    throw new ArgumentException("Could not locate the variable "
                        + name + " in named arguments of function.");
                }
            }

            /// <summary>
            /// We obtain the binding, if it exists.
            /// </summary>
            /// <param name="name">Name of variable.</param>
            /// <returns>Asociated binding.</returns>
            public void GetBinding([NotNull] string name, out VariableBinding binding)
            {
                object obj = values[name];
                if (obj == null) throw new ArgumentException("The binding with that name does not exist.");

                if (obj.GetType() != typeof(VariableBinding)){
                    throw new ArgumentException("The binding with that name does not exist, this variable " + 
                                                "is used as parameter.");
                }
                binding = (VariableBinding)obj;
            }

            /// <summary>
            /// Does params have a variable binding for this value.
            /// </summary>
            /// <param name="name">The variable name.</param>
            /// <returns>Is this a binding.</returns>
            public bool HasBinding([NotNull] string name)
            {
                object obj = values[name];
                if (obj == null) return false;
                if (obj.GetType() == typeof(VariableBinding)) return true;
                return false;
            }

            /// <summary>
            /// Is the whole set defined.
            /// </summary>
            public bool IsDefined
            {
                get
                {
                    foreach (string s in sortedKeys)
                    {
                        if (!values.ContainsKey(s)) return false;
                    }
                    return true;
                }
            }

            /// <summary>
            /// Params creator.
            /// </summary>
            public Expression Creator
            {
                get { return expression; }
            }

            /// <summary>
            /// The highest offset of variable being used + 1.
            /// </summary>
            public uint VariableCount
            {
                get
                {
                    int current = -1;

                    foreach (object obj in values.Keys)
                    {
                        if (obj is VariableBinding)
                        {
                            VariableBinding binding = (VariableBinding)obj;
                            if (binding.Position > current) current = (int)binding.Position;
                        }
                    }

                    return (uint)(current + 1);
                }
            }

            #endregion

            #region IEnumerable<string> Members

            public IEnumerator<string> GetEnumerator()
            {
                foreach (string s in sortedKeys)
                {
                    yield return s;
                }
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                foreach (string s in sortedKeys)
                {
                    yield return s;
                }
            }

            #endregion
        }

        #endregion

    }

#if SHARPMEDIA_TESTSUITE
    /// <summary>
    /// Expression testing suite.
    /// </summary>
    [TestSuite]
    public class ExpressionTest
    {
        [CorrectnessTest]
        public void IdentityTest()
        {
            // Construct expression.
            Expression exp = Expression.Parse("x");
            Expression.FunctionParams p = exp.Params;
            p.SetBinding("x", exp.Variable(0));
            Functionf f = exp.GetFunctionf(p, null);

            // Test it.
            for (float x = 0.0f; x < 100.0f; x += 1.0f)
            {
                Assert.AreEqual(f(x), x);
            }
        }


        [CorrectnessTest]
        public void LinearTest()
        {
            // Prepare; x must be auto bound.
            Expression exp = Expression.Parse("linear", "k*x+n");
            Expression.FunctionParams p = exp.GetParams(true);
            p["k"] = 2.0;
            p["n"] = -2.0;
            Functionf f = exp.GetFunctionf(p, FunctionSet.Default);

            Assert.AreEqual(f(1.0f), 0.0f);
            Assert.AreEqual(f(2.0f), 2.0f);
        }

        [CorrectnessTest]
        public void QuadraticTest()
        {
            Expression exp = Expression.Parse("quadratic", "(3+2)^2*x^2+(7/4)*x+(x/4)");
            Expression.FunctionParams p = exp.Params;
            Functionf f = exp.GetFunctionf(p, FunctionSet.Default);

            Assert.AreEqual(f(1.0f), (25.0f + 7.0f / 4.0f + 0.25f));
        }

        [CorrectnessTest]
        public void Call()
        {
            Expression exp = Expression.Parse("sin(x)");
            Functionf f = exp.GetFunctionf(exp.Params, FunctionSet.Default);
            Assert.AreEqual(f(0.0f), 0.0);
        }

        [CorrectnessTest]
        public void Multicall()
        {
            Expression exp = Expression.Parse("log(x, 10)");
            Functionf f = exp.GetFunctionf(exp.Params, FunctionSet.Default);
            Assert.AreEqual(f(1.0f), 0.0f);
        }

        [CorrectnessTest]
        public void MultiArg()
        {
            Expression exp = Expression.Parse("log(x, 10)*ln(y)*z");
            Function3f f = exp.GetFunction3f(exp.Params, FunctionSet.Default);
            Assert.AreEqual(f(2.0f, 1.0f, 0.5f), 0.0f);
        }

        float fx(float x)
        {
            return x * (x - 1.0f) + 2.0f;
        }

        double fxd(double x)
        {
            return x * (x - 1.0) + 2.0;
        }

        /*
        [Assert]
        public bool PerfTest1()
        {
            // We create comparison test.
            Expression exp = Expression.Parse("x * ( x -1) + n");
            Expression.FunctionParams p0 = exp.GetParams(false);
            p0.SetBinding("x", exp.Variable(0));
            p0["n"] = 2.0;
            Functionf f = exp.GetFunctionf(p0, FunctionSet.Default);
            float upperLimit = (float)Int32.Parse("1000000");

            // We make a loop.
            DateTime t0 = DateTime.Now;
            float t = 0.0f;
            for (float i = 0; i < upperLimit; i++)
            {
                t += f(i);
            }

            DateTime t1 = DateTime.Now;

            for (float j = 0; j < upperLimit; j++)
            {
                t += fx(j);
            }

            DateTime t2 = DateTime.Now;

            TimeSpan sp1 = t1 - t0;
            TimeSpan sp2 = t2 - t1;

            return true;
        }*/
        /*

        [CorrectnessTest]
        public void PerfTest2()
        {
            // Prepate.
            Expression exp = Expression.Parse("x * ( x -1) + n");
            Expression.FunctionParams p = exp.GetParams(false);
            p.SetBinding("x", exp.Variable(0));
            p["n"] = 2.0;
            Functiond f = exp.GetFunctiond(p, FunctionSet.Default);

            

            DateTime t0 = DateTime.Now;
            Integrator.MonteCarlo(f, new Intervald(0.0, 2.0), 10000000, 10000, new Random.NativeGenerator());
            DateTime t1 = DateTime.Now;
            Integrator.MonteCarlo(fxd, new Intervald(0.0, 2.0), 10000000, 10000, new Random.NativeGenerator());
            DateTime t2 = DateTime.Now;

            TimeSpan sp1 = t1 - t0;
            TimeSpan sp2 = t2 - t1;

            // We allow our delegate to perform only 100% slower.
            Assert.IsTrue((sp2.Ticks / sp1.Ticks) <= 1);
            
        }*/

        [CorrectnessTest]
        internal void MultiDimToSingle()
        {
            Expression exp = Expression.Parse("x^x,x^(x^x+1),x*x");
            Expression.FunctionParams p = exp.GetParams(false);
            p.SetBinding("x", exp.Variable(0));
            Functiond f = exp.GetFunctiond(p, FunctionSet.Default);

            Assert.AreEqual(f(2.0f), 4.0f);
        }

    }
#endif
}
