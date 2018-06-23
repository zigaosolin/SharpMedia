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
using PostSharp.CodeModel;

namespace SharpMedia.AspectOriented.Framework
{
    public interface IMethodProcessor
    {
        /// <summary>
        /// Returns true if the processor is to process a module (match by name)
        /// </summary>
        /// <param name="typeName">Type name</param>
        /// <param name="methodName">Method name</param>
        bool Match(string typeName, string methodName);

        /// <summary>
        /// Returns true if the processor is to process a method (match by value)
        /// </summary>
        /// <param name="type">Method</param>
        bool Match(IMethod method);

        /// <summary>
        /// Process the method
        /// </summary>
        void Process(IMethod methodIn, ref IMethod methodOut);
    }
}
