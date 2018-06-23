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

namespace SharpMedia.ComponentOS.Configuration
{
    /// <summary>
    /// Represents required configuration from a type
    /// </summary>
    [Serializable]
    public class TypeRequiredConfiguration : IRequiresConfiguration
    {
        #region Private
        string typeName;
        #endregion

        #region IRequiresConfiguration Members

        public IRequiresConfiguration[] PreInitializers
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IRequirement[] Requirements
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public object Instance
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IRequiresConfiguration Clone()
        {
            return new TypeRequiredConfiguration(typeName);
        }

        #endregion

        public TypeRequiredConfiguration(string typeName)
        {
            this.typeName = typeName;
        }
    }
}
