using System;
using System.Collections.Generic;
using System.Text;
using SharpMedia.Database;
using SharpMedia.Components.Configuration;

namespace SharpMedia.Tools.Parameters
{

    /// <summary>
    /// A node parameter.
    /// </summary>
    public class Node : IToolParameter
    {
        #region Private Members
        NodeUIAttribute attribute;
        Type parseType;
        string name;
        DatabaseManager manager;

        string path;
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="parseType"></param>
        /// <param name="name"></param>
        /// <param name="manager"></param>
        public Node(NodeUIAttribute attribute, Type parseType, string name, DatabaseManager manager)
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
                return name + " = " + path;
            }
            else
            {
                return string.Empty;
            }
        }

        public bool Match(string sourceString)
        {
            Node<object> obj = manager.Find(sourceString);
            if (obj == null)
            {
                return false;
            }

            Type nodeGeneric = attribute.DefaultType;

            if (nodeGeneric != null && !obj.Is(nodeGeneric))
            {
                return false;
            }

            foreach (Type neededType in attribute.RequiredTypes)
            {
                if (!obj.TypedStreamExists(neededType))
                {
                    return false;
                }
            }
            return true;
        }


        public void MatchThrow(string sourceString)
        {
            Node<object> obj = manager.Find(sourceString);
            if (obj == null)
            {
                throw new Exception(string.Format("The path '{0}' is non existant", sourceString));
            }

            Type nodeGeneric = attribute.DefaultType;

            if (nodeGeneric != null && !obj.Is(nodeGeneric))
            {
                throw new Exception(string.Format("The path '{0}' does not match type constraint; " +
                    "it is not castable to '{1}'", sourceString, nodeGeneric.ToString()));
            }

            foreach (Type neededType in attribute.RequiredTypes)
            {
                if (!obj.TypedStreamExists(neededType))
                {
                    throw new Exception(string.Format("The path '{0}' does not match type constraint; " +
                    "type stream '{1}' does not exist", sourceString, neededType.ToString()));
                }
            }

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
            // We perform a match.
            MatchThrow(sourceString);

            path = sourceString;
        }

        public void Apply(IComponentDirectory toolDirectory)
        {
            MatchThrow(path);

            
        }

        public string[] PossibleValuesHint
        {
            get { throw new NotImplementedException(); }
        }

        public bool AcceptsOnlyHintedValues
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsSet
        {
            get { return path != null; }
        }


        #endregion
    }
}
