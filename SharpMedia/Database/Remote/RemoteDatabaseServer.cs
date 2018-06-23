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
using System.Runtime.Remoting;
using SharpMedia.Database.Managed;
using System.Runtime.Remoting.Lifetime;
using SharpMedia.Testing;
using System.Runtime.Remoting.Channels;
using System.Threading;

namespace SharpMedia.Database.Remote
{
    /// <summary>
    /// A remote database server 
    /// </summary>
    public sealed class RemoteDatabaseServer : IDisposable
    {
        #region Private Members
        RemoteDatabase database;
        #endregion

        #region Public Members

        /// <summary>
        /// Creates a remote database server
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="root"></param>
        public RemoteDatabaseServer(string uri, Node<object> root)
        {
            if (root.AsINode().DriverAspect == null)
            {
                throw new InvalidOperationException(string.Format("The node {0} cannot be remoted, you probably use "
                    + "layers that are not compatible with remoting.", root.Path));
            }

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(RemoteDatabase), uri, WellKnownObjectMode.Singleton);

            database = (RemoteDatabase)Activator.GetObject(typeof(RemoteDatabase), uri);
            database.Root = root.AsINode().DriverAspect;
           
            
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // Force garbage collection somehow ...
            database = null;
        }

        #endregion
    }


#if SHARPMEDIA_TESTSUITE

    [TestSuite]
    internal class RemoteDatabaseTest
    {
        Node<object> root = null;

        public void Server1()
        {
            // TODO: register channels.
            RemoteDatabaseServer server = 
                new RemoteDatabaseServer(@"tcp://localhost:8085/RemoteDatabase", root);
            

            // We keep server opened, 
            Thread.Sleep(100000);
        }

        public void Client1()
        {
            DatabaseManager manager = new DatabaseManager();

            manager.Mount("/Remote/Computer", RemoteDatabase.Locate(@"tcp://localhost:8085/RemoteDatabase"));

            // We can use manager.
        }



    }

#endif
}
