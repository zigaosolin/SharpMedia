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

namespace SharpMedia.Database.Ldap
{
    /// <summary>
    /// A Ldap tuple is entry in Ldap database that does not have a special meaning.
    /// </summary>
    [Serializable]
    public class LdapTuple
    {
        #region Private Members
        string propertyName;
        uint index;
        object value;
        #endregion

        #region Public Members

        /// <summary>
        /// The property name.
        /// </summary>
        public string PropertyName
        {
            get
            {
                return propertyName;
            }
            set
            {
                propertyName = value;
            }
        }

        /// <summary>
        /// The value of tuple.s
        /// </summary>
        public object Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// Index of tuple.
        /// </summary>
        /// <remarks>Setting index in array has no effect when writing, it is always appended.</remarks>
        public uint Index
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
            }
        }

        /// <summary>
        /// Default tuple.
        /// </summary>
        public LdapTuple()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public LdapTuple(string propertyName, uint index, object value)
        {
            this.propertyName = propertyName;
            this.index = index;
            this.value = value;
        }


        #endregion


    }
}
