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
using System.Reflection;

using SharpMedia.Components.Database;
using SharpMedia.Components.Configuration;
using System.IO;
using SharpMedia.Database;

namespace SharpMedia.Components.Kernel
{
    /// <summary>
    /// Kernel assembly loader
    /// </summary>
    internal class KernelAssemblyLoader : MarshalByRefObject, IAssemblyLoader
    {
        static string[] searchPaths = new string[] {
            "/System/Assemblies",
            "/System/Applications/Assemblies",
            "/Applications/Assemblies"
        };

        private DatabaseManager mgr;
        [Required]
        public DatabaseManager DatabaseManager
        {
            get { return mgr; }
            set { mgr = value; }
        }

        /// <summary>
        /// Assembly Cache
        /// </summary>
        static Dictionary<string, System.Reflection.Assembly> asmCache = new Dictionary<string, System.Reflection.Assembly>();

        #region IAssemblyLoader Members

        public System.Reflection.Assembly Load(string requiredName)
        {
            string debugInfo;
            return Load(requiredName, out debugInfo);
        }

        public System.Reflection.Assembly Load(string requiredName, out string debug)
        {
            debug = "";
            if (asmCache.Count == 0)
            {
                foreach (System.Reflection.Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    asmCache[asm.GetName().Name] = asm;
                    Console.Out.WriteLine("Caching Assembly: {0}", asm.GetName().Name);
                }
            }

            AssemblyName asmName = new AssemblyName(requiredName);
            if (asmCache.ContainsKey(asmName.Name))
            {
                return asmCache[asmName.Name];
            }
            else
            {
                foreach (string path in searchPaths)
                {
                    string xpath = path + "/" + asmName.Name.Replace('.','/');
                    debug += String.Format(" .. Assembly.Load, Probing Path '{0}'\n", xpath);
                    if (mgr.Find(xpath) != null)
                    {
                        Node<object> node = mgr.Find(xpath);



                        if (node != null && node.TypedStreamExists <SharpMedia.Components.Database.AssemblyDesc>())
                        {
                            using (TypedStream<SharpMedia.Components.Database.AssemblyDesc> ts =
                                    node.OpenForReading<SharpMedia.Components.Database.AssemblyDesc>())
                                {
                                Assembly asm = null;
                                if (false && !ts.Object.Unmanaged)
                                {
                                    asm = Assembly.Load(ts.Object.ByteContent);
                                }
                                else
                                {
                                    File.WriteAllBytes(asmName + ".dll", ts.Object.ByteContent);
                                    // asm = Assembly.LoadFrom(Path.GetFullPath(asmName + ".dll"));
                                    string fullPath = Path.GetFullPath(asmName + ".dll");
                                    asm = Assembly.LoadFile(fullPath);
                                }

                                asmCache[requiredName] = asm;
                                return asm;
                            }
                        }
                    }
                }
            }

            debug += String.Format("> Failed to get Assembly {0}\n", requiredName);
            return null;
        }

        #endregion

        internal KernelAssemblyLoader()
        {
        }
    }
}
