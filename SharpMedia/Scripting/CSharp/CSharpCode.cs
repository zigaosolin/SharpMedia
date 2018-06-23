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
using SharpMedia.AspectOriented;

namespace SharpMedia.Scripting.CSharp
{
    /// <summary>
    /// A C# code object. Includes textural code and compile attributes.
    /// </summary>
    /// <remarks>It is immutable.</remarks>
    [Serializable]
    public sealed class CSharpCode
    {
        #region Private Members
        string[] code;
        string entryPoint;
        string[] referencedAssemblies;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates CSharpCode object.
        /// </summary>
        public CSharpCode([NotEmpty] string code, [NotEmpty] string entryPoint, 
            [NotNull] params string[] referencedAssemblies)
            : this(new string[]{code}, entryPoint, referencedAssemblies)
        {
        }

        /// <summary>
        /// Creates CSharpCode object.
        /// </summary>
        public CSharpCode([NotEmpty] string[] code, [NotEmpty]string entryPoint,
            [NotNull] params string[] referencedAssemblies)
        {
            this.code = code;
            this.entryPoint = entryPoint;
            this.referencedAssemblies = referencedAssemblies;
        }


        #endregion

        #region Properties

        /// <summary>
        /// Entry point of script. It can be either class name or static method.
        /// </summary>
        public string EntryPoint
        {
            get
            {
                return entryPoint;
            }
        }

        /// <summary>
        /// Code to be compiler, can be multiple files.
        /// </summary>
        public string[] Code
        {
            get
            {
                return code;
            }
        }
        
        /// <summary>
        /// Referenced assemblies.
        /// </summary>
        public string[] ReferencedAssemblies
        {
            get
            {
                return referencedAssemblies;
            }
        }

        #endregion

    }
}
