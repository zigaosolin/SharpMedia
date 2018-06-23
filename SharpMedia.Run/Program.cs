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
using SharpMedia.Database.Memory;
using SharpMedia.Database;
using System.IO;
using SharpMedia.Components.Kernel;
using System.Threading;
using SharpMedia.Components.Database;
using SharpMedia.Database.Host;
using SharpMedia.Loading;

namespace SharpMedia.Run
{

    class Program
    {
        const string persistedDbPath = "root.fs";
        static void Main(string[] args)
        {
            
            IDatabase db = null;

            if (false && File.Exists(persistedDbPath))
            {
                db = MemoryDatabase.OpenDatabase(persistedDbPath);
            }
            else
            {
                db = MemoryDatabase.CreateNewDatabase("root");
            }

            DatabaseManager manager = new DatabaseManager();
            manager.Mount("/", db);
            manager.Mount("/Volumes/Host", new HostDatabase(".",
                new KeyValuePair<string, ILoadableFactory>("appxml", new XmlLoadableFactory()),
                new KeyValuePair<string, ILoadableFactory>("libxml", new XmlLoadableFactory()),
                new KeyValuePair<string, ILoadableFactory>("xml", new XmlLoadableFactory()),
                new KeyValuePair<string, ILoadableFactory>("ipkg", new XmlLoadableFactory())
            ));
            KernelStartup.Execute(manager, args);

            // persist the database
            MemoryDatabase.PersistToStream(persistedDbPath, db as MemoryDatabase);

            Environment.Exit(0);
        }
    }
}
