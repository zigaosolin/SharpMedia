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
using System.IO;

namespace TemplateEngine
{

    /// <summary>
    /// The mapping.
    /// </summary>
    public sealed class Mapping
    {
        public string Source;
        public string Template;
        public string Destination;
    }

    /// <summary>
    /// A template project.
    /// </summary>
    public sealed class TemplateProject : ITemplateResolver
    {
        #region Private Members
        SortedList<string, ITemplateSet> templateSets = new SortedList<string, ITemplateSet>();
        SortedList<string, IDocumentNode> documentSources = new SortedList<string, IDocumentNode>();
        List<Mapping> generationMapper = new List<Mapping>();
        #endregion

        #region ITemplateResolver Members

        public ITemplateSet Resolve(string reference)
        {
            ITemplateSet set;
            if (templateSets.TryGetValue(reference, out set)) return set;
            return null;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="templates"></param>
        /// <param name="documents"></param>
        /// <param name="mapper"></param>
        public TemplateProject(SortedList<string, ITemplateSet> templates,
            SortedList<string, IDocumentNode> documents, List<Mapping> mapper)
        {
            this.templateSets = templates;
            this.documentSources = documents;
            this.generationMapper = mapper;
        }

        #endregion

        #region Static Members

        /// <summary>
        /// Parses project from document.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static TemplateProject Parse(XmlDocument doc)
        {
            XmlNode sources = doc["TemplateProject"]["Sources"];
            XmlNode templates = doc["TemplateProject"]["Templates"];
            XmlNode mappings = doc["TemplateProject"]["Mappings"];
            

            SortedList<string, ITemplateSet> xtemplates = new SortedList<string, ITemplateSet>();
            SortedList<string, IDocumentNode> xsources = new SortedList<string, IDocumentNode>();
            List<Mapping> xmappings = new List<Mapping>();

            foreach (XmlNode source in sources.ChildNodes)
            {
                if (source.NodeType == XmlNodeType.Comment) continue;
                if (source.Name != "Source") continue;

                string name = source.Attributes["Name"].Value;
                string path = source.Attributes["Path"].Value;

                SourceDocument document = SourceDocument.Parse(path);

                xsources.Add(name, document);
            }

            foreach (XmlNode template in templates.ChildNodes)
            {
                if (template.NodeType == XmlNodeType.Comment
                    || template.Name != "Template") continue;

                string path = template.Attributes["Path"].Value;

                List<ITemplateSet> sets = TemplateSet.Load(path);
                foreach (ITemplateSet set in sets)
                {
                    xtemplates.Add(set.Name, set);
                }
            }

            foreach (XmlNode mapping in mappings)
            {
                if (mapping.NodeType == XmlNodeType.Comment || mapping.Name != "Mapping") continue;

                Mapping m = new Mapping();

                m.Source = mapping.Attributes["Source"].Value;
                m.Template = mapping.Attributes["Template"].Value;
                m.Destination = mapping.Attributes["OutputPath"].Value;

                xmappings.Add(m);
            }

            return new TemplateProject(xtemplates, xsources, xmappings);
        }

        /// <summary>
        /// Parses project from file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static TemplateProject Parse(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            return Parse(doc);
        }

        #endregion

        #region Members

        public void Process()
        {
            foreach (ITemplateSet set in this.templateSets.Values)
            {
                set.Resolve(this);
            }

            // Later on, we will implement rewriting only if changed
            foreach (Mapping mapping in this.generationMapper)
            {
                Console.WriteLine("Processing mapping '{0}:{1}' -> '{2}'",
                    mapping.Source, mapping.Template, mapping.Destination);

                ITemplateSet set = templateSets[mapping.Template];
                IDocumentNode src = documentSources[mapping.Source];

                using (Stream stream = File.Create(mapping.Destination))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.WriteLine("// This file was generated by TemplateEngine from template source '{0}'", mapping.Source);
                        writer.WriteLine("// using template '{0}. Do not modify this file directly, modify it from template source.", mapping.Template);
                        writer.WriteLine();

                        src.Emit(set, writer);
                    }
                }
                
            }
        }

        #endregion
    }
}
