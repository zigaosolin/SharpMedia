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
using System.Runtime.CompilerServices;
using SharpMedia.Database;
using SharpMedia.Components.Services;
using SharpMedia.Components.Configuration;
using SharpMedia.Components.Database;

namespace SharpMedia.Components.Kernel.Services
{
    /// <summary>
    /// A Service Manager
    /// </summary>
    public sealed class ServiceManager : IServiceRegistry, IServiceControl, IDisposable
    {
        #region Private Members
        static readonly string serviceLocation = "/System/Runtime/Services";
        object innerLock = new object();
        Dictionary<string, Service> services = new Dictionary<string, Service>();
        volatile bool runCheckThread = true;
        Thread checkThread = null;
        private IComponentDirectory componentDirectory;
        private IApplicationInstances appInstances;
        private DatabaseManager databaseManager;
        #endregion


        #region Private Methods

        void Dispose(bool fin)
        {
            if (runCheckThread == false) return;
            runCheckThread = false;
            checkThread.Join();
            checkThread = null;

            if (!fin) GC.SuppressFinalize(this);

        }

        ~ServiceManager()
        {
            Dispose(true);
        }

        void CheckMain()
        {
            while (runCheckThread)
            {
                lock (innerLock)
                {
                    /* FIXME: This is crude... in the future we want to have stuff */
                    foreach (Service serv in StoppedServices)
                    {
                        if (serv.AutomaticStartup) serv.Start();
                    }
                }
                Thread.Sleep(50);
            }
        }

        private void UpdateServices()
        {
            lock (innerLock)
            {
                if (DatabaseManager.Find(serviceLocation) == null)
                {
                    return;
                }

                foreach (Service value in DatabaseManager.Find(serviceLocation).ArrayOfType<Service>())
                {
                    if (value == null) continue;

                    if (!services.ContainsKey(value.ApplicationPath))
                    {
                        services[value.ApplicationPath] = value;
                        services[value.ApplicationPath].AppInstances = this.appInstances;
                    }
                }
            }
        }

        #endregion

        #region Properties

        [Required]
        public IComponentDirectory ComponentDirectory
        {
            get { return componentDirectory; }
            set { componentDirectory = value; }
        }

        
        [Required]
        public IApplicationInstances AppInstances
        {
            get { return appInstances; }
            set { appInstances = value; }
        }

        
        [Required]
        public DatabaseManager DatabaseManager
        {
            get { return databaseManager; }
            set { databaseManager = value; }
        }

        #endregion

        

        

        #region IServiceRegistry Members

        public Service[] StartedServices
        {
            get 
            {
                lock (innerLock)
                {
                    UpdateServices();
                    List<Service> result = new List<Service>();
                    foreach (Service service in services.Values)
                    {
                        if (service.Started) result.Add(service);
                    }

                    return result.ToArray();
                }
            }
        }

        public Service[] StoppedServices
        {
            get
            {
                lock (innerLock)
                {
                    UpdateServices();
                    List<Service> result = new List<Service>();
                    foreach (Service service in services.Values)
                    {
                        if (!service.Started) result.Add(service);
                    }

                    return result.ToArray();
                }
            }
        }

        public void RegisterServiceComponent(IComponentProvider provider)
        {
            if (provider.MatchAgainstNameAllowed)
            {
                /* FIXME: We will need to do proper per-process cleanup in the future */
                foreach (IComponentProvider prov in componentDirectory.FindByName(provider.MatchedName))
                {
                    componentDirectory.UnRegister(prov);
                }
            }

            componentDirectory.Register(provider);
        }

        #endregion

        public ServiceManager()
        {
            checkThread = new Thread(CheckMain);
        }

        #region IServiceControl Members

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool Stop()
        {
            if (checkThread.IsAlive)
            {
                runCheckThread = false;
                checkThread.Join();
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool Start()
        {
            if (!checkThread.IsAlive)
            {
                runCheckThread = true;
                checkThread.Start();
            }

            return true;
        }

        public bool Started
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get { return checkThread != null && checkThread.IsAlive; }
        }

        public bool AutomaticStartup
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            lock (innerLock)
            {
                Dispose(true);
            }
        }

        #endregion
    }
}
