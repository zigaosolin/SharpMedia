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
using SharpMedia.AspectOriented.Framework;
using PostSharp.CodeModel;

namespace SharpMedia.AspectOriented.Processors
{
    /// <summary>
    /// Process type attributes
    /// </summary>
    public class ProcessTypeAttributes : Attribute, ITypeProcessor
    {
        public bool Match(string typeName)
        {
            return true;
        }

        public bool Match(PostSharp.CodeModel.IType type)
        {
            try
            {
                TypeDefDeclaration definition = type.GetTypeDefinition();
                if (definition == null) return false;
            }
            catch (Exception exc)
            {
                // bind error, or type does not have a definition
                // in either case, not a type we want to process
                return false;
            }

            return true;
        }

        public void Process(IType typeIn, ref IType typeOut)
        {

            foreach (CustomAttributeDeclaration decl in typeIn.CustomAttributes)
            {
                object attr = decl.ConstructRuntimeObject();

                if (attr is ClassAspectAttribute)
                {
                    ProcessClassAttribute(typeIn, ref typeOut, attr as ClassAspectAttribute);
                }
                else if(attr is AspectOrientedAttribute) 
                {
                    // FIXME: throw an exception or log...
                    // we probably won't graciously support someone putting [NotNull] on a class decl
                }

                foreach (PropertyDeclaration property in typeIn.GetTypeDefinition().Properties)
                {
                    foreach (CustomAttributeDeclaration propertyDecl in property.CustomAttributes)
                    {
                        object propertyAttr = propertyDecl.ConstructRuntimeObject();
                        if (propertyAttr is PropertyAspectAttribute)
                        {
                            ProcessPropertyAttribute(typeIn, ref typeOut, property, propertyAttr as PropertyAspectAttribute);
                        }
                    }
                }

                // all methods, possibly including property getters and setters
                foreach(IMethod method in typeIn.Methods) 
                {
                    // FIXME: skip parameter definitions, if they are found

                    foreach (CustomAttributeDeclaration methDecl in method.CustomAttributes)
                    {
                        object methAttr = methDecl.ConstructRuntimeObject();
                        if (methAttr is MethodAspectAttribute)
                        {
                            ProcessMethodAttribute(typeIn, ref typeOut, method, methAttr as MethodAspectAttribute);
                        }
                    }

                    foreach (ParameterDeclaration parameter in method.GetMethodDefinition().Parameters)
                    {
                        foreach (CustomAttributeDeclaration paramDecl in parameter.CustomAttributes)
                        {
                            object paramAttr = paramDecl.ConstructRuntimeObject();
                            if (paramAttr is ParameterAspectAttribute)
                            {
                                ProcessParameterAttribute(typeIn, ref typeOut, method, parameter, paramAttr as ParameterAspectAttribute);
                            }
                        }
                    }
                }
            }
        }

        // process a property aspect attribute
        private void ProcessPropertyAttribute(IType typeIn, ref IType typeOut, PropertyDeclaration property, PropertyAspectAttribute iPropertyAspectAttribute)
        {
            // FIXME: Not implemented
            throw new NotImplementedException();
        }

        // process a parameter aspect attribute
        private void ProcessParameterAttribute(IType typeIn, ref IType typeOut, IMethod method, ParameterDeclaration parameter, ParameterAspectAttribute iParameterAspectAttribute)
        {
            // FIXME: Not implemented
            throw new NotImplementedException();
        }

        // process a method aspect attribute
        private void ProcessMethodAttribute(IType typeIn, ref IType typeOut, IMethod method, MethodAspectAttribute iMethodAspectAttribute)
        {
            // FIXME: Not implemented
            throw new NotImplementedException();
        }

        // process a class aspect attribute
        private void ProcessClassAttribute(IType typeIn, ref IType typeOut, ClassAspectAttribute iClassAspectAttribute)
        {
            iClassAspectAttribute.Process(typeIn, ref typeOut);
        }
    }
}
