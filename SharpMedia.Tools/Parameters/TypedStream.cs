using System;
using System.Collections.Generic;
using System.Text;
using SharpMedia.Database;
using SharpMedia.Components.Configuration;
using SharpMedia.Components.Configuration.ComponentProviders;

namespace SharpMedia.Tools.Parameters
{

    /// <summary>
    /// A typed stream parameter.
    /// </summary>
    public class TypedStream : IToolParameter
    {
        #region Private Members
        TypedStreamUIAttribute attribute;
        Type parseType;
        string name;
        DatabaseManager manager;

        string path;
        string typeName;
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="parseType"></param>
        /// <param name="name"></param>
        /// <param name="manager"></param>
        public TypedStream(TypedStreamUIAttribute attribute, Type parseType, string name, DatabaseManager manager)
        {
            this.attribute = attribute;
            this.parseType = parseType;
            this.name = name;
            this.manager = manager;
        }

        #endregion

        #region Helper Members

        public override string ToString()
        {
            if (path != null)
            {
                return name + " = " + path + "@" + typeName;
            }
            return string.Empty;
        }

        public bool Match(string sourceString, out string path, out string typeName)
        {
            path = string.Empty;
            typeName = string.Empty;

            // We extract path and typed stream.
            string[] data = sourceString.Split('@');
            if(data.Length == 0) 
            {
                return false;
            }

            path = data[0];
            Type type;
            Node<object> node = manager.Find(path);
            if(node == null)
            {
                return false;
            }

            if (data.Length == 1)
            {
                
                type = node.DefaultType;
            }
            else if (data.Length == 2)
            {
                type = Type.GetType(data[1]);
            }
            else
            {
                return false;
            }


            // We check type.
            if (type == null)
            {
                return false;
            }
            if (attribute.Type != null && !Common.IsTypeSameOrDerived(attribute.Type, type))
            {
                return false;
            }

            typeName = type.FullName;

            // We now validate.
            using (TypedStream<object> typedStream = node.Open(type, OpenMode.Read))
            {
                // Check object count.
                uint count = typedStream.Count;
                if (count < attribute.MinObjectCount || count > attribute.MaxObjectCount)
                {
                    return false;
                }

                // Are derived disallowed.
                if (attribute.DissallowDerivedTypes &&
                    (typedStream.Flags & StreamOptions.AllowDerivedTypes) != 0)
                {
                    return false;
                }
            }

            return true;
        }


        public void MatchThrow(string sourceString, out string path, out string typeName)
        {
            // FIXME: use proper exceptions.
            if (!Match(sourceString, out path, out typeName)) throw new Exception("Invalid path");

        }

        #endregion

        #region IToolParameter Members

        public string Name
        {
            get { return name; }
        }

        public UIAttribute Attribute
        {
            get { return attribute; }
        }

        public void Parse(string sourceString)
        {
            string dum1, dum2;

            // We perform a match.
            MatchThrow(sourceString, out dum1, out dum2);

            path = dum1;
            typeName = dum2;
        }

        public void Apply(IComponentDirectory toolDirectory)
        {
            string dum1, dum2;
            MatchThrow(path, out dum1, out dum2);

            Node<object> node = manager.Find(path);
            TypedStream<object> obj = node.Open(Type.GetType(typeName), attribute.OpenMode);

            toolDirectory.Register(new Instance(obj, name));
        }

        public string[] PossibleValuesHint
        {
            get { return new string[0]; }
        }

        public bool AcceptsOnlyHintedValues
        {
            get { return false; }
        }

        public bool IsSet
        {
            get { return path != null; }
        }


        #endregion
    }
}
