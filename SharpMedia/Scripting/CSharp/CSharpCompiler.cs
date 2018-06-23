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
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using SharpMedia.Testing;
using System.Reflection;
using SharpMedia.Math;

namespace SharpMedia.Scripting.CSharp
{


    /// <summary>
    /// A C# compiler.
    /// </summary>
    public sealed class CSharpCompiler : IScriptCompiler
    {
        #region Constructors

        public CSharpCompiler()
        {
        }

        #endregion

        #region IScriptCompiler Members

        public IScript Compile(object data)
        {
            // We first cast it.
            CSharpCode code = data as CSharpCode;

            // We create provider.
            using (CSharpCodeProvider provider = new CSharpCodeProvider())
            {
                CompilerParameters parameters = new CompilerParameters(code.ReferencedAssemblies);
                parameters.GenerateExecutable = false;
                parameters.GenerateInMemory = true;
                parameters.IncludeDebugInformation = false;
                
                
                // We compile it.
                CompilerResults results = provider.CompileAssemblyFromSource(parameters, code.Code);
                if (results.Errors.HasErrors)
                {
                    // We output errors.
                    for (int i = 0; i < results.Errors.Count; i++)
                    {
                        if (results.Errors[i].IsWarning)
                        {
                            Common.Warning(typeof(CSharpCompiler), results.Errors[i].ToString());
                        }
                        else
                        {
                            Common.Error(typeof(CSharpCompiler), results.Errors[i].ToString());
                        }
                    }

                    // We just throw exception with the first error.
                    throw new ArgumentException("At least one compiler error: " + results.Errors[0].ToString());
                }

                // We return the script.
                return new CSharpScript(results.CompiledAssembly, code.EntryPoint);
            }
            

        }

        public Type ScriptCodeType
        {
            get { return typeof(CSharpCode); }
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE

    [TestSuite]
    internal class CSharpCompilerTest
    {
        CSharpCode code = new CSharpCode(
                "using System;\n" +
                "using SharpMedia.Math;\n" +
                "namespace SharpMedia.Scripting.CSharp.TestSuite {\n" +
                "  public sealed class MyClass {\n" +
                "    Vector3f a;\n" +
                "    public MyClass(Vector3f a) { this.a = a; }\n" +
                "  public override string ToString() { return a.ToString(); }\n" +
                "  }\n" +
                "}\n",
                "SharpMedia.Scripting.CSharp.TestSuite.MyClass",
                "System.dll", "SharpMedia.dll");

        [CorrectnessTest]
        public void RuntimeCompile()
        {
            CSharpCompiler compiler = new CSharpCompiler();

            IScript script = compiler.Compile(code);

            Type type = script.ObjectType;
        }

        [CorrectnessTest]
        public void RuntimeCompile2()
        {
            ScriptEngine.AddScriptCompiler(new CSharpCompiler());

            object obj = ScriptEngine.ImportAndRun(code, new Vector3f(1, -5, 1));
            //Console.WriteLine(obj.ToString());
        }
    }

#endif
}
