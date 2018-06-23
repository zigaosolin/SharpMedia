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
using System.Threading;
using SharpMedia.Components.Configuration;
using SharpMedia.Components.Configuration.ComponentProviders;
using SharpMedia.Components.Database;
using SharpMedia.Components.Applications;
using SharpMedia.Database;

namespace SharpMedia.Components.Kernel.Applications.DomainBased
{
    /// <summary>
    /// Lives in the app domain and supervises the appdomain-local initialization and
    /// running of the application
    /// </summary>
    [Ignore]
    internal class Bridge : MarshalByRefObject, IOSApplication
    {
        Thread               thread         = null;
        IApplicationCallback callback       = null;
        IComponentDirectory  directory      = null;
        IApplicationBase     appInstance    = null;
        string               verb           = string.Empty;
        string[]             arguments      = new string[0];

        static IAssemblyLoader assemblyLoader = null;

        void BridgeMain()
        {
            try
            {
                callback.Exited(null, (appInstance as IApplicationBase).Start(verb, arguments));
            }
            catch (Exception e)
            {
                callback.Exited(null, e);
            }
        }

        #region IOSApplication Members

        public bool Kill()
        {
            if (thread != null && thread.IsAlive)
            {
                thread.Abort();
            }

            return true;
        }

        public IApplicationCallback Callback
        {
            set { this.callback = value; }
        }

        public bool IsRunning
        {
            set { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        /// <summary>
        /// Sets up the bridge in the new appdomain
        /// </summary>
        /// <param name="parentDirectory">Parent component directory</param>
        /// <param name="app">Application descriptor</param>
        /// <param name="guid">Application instance GUID</param>
        /// <param name="appInstance">Self application instance to link into the directory</param>
        internal void Setup(IComponentDirectory parentDirectory, ApplicationDesc app, Guid guid, IApplicationInstance appInstance, IAssemblyLoader xassemblyLoader)
        {
            assemblyLoader = xassemblyLoader;

            string iname = app.Name + " " + guid;

            // Console.Out.WriteLine("Creating process: {0}", iname);
            AppDomain.CurrentDomain.TypeResolve += new ResolveEventHandler(CurrentDomain_TypeResolve);
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            this.directory = new ComponentDirectory(parentDirectory, iname);
            this.directory.Register(
                new Instance(
                    this.directory.GetInstance<DatabaseManager>().Find("/")));

            IComponentProvider appProvider = null;

            /** register self as process **/
            this.directory.Register(
                new Instance(appInstance, "Self"));

            /******************************************************************************************* 
             * TODO: SECURITY: this is where we would re-register views of already registered (kernel)
             * providers to shadow the originals
             *******************************************************************************************/

            foreach (IComponentProvider provider in app.Components)
            {
                this.directory.Register(provider);

                if (provider.MatchedName == app.ApplicationComponent)
                {
                    appProvider = provider;
                }
            }

            if (appProvider == null)
            {
                throw new Exception(
                    String.Format(
                        "The application {0} cannot be set up because the application component was not found",
                        app.Name));
            }

            this.appInstance = this.directory.GetInstance(appProvider.MatchedName) as IApplicationBase;

            if (app == null)
            {
                throw new Exception(
                    String.Format(
                        "The application {0} cannot be set up because the application component cannot be instantiated",
                        app.Name));
            }

            /* a-ok */
        }

        /// <summary>
        /// Starts the application
        /// </summary>
        /// <param name="arguments">If the application is command line based, its arguments;
        /// if the application is document based, first entry is the verb, second is the path</param>
        public int Start(string verb, params string[] arguments)
        {
            this.verb = verb;
            this.arguments = arguments.Clone() as string[];

            thread = new Thread(BridgeMain);
            thread.Start();

            return 0;
        }

        static System.Reflection.Assembly CurrentDomain_TypeResolve(object sender, ResolveEventArgs args)
        {
            Console.Out.WriteLine(".. Probing Type {0}", args.Name);
            string tmp = "";
            foreach (string sub in args.Name.Split('.'))
            {
                tmp = tmp == "" ? sub : tmp + "." + sub;
                if (tmp == string.Empty) continue;

                System.Reflection.Assembly asm = assemblyLoader.Load(tmp);
                if (asm.GetType(args.Name) != null) return asm;
            }
            Console.Out.WriteLine("> Failed to get type {0}", args.Name);

            return null;
        }

        static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Console.Out.WriteLine(".. Probing Assembly {0}", args.Name);
            return assemblyLoader.Load(args.Name);
        }
    }
}
