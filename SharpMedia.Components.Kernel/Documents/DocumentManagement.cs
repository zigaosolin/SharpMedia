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
using SharpMedia.Components.Documents;
using SharpMedia.Database;
using System.Runtime.CompilerServices;
using SharpMedia.Components.Configuration;

namespace SharpMedia.Components.Kernel.Documents
{
    /// <summary>
    /// Information about a document type
    /// </summary>
    [Serializable]
    internal class DocumentTypeInfo
    {
        Dictionary<string, List<string>> verb2Application = new Dictionary<string, List<string>>();

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string[] AppsWithVerb(string verb)
        {
            if (verb2Application.ContainsKey(verb))
            {
                return verb2Application[verb].ToArray();
            }
            else
            {
                return new string[0];
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string[] AvailableVerbs(string appId)
        {
            List<string> verbs = new List<string>();

            foreach (KeyValuePair<String, List<string>> kvp in verb2Application)
            {
                foreach (string app in kvp.Value)
                {
                    if (app == appId) { verbs.Add(kvp.Key); break; }
                }
            }

            return verbs.ToArray();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal bool DeleteApplicationInformation(string appInfo)
        {
            foreach (KeyValuePair<String, List<string>> kvp in verb2Application)
            {
                if (kvp.Value.Contains(appInfo))
                {
                    kvp.Value.RemoveAll(delegate(string other) { return other == appInfo; });

                    return true;
                }
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void AddDocUsage(string app, string[] supportedVerbs)
        {
            foreach (string verb in supportedVerbs)
            {
                if (!verb2Application.ContainsKey(verb))
                {
                    verb2Application[verb] = new List<string>();
                }

                if (!verb2Application[verb].Contains(app))
                {
                    verb2Application[verb].Add(app);
                }
            }
        }
    }

    /// <summary>
    /// Implements document management functions
    /// </summary>
    /// <remarks>
    /// Storing pattern (symlinks)
    /// 
    ///   /System/Runtime/DocumentBindings/{TypeName} --> DocumentTypeInfo: SingleObject
    /// </remarks>
    internal class DocumentManagement : IDocumentManagement, IDocumentManagementRegistry
    {
        #region Internals
        static readonly string bindingsPath = "/System/Runtime/DocumentBindings";

        Dictionary<string, DocumentTypeInfo> type2info = new Dictionary<string, DocumentTypeInfo>();

        private DatabaseManager databaseManager;

        [Required]
        public DatabaseManager DatabaseManager
        {
            get { return databaseManager; }
            set { databaseManager = value; }
        }

        private void AssureInitialized()
        {
            if (type2info.Count == 0)
            {
                /** load **/
                foreach (Node<object> node in DatabaseManager.Find<Object>(bindingsPath).Children)
                {
                    if (!node.TypedStreamExists<DocumentTypeInfo>())
                    {
                        node.Parent.Delete(node.Name);
                    }
                    else
                    {
                        type2info[node.Name] = node.ObjectOfType<DocumentTypeInfo>();
                    }
                }
            }
        }

        #endregion

        #region IDocumentManagement Members

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string[] AvailableApplications(string type, string verb)
        {
            AssureInitialized();
            if (type2info.ContainsKey(type))
            {
                return type2info[type].AppsWithVerb(verb);
            }

            return new string[0];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string[] AvailableVerbs(string app, string type)
        {
            AssureInitialized();
            if (type2info.ContainsKey(type))
            {
                return type2info[type].AvailableVerbs(app);
            }

            return new string[0];
        }

        #endregion

        #region IDocumentManagementRegistry Members

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RegisterDocumentUsage(string documentType, string app, string[] supportedVerbs)
        {
            foreach (string verb in supportedVerbs)
            {
                Console.Out.WriteLine("Bind: {0}[{2}] -> {1}", documentType, app, verb);
            }
            AssureInitialized();
            if (!type2info.ContainsKey(documentType))
            {
                /** create new **/
                databaseManager.Find(bindingsPath).Create(
                    documentType, typeof(DocumentTypeInfo), StreamOptions.SingleObject);

                type2info[documentType] = new DocumentTypeInfo();
            }
            /** update and save **/
            type2info[documentType].AddDocUsage(app, supportedVerbs);

            Node<DocumentTypeInfo> node = 
                databaseManager.Find<DocumentTypeInfo>(bindingsPath + "/" + documentType);

            node.Object = type2info[documentType];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteApplicationInformation(string appInfo)
        {
            AssureInitialized();
            foreach (string type in type2info.Keys)
            {
                if (type2info[type].DeleteApplicationInformation(appInfo))
                {
                    Node<DocumentTypeInfo> node =
                        databaseManager.Find<DocumentTypeInfo>(bindingsPath + "/" + type);

                    node.Object = type2info[type];
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteTypeInformation(string type)
        {
            AssureInitialized();
            if (type2info.ContainsKey(type))
            {
                type2info.Remove(type);
                databaseManager.Find(bindingsPath).Delete(type);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SelectDefault(string documentType, string app, string verb)
        {
            AssureInitialized();
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
