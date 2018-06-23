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
using SharpMedia.Components.Applications;
using SharpMedia.Components.Database;

namespace SharpMedia.Components.Kernel.Applications.DomainBased
{
    /// <summary>
    /// An ApplicationInstance implementation
    /// </summary>
    internal class AppDomainApplication : 
        MarshalByRefObject, IOSApplication, IApplicationInstance, IApplicationCallback, IApplicationBase
    {
        AppDomain           appDomain             = null;
        IOSApplication      osApplication         = null;
        ApplicationDesc     applicationDescriptor = null;
        Bridge              bridge                = null;
        ulong               instanceNumber        = 0;
        Guid                instanceGuid          = Guid.Empty;

        /// <summary>
        /// Application instance that lives in a dedicated app domain
        /// </summary>
        /// <param name="parentDirectory"></param>
        /// <param name="application"></param>
        /// <param name="instanceGuid"></param>
        internal AppDomainApplication(
            IComponentDirectory parentDirectory, 
            ApplicationDesc application, 
            Guid instanceGuid, 
            ulong instanceNumber)
        {
            this.instanceGuid   = instanceGuid;
            this.instanceNumber = instanceNumber;

            string iname        = application.Name + " " + instanceGuid.ToString();
            appDomain           = AppDomain.CreateDomain(iname);

            /* create instance and unwrap the caller so that all the stuff happens in its own domain */
            this.bridge = appDomain.CreateInstanceAndUnwrap(
                typeof(Bridge).Assembly.FullName,
                typeof(Bridge).FullName) as Bridge;

            this.applicationDescriptor = application;

            /* set up the instance */
            this.bridge.Setup(parentDirectory, application, instanceGuid, this, parentDirectory.GetInstance<IAssemblyLoader>());
            this.bridge.Callback = this;
        }

        #region IOSApplication Members

        public bool Kill()
        {
            if (osApplication.Kill())
            {
                this.isRunning     = false;
                this.exitResult    = ~0;
                this.exitException = new Exception("Application Killed");

                Cleanup();

                return true;
            }

            return false;
        }

        private void Cleanup()
        {
            if (appDomain != null)
            {
                AppDomain.Unload(appDomain); appDomain = null; 
            }
        }

        bool isRunning;
        public bool IsRunning
        {
            get { return isRunning; }
        }

        Exception exitException;
        public Exception ExitException
        {
            get { return exitException; }
        }

        IApplicationCallback callback;
        public IApplicationCallback Callback
        {
            set { this.callback = value; }
        }

        int exitResult;
        public int ExitResult
        {
            get { return exitResult; }
        }

        #endregion

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

        #endregion

        #region IApplicationCallback Members

        public void Exited(IApplicationInstance sender, int returnValue)
        {
            this.isRunning = false;
            this.exitResult = returnValue;
            this.exitException = null;

            this.callback.Exited(this, returnValue);
            Cleanup();
        }

        public void Exited(IApplicationInstance sender, Exception exception)
        {
            this.isRunning = false;
            this.exitException = exception;
            this.exitResult = ~0;

            this.callback.Exited(this, exception);
            Cleanup();
        }

        #endregion

        #region IApplicationBase Members

        public int Start(string verb, params string[] args)
        {
            bridge.Start(verb, args);
            return 0;
        }

        #endregion
    }
}
