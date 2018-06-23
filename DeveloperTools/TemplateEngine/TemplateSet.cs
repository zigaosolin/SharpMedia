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
using System.Xml;

namespace TemplateEngine
{

    /// <summary>
    /// A template set.
    /// </summary>
    public class TemplateSet : ITemplateSet
    {
        #region Private Members
        string name;
        string[] inheritedNotResolved;
        ITemplateSet[] inherited = null;
        SortedList<string, object> templates;
        #endregion

        #region Public Members

        public TemplateSet(string name, string[] inherits, SortedList<string, object> templates)
        {
            this.name = name;
            this.inheritedNotResolved = inherits;
            this.templates = templates;
        }

        #endregion

        #region Private Methods

        static void ImportTemplate(XmlNode node, SortedList<string, object> templates)
        {
            string name = node.Attributes["Name"].Value;
            string value = node.Attributes["Value"].Value;

            templates.Add(name, value);
        }


        static ITemplateSet ImportSet(XmlNode node)
        {
            string name = string.Empty;
            SortedList<string, object> templates = new SortedList<string, object>();
            string[] extends = new string[0];

            // attributes
            foreach (XmlAttribute attribute in node.Attributes)
            {
                switch (attribute.Name)
                {
                    case "Name":
                        name = attribute.Value;
                        break;
                    case "Extends":
                        extends = attribute.Value.Split(',');
                        for (int i = 0; i < extends.Length; i++) extends[i] = extends[i].Trim();
                        break;
                    default:
                        break;
                }
            }

            // values
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "Template")
                {
                    ImportTemplate(child, templates);
                }
                else if (child.Name == "TemplateSet")
                {
                    templates.Add(child.Attributes["Name"].Value, ImportSet(child));
                }
                else if (child.Name == "TemplateLink")
                {
                    templates.Add(child.Attributes["Name"].Value,
                        new LinkSet(child.Attributes["Name"].Value, child.Attributes["Link"].Value));
                }
            }

            return new TemplateSet(name, extends, templates);
        }

        #endregion

        #region Static Members

        /// <summary>
        /// Loads all template sets from document.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static List<ITemplateSet> Load(XmlDocument document)
        {
            List<ITemplateSet> set = new List<ITemplateSet>();
            foreach (XmlNode node in document["TemplateSets"].ChildNodes)
            {
                if (node.Name != "TemplateSet")
                {
                    continue;
                }

                set.Add(ImportSet(node));
            }

            return set;
        }

        /// <summary>
        /// Loads the set from XML file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static List<ITemplateSet> Load(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            return Load(doc);
        }

        #endregion

        #region ITemplate Members

        public string Name
        {
            get { return name; }
        }

        public void Resolve(ITemplateResolver resolver)
        {
            inherited = new ITemplateSet[inheritedNotResolved.Length];
            for (int i = 0; i < inheritedNotResolved.Length; i++)
            {
                inherited[i] = resolver.Resolve(inheritedNotResolved[i]);
            }

            foreach (object obj in templates.Values)
            {
                if (obj is ITemplateSet)
                {
                    (obj as ITemplateSet).Resolve(resolver);
                }
            }
        }

        public object Provide(string name)
        {
            if (inherited == null)
            {
                throw new InvalidOperationException("Must resolve before calling this method.");
            }

            object obj;
            if (templates.TryGetValue(name, out obj)) return obj;

            for (int i = 0; i < inherited.Length; i++)
            {
                obj = inherited[i].Provide(name);
                if (obj != null)
                {
                    return obj;
                }
            }

            return null;
        }

        public string[] Available
        {
            get 
            { 
                List<string> s = new List<string>();
                foreach (string t in templates.Keys)
                {
                    s.Add(t);
                }
                return s.ToArray();
            }
        }

        #endregion

    }
}
