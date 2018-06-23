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
    /// <summary>
    /// Interface that signifies attributes that perform aspect oriented
    /// </summary>
    public abstract class AspectOrientedAttribute : Attribute
    {
    }

    /// <summary>
    /// Class aspect attributes
    /// </summary>
    public abstract class ClassAspectAttribute : AspectOrientedAttribute, ITypeProcessor
    {
        #region ITypeProcessor Members

        public virtual bool Match(string typeName)
        {
            return true;
        }

        public virtual bool Match(IType type)
        {
            return true;
        }

        public abstract void Process(IType typeIn, ref IType typeOut);

        #endregion
    }

    /// <summary>
    /// Assembly aspect attributes
    /// </summary>
    public abstract class IAssemblyAspectAttribute : AspectOrientedAttribute, IAssemblyProcessor
    {
        #region IAssemblyProcessor Members

        public virtual bool Match(System.Reflection.AssemblyName asmName)
        {
            return true;
        }

        public virtual bool Match(IAssembly assembly)
        {
            return true;
        }

        public abstract bool Process(IAssembly assemblyIn, ref IAssembly assemblyOut);

        #endregion
    }

    // FIXME: The interfaces below need secondary interfaces

    /// <summary>
    /// Method aspect attributes
    /// </summary>
    public abstract class MethodAspectAttribute : AspectOrientedAttribute, IMethodProcessor
    {
        #region IMethodProcessor Members

        public virtual bool Match(string typeName, string methodName)
        {
            return true;
        }

        public virtual bool Match(IMethod method)
        {
            return true;
        }

        public abstract void Process(IMethod typeIn, ref IMethod typeOut);

        #endregion
    }

    /// <summary>
    /// Property aspect attributes
    /// </summary>
    public abstract class PropertyAspectAttribute : AspectOrientedAttribute, IPropertyProcessor
    {
        #region IPropertyProcessor Members

        public virtual bool Match(string typeName, string propertyName)
        {
            return true;
        }

        public virtual bool Match(PropertyDeclaration method)
        {
            return true;
        }

        public abstract void Process(IType typeIn, ref IType typeOut, PropertyDeclaration propIn, ref PropertyDeclaration propOut);

        #endregion
    }

    /// <summary>
    /// Parameter aspect attributes
    /// </summary>
    /// <remarks>All interfaces in this file are to be moved to ABCs : Attribute with abstract Process()</remarks>
    public abstract class ParameterAspectAttribute : AspectOrientedAttribute, IMethodProcessor
    {
        #region IMethodProcessor Members

        public virtual bool Match(string typeName, string methodName)
        {
            return true;
        }

        public virtual bool Match(PostSharp.CodeModel.IMethod method)
        {
            return true;
        }

        public abstract void Process(IMethod typeIn, ref IMethod typeOut);

        #endregion

        /// <summary>
        /// Parameter that is being processed
        /// </summary>
        public ParameterDeclaration Parameter { get;  set; }

    }

    /// <summary>
    /// Generic parameter aspect attributes
    /// </summary>
    public abstract class IGenericParameterAspectAttribute : AspectOrientedAttribute
    {
    }
}
