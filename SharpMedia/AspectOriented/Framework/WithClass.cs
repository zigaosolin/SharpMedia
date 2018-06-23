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
    /// Common tasks to perform with modules
    /// </summary>
    public static class WithClass
    {
        /// <summary>
        /// Copy class contents from one to another
        /// </summary>
        public static void Copy([NotNull] TypeDefDeclaration clsIn, ref TypeDefDeclaration clsOut)
        {
            byte[] classData = Common.SerializeToArray(clsIn);
            clsOut = (TypeDefDeclaration) Common.DeserializeFromArray(classData);
        }

        /// <summary>
        /// Remove a method from a class
        /// </summary>
        public static void RemoveMethod([NotNull] ref TypeDefDeclaration decl, [NotNull] MethodDefDeclaration method)
        {
            decl.Methods.Remove(method);
        }

        /// <summary>
        /// Remove a property from a class
        /// </summary>
        public static void RemoveProperty([NotNull] ref TypeDefDeclaration decl, [NotNull] PropertyDeclaration property)
        {
            decl.Properties.Remove(property);
        }

        /// <summary>
        /// Finds a method with the same signature in the class
        /// </summary>
        /// <param name="decl">Class to inspect</param>
        /// <param name="method">Method with a name and signature</param>
        /// <returns>Found method or null</returns>
        public static MethodDefDeclaration FindSameMethod([NotNull] TypeDefDeclaration decl, [NotNull] MethodDefDeclaration method)
        {
            IMethod found =
                decl.Methods.GetMethod(
                    method.Name,
                    method,
                    BindingOptions.OnlyExisting);

            if (found == null) return null;

            return found.GetMethodDefinition(BindingOptions.OnlyExisting);
        }

        /// <summary>
        /// Adds a method to a class
        /// </summary>
        /// <param name="decl">Class to add the method to</param>
        /// <param name="method">Method to add</param>
        public static void AddMethod([NotNull] TypeDefDeclaration decl, [NotNull] MethodDefDeclaration method)
        {
            MethodDefDeclaration previous = FindSameMethod(decl, method);
            if (previous != null)
            {
                previous.Remove();
            }

            decl.Methods.Add(method);
        }
    }
}
