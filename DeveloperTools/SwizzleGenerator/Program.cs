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
using System.IO;

namespace SwizzleGenerator
{
    class Program
    {

        //static readonly string[] mapper = new string[] { "Floatx1", "Floatx2", "Floatx3", "Floatx4" };
        static readonly string[] mapper = new string[] { "Integerx1", "Integerx2", "Integerx3", "Integerx4" };
        static readonly char[] table = new char[] { 'X', 'Y', 'Z', 'W' };

        static string GenSwizzle(params int[] idx)
        {
            string s = string.Empty;
            for (int i = 0; i < idx.Length; i++)
            {
                s += table[idx[i]];
            }
            return s;
        }



        static string Generate(string outType, string swizzle)
        {
            return string.Format(
"        /// <summary>" + Environment.NewLine +
"        /// The {2} swizzle." + Environment.NewLine +
"        /// </summary>" + Environment.NewLine +
"        public {0} {2}" + Environment.NewLine +
"        {{" + Environment.NewLine + 
"            get" + Environment.NewLine +
"            {{" + Environment.NewLine +
"                SwizzleOperation op = new SwizzleOperation(SwizzleMask.FromString(\"{1}\"));"+ Environment.NewLine +
"                op.BindInputs(pin);" + Environment.NewLine +
"                return new {0}(op.Outputs[0], Generator);" + Environment.NewLine +
"            }}" + Environment.NewLine +
"            [param: NotNull]" + Environment.NewLine +
"            set" + Environment.NewLine +
"            {{" + Environment.NewLine +
"                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.FromString(\"{1}\"));" + Environment.NewLine +
"                op.BindInputs(pin, value.Pin);" + Environment.NewLine +
"                pin = op.Outputs[0];" + Environment.NewLine +
"            }}" + Environment.NewLine +
"        }}" + Environment.NewLine + Environment.NewLine, outType, swizzle, swizzle.ToUpper());

        }


        static void Main(string[] args)
        {
            int c = int.Parse(args[1]);

            using (TextWriter writer = new StreamWriter(args[0]))
            {

                // We generate 1-level swizzles
                for (int i = 0; i < c; i++)
                {
                    writer.Write(Generate(mapper[0], GenSwizzle(i)));
                }

                // We generate 2-level swizzles
                for (int i = 0; i < c; i++)
                {
                    for (int j = 0; j < c; j++)
                    {
                        writer.Write(Generate(mapper[1], GenSwizzle(i, j)));
                    }
                }

                // We generate 3-level swizzles
                for (int i = 0; i < c; i++)
                {
                    for (int j = 0; j < c; j++)
                    {
                        for (int k = 0; k < c; k++)
                        {
                            writer.Write(Generate(mapper[2], GenSwizzle(i, j, k)));
                        }
                    }
                }

                // We generate 4-level swizzles
                for (int i = 0; i < c; i++)
                {
                    for (int j = 0; j < c; j++)
                    {
                        for (int k = 0; k < c; k++)
                        {
                            for (int l = 0; l < c; l++)
                            {
                                writer.Write(Generate(mapper[3], GenSwizzle(i, j, k, l)));
                            }
                        }
                    }
                }
            }
        }
    }
}
