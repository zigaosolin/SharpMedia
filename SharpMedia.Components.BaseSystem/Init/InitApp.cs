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

using SharpMedia.Components.Applications;
using SharpMedia.Components.Configuration;
using SharpMedia.Components.Services;
using SharpMedia.Components.TextConsole;
using SharpMedia.Components.Configuration.ComponentProviders;
using SharpMedia.Threading.Default;
using SharpMedia.Threading;

namespace SharpMedia.Components.BaseSystem.Init
{
    /// <summary>
    /// Initialization application.
    /// </summary>
    internal class InitApp : Application
    {
        #region Private Members
        private IApplicationInstances applicationInstances;
        private IComponentDirectory   componentDirectory;
        private ThreadControl         threadControl;
        #endregion

        #region Properties

        [Required]
        public IApplicationInstances ApplicationInstances
        {
            get { return applicationInstances; }
            set { applicationInstances = value; }
        }

        
        [Required]
        public IComponentDirectory ComponentDirectory
        {
            get { return componentDirectory; }
            set { componentDirectory = value; }
        }

        [Required]
        public ThreadControl ThreadControl
        {
            get { return threadControl; }
            set { threadControl = value; }
        }

        #endregion

        #region Overrides

        public override int Start(string verb, string[] args)
        {
            Console.WriteLine(String.Join(",", args));

            Console.WriteLine(">> INIT, v.0.0.1 Booting SharpMedia ComponentOS (c) 2005-2008");
            Console.WriteLine("Init: Starting Services...");
            (componentDirectory.GetInstance<IServiceRegistry>() as IServiceControl).Start();

            Console.Out.WriteLine("Double Vectors Per Second on a WU Thread: " + 
                (threadControl.WorkUnits.FreeMost(typeof(ICPUWorkUnit)) as ICPUWorkUnit).FPU.DoubleVectorCrosses4D);

            IApplicationInstance proc = 
                applicationInstances.Run(
                    args.Length > 0 ? args[0] : "/System/Applications/Components/TextShell", string.Empty, args);

            while (proc.IsRunning) Thread.Sleep(100);

            Console.WriteLine("Init: Shutting Down...");
            (componentDirectory.GetInstance<IServiceRegistry>() as IServiceControl).Stop();
            return 1;
        }

        #endregion
    }
}
