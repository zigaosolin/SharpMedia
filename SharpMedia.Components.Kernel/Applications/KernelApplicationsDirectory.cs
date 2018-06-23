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
using SharpMedia.Components.Database;
using SharpMedia.Components.Kernel.Configuration;
using SharpMedia.Components.Configuration;
using SharpMedia.Components.Applications;
using SharpMedia.Components.Kernel.Applications.Local;
using SharpMedia.Database;

namespace SharpMedia.Components.Kernel
{
    /// <summary>
    /// Implements the ApplicationsDirectory component
    /// </summary>
    [IgnoreInterface(typeof(IApplicationCallback)), IgnoreBaseClass]
    internal class KernelApplicationsDirectory : MarshalByRefObject, IApplicationInstances, IApplicationCallback
    {
        internal void ApplicationStarted(IApplicationInstance osApp) 
        {
            applicationInstances.Add(osApp);
        }

        internal void ApplicationExited(IApplicationInstance osApp, Exception exc)
        {
            applicationInstances.Remove(osApp);
        }

        internal void ApplicationExited(IApplicationInstance osApp, int rval)
        {
            applicationInstances.Remove(osApp);
        }

        private IComponentDirectory componentDirectory;
        [Required]
        public IComponentDirectory ComponentDirectory
        {
            get { return componentDirectory; }
            set { componentDirectory = value; }
        }

        private DatabaseManager databaseManager;
        [Required]
        public DatabaseManager DatabaseManager
        {
            get { return databaseManager; }
            set { databaseManager = value; }
        }

        List<IApplicationInstance> applicationInstances = new List<IApplicationInstance>();

        #region IApplicationInstanceManagement Members

        public IApplicationInstance[] FindInstances(Guid applicationId)
        {
            return applicationInstances.FindAll(
                delegate(IApplicationInstance instance)
                {
                    return instance.ApplicationInfo.Id == applicationId;
                }).ToArray();
        }

        public IApplicationInstance FindInstance(Guid instanceId)
        {
            return applicationInstances.Find(
                delegate(IApplicationInstance instance)
                {
                    return instance.InstanceId == instanceId;
                });
        }

        private ulong NewInstanceNumber(ApplicationDesc app)
        {
            List<IApplicationInstance> instances = this.applicationInstances.FindAll(
                delegate(IApplicationInstance instance)
                {
                    return instance.ApplicationInfo.Id == app.Id;
                });

            ulong instanceNumber = 0;
            if (instances.Count > 0)
            {
                instanceNumber = instances[instances.Count - 1].InstanceNumber + 1;
            }

            return instanceNumber;
        }

        public IApplicationInstance Run(string path, string verb, string[] args)
        {
            ApplicationDesc app = null;
            try
            {
                app = databaseManager.Find<ApplicationDesc>(path).Object;
            }
            catch (Exception)
            {
            }

            if (app == null)
            {
                throw new Exception(
                    String.Format(
                        "There is no application present at: '{0}'", path));
            }

            LocalApplication application = new LocalApplication(
                this.componentDirectory,
                app, Guid.NewGuid(), NewInstanceNumber(app));

            application.Callback = this;
            applicationInstances.Add(application);


            

            application.Start(verb, args);

            return application;
        }

        public void Kill(IApplicationInstance app)
        {
            if (app is IOSApplication)
            {
                (app as IOSApplication).Kill();
            }
            else throw new Exception("Invalid application instance");
        }

        public IApplicationInstance[] Instances
        {
            get { return this.applicationInstances.ToArray(); }
        }

        public IApplicationInstance Run(string path, string verb, string[] args, IComponentDirectory environ)
        {
            ApplicationDesc app = null;
            try
            {
                app = databaseManager.Find<ApplicationDesc>(path).Object;
            }
            catch (Exception)
            {
            }

            if (app == null)
            {
                throw new Exception(
                    String.Format(
                        "There is no application present at: '{0}'", path));
            }

            ComponentDirectory localEnvironment = new ComponentDirectory(this.componentDirectory, path);
            foreach (IComponentProvider component in environ.RegisteredProviders)
            {
                localEnvironment.Register(component);
            }

            LocalApplication application = new LocalApplication(
                localEnvironment,
                app, Guid.NewGuid(), NewInstanceNumber(app));

            application.Callback = this;
            applicationInstances.Add(application);
            application.Start(verb, args);

            return application;
        }

        #endregion

        #region IApplicationCallback Members

        public void Exited(IApplicationInstance sender, int returnValue)
        {
            applicationInstances.RemoveAll(delegate(IApplicationInstance instance)
            {
                return instance.InstanceId == sender.InstanceId;
            });

            //Console.Out.WriteLine("Kernel: {0} has exited with code {1}", sender.ApplicationInfo.Name, returnValue);
        }

        public void Exited(IApplicationInstance sender, Exception exception)
        {
            applicationInstances.RemoveAll(delegate(IApplicationInstance instance)
            {
                return instance.InstanceId == sender.InstanceId;
            });

            //Console.Out.WriteLine("Kernel: {0} has exited with exception\n{1}", sender.ApplicationInfo.Name, exception);
        }

        #endregion
    }
}
