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
using System.Xml;
using SharpMedia.AspectOriented;
using SharpMedia.Graphics.GUI.Compiler.Emit;
using System.IO;
using SharpMedia.Testing;

namespace SharpMedia.Graphics.GUI.Compiler
{


    /// <summary>
    /// A Gui importer, acts as a component.
    /// </summary>
    public class GuiCompiler
    {

        public GuiCompiler()
        {
        }

        /// <summary>
        /// Imports an XML document.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public CompileResult Compile(string file, [NotNull] CompileOptions options)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(file);

            return Compile(doc, options);
        }

        /// <summary>
        /// Imports an XML doculemt.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public CompileResult Compile([NotNull] XmlDocument document, [NotNull] CompileOptions options)
        {
            ASTApplication application = new Emit.ASTApplication();

            //try
            {
                // We create context.
                CompileContext context = new CompileContext(options);
                

                // 1) Parse it.
                application.Parse(context, document["Application"]);

                // 2) Resolve it.
                ResolveObject(context, null, application);

                // 3) Emit it.
                using (FileStream file = File.Create(options.OutputFile))
                {
                    using (TextWriter writer = new StreamWriter(file))
                    {
                        application.Emit(context, writer);
                    }
                }

            }
            //catch (GuiCompilingException ex)
            {
                CompileResult result = new CompileResult();
                result.IsSuccessful = false;
                //result.Errors = new Message(ex.Message)
                return result;
            }

            CompileResult r = new CompileResult();
            r.IsSuccessful = true;
            return r;
        }

        void ResolveObject(CompileContext context, Resolver resolver, IElement element)
        {
            if (element is IResolvable)
            {
                (element as IResolvable).Resolve(context, resolver);
            }

            foreach (IElement child in element.Children)
            {
                ResolveObject(context, resolver, child);
            }
        }

    }

#if SHARPMEDIA_TESTSUITE

    [TestSuite]
    internal class GuiCompilerTest
    {
        [CorrectnessTest]
        public void SampleParse()
        {
            CompileOptions options = new CompileOptions();
            options.OutputFile = "sample.cs";
            options.ConfigurationFile = null;

            GuiCompiler compiler = new GuiCompiler();
            CompileResult result = compiler.Compile(@"E:\SharpMedia\trunk\BuildOutput\testgui.xml", options);
        }
    }

#endif
}
