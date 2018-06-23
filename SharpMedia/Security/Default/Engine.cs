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

namespace SharpMedia.Security.Default
{
    public class Engine : MarshalByRefObject, ISecurityEngine
    {
        #region ISecurityEngine Members

        public bool Can(object who, Type what, object toWhom)
        {
            return true;
        }

        public void Grant(object by, object who, Type what, object toWhom)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Revoke(object by, object from, Type what, object toWhom)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
