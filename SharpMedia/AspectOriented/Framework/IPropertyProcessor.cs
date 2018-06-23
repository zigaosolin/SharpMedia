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
    public interface IPropertyProcessor
    {
        /// <summary>
        /// Returns true if the processor is to process a property (match by name)
        /// </summary>
        /// <param name="typeName">Type name</param>
        /// <param name="methodName">Property name</param>
        bool Match(string typeName, string propertyName);

        /// <summary>
        /// Returns true if the processor is to process a property (match by value)
        /// </summary>
        /// <param name="type">Method</param>
        bool Match(PropertyDeclaration method);

        /// <summary>
        /// Process the property
        /// </summary>
        /// <param name="propIn">Property to process</param>
        /// <param name="propOut">Processed property</param>
        /// <param name="typeIn">Class that contains the property</param>
        /// <param name="typeOut">Processed class</param>
        void Process(IType typeIn, ref IType typeOut, PropertyDeclaration propIn, ref PropertyDeclaration propOut);
    }
}
