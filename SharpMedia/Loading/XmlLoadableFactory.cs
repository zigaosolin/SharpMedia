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
using SharpMedia.Components.Installation;

namespace SharpMedia.Loading
{
    /// <summary>
    /// An Xml loading factory.
    /// </summary>
    public sealed class XmlLoadableFactory : ILoadableFactory
    {

        public XmlLoadableFactory()
        {
        }

        #region ILoadableFactory Members

        public object Load(System.IO.Stream s)
        {
            SerializableXmlDocument doc = new SerializableXmlDocument();
            try
            {
                doc.Load(s);
                return doc;
            }
            catch (Exception e)
            {
                Common.WarningFormatted(typeof(XmlLoadableFactory), 
                    "Could not load Xml document from stream '{0}': {1}", s, e);
                return null;
            }
        }

        public void Save(object value, System.IO.Stream s)
        {
            SerializableXmlDocument doc;
            if (value is SerializableXmlDocument)
            {
                doc = value as SerializableXmlDocument;
            }
            else
            {
                doc = new SerializableXmlDocument();
                doc.LoadXml((value as XmlDocument).InnerXml);

                // FIXME: very inefficient.
                Common.Warning(typeof(XmlLoadableFactory), "Unefficient saving of object, " +
                    "try using SerializableXmlDocument instead of XmlDocument.");
            }
            try
            {
                doc.Save(s);
            }
            catch (Exception e)
            {
                Common.WarningFormatted(typeof(XmlLoadableFactory),
                    "Could not save Xml document to stream '{0}': {1}", s, e);
                throw;
            }
            
        }

        public ulong? Size(object value)
        {
            FakeStream stream = new FakeStream();
            Save(value, stream);

            return (ulong?) stream.Length;
        }

        public bool CanLoadMore(System.IO.Stream s)
        {
            return s.Length != s.Position;
        }

        public Type LoadableType
        {
            get { return typeof(XmlDocument); }
        }

        #endregion
    }
}
