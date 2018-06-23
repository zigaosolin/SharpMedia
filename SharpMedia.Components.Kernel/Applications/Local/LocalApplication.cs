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
using SharpMedia.Components.Configuration;
using System.Threading;
using SharpMedia.Components.Applications;
using SharpMedia.Components.Database;
using SharpMedia.Components.Configuration.ComponentProviders;
using SharpMedia.Database;

namespace SharpMedia.Components.Kernel.Applications.Local
{
    /// <summary>
    /// A Local application that lives in the callers' app domain
    /// </summary>
    [Serializable]
    internal class LocalApplication : 
        IOSApplication, IApplicationInstance, IApplicationCallback, IApplicationBase
    {
        IOSApplication      osApplication         = null;
        ApplicationDesc     applicationDescriptor = null;
        ulong               instanceNumber        = 0;
        Guid                instanceGuid          = Guid.Empty;
        Thread              applicationThread     = null;
        IApplicationBase    appInstance           = null;
        string              verb                  = string.Empty;
        string[]            args                  = new string[0];
        IComponentDirectory localDirectory        = null;

        internal LocalApplication(
            IComponentDirectory parentDirectory,
            ApplicationDesc application,
            Guid instanceGuid,
            ulong instanceNumber)
        {
            this.instanceGuid = instanceGuid;
            this.instanceNumber = instanceNumber;

            string iname = application.Name + " " + instanceGuid.ToString();

            this.applicationDescriptor = application;

            localDirectory = new ComponentDirectory(parentDirectory, iname);

            localDirectory.Register(
                new Instance(
                    localDirectory.GetInstance<DatabaseManager>().Find("/"), "WorkingDirectory"));

            localDirectory.Register(new Instance(this,"Process"));

            foreach (IComponentProvider provider in application.Components)
            {
                localDirectory.Register(provider);
            }
        }


        string[] RegisterAutoParametrization(IComponentDirectory destination, string[] args)
        {
            // We must add auto parametrization.
            ConfiguredComponent typeProvider = destination.FindByName(
                applicationDescriptor.ApplicationComponent)[0] as ConfiguredComponent;

            Type type = typeProvider.ComponentType;
            if (type.GetCustomAttributes(typeof(AutoParametrizeAttribute), true).Length > 0)
            {
                return AutoParametrization.PerformAutoParametrization(type, localDirectory, args);
            }
            return args.Clone() as string[];
        }

        void LocalMain()
        {
            try
            {
                Exited(null, (appInstance as IApplicationBase).Start(this.verb, this.args));
            }
            catch (Exception e)
            {
                Exited(null, e);
            }
        }

        #region IApplicationInstance Members

        public ulong InstanceNumber
        {
            get { return instanceNumber; }
        }

        public Guid InstanceId
        {
            get { return instanceGuid; }
        }

        public IApplicationInfo ApplicationInfo
        {
            get { return applicationDescriptor; }
        }

        bool isRunning;
        public bool IsRunning
        {
            get { return isRunning; }
        }

        int exitResult;
        public int ExitResult
        {
            get { return exitResult; }
        }

        Exception exitException;
        public Exception ExitException
        {
            get { return exitException; }
        }

        #endregion

        #region IApplicationCallback Members

        public void Exited(IApplicationInstance sender, int returnValue)
        {
            callback.Exited(this, returnValue);
            isRunning = false;
            exitResult = returnValue;
            exitException = null;
        }

        public void Exited(IApplicationInstance sender, Exception exception)
        {
            callback.Exited(this, exception);
            isRunning = false;
            exitException = exception;
            exitResult = ~0;
        }

        #endregion

        #region IAplicationBase Members

        public int Start(string verb, string[] arguments)
        {
            // We auto-parametrize.
            this.verb = verb;
            this.args = RegisterAutoParametrization(localDirectory, arguments);

            // Only create on first start.
            if (appInstance == null)
            {
                appInstance = localDirectory.GetInstance(
                    applicationDescriptor.ApplicationComponent) as IApplicationBase;
            }

            applicationThread = new Thread(LocalMain);
            isRunning = true;
            applicationThread.Start();

            return exitResult;
        }

        #endregion

        #region IOSApplication Members

        public bool Kill()
        {
            if (applicationThread != null && applicationThread.IsAlive)
            {
                applicationThread.Abort();
                isRunning     = false;
                exitResult    = ~0;
                exitException = new Exception("Application Killed");

                return true;
            }

            return false;
        }

        IApplicationCallback callback;
        public IApplicationCallback Callback
        {
            set { callback = value; }
        }

        #endregion
    }
}
