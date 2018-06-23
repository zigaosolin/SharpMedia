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
using System.Reflection;
using System.IO;

namespace SharpMedia.Components.Database
{
    /// <summary>
    /// An Assembly
    /// </summary>
    [Serializable]
    public class AssemblyDesc
    {
        private byte[] byteContent;
        /// <summary>
        /// Byte content
        /// </summary>
        public byte[] ByteContent
        {
            get { return byteContent; }
            set { byteContent = value; }
        }

        private bool unamanaged;
        /// <summary>
        /// True if the assembly contains unmanaged code, and should be
        /// shadow-loaded
        /// </summary>
        public bool Unmanaged
        {
            get { return unamanaged; }
            set { unamanaged = value; }
        }

        private AssemblyName assemblyName;
        /// <summary>
        /// Name of the Assembly
        /// </summary>
        public AssemblyName AssemblyName
        {
            get { return assemblyName; }
            set { assemblyName = value; }
        }

        public AssemblyDesc(System.Reflection.Assembly asm)
        {
            ByteContent = File.ReadAllBytes(asm.Location);
            AssemblyName = asm.GetName();
        }

        public AssemblyDesc(byte[] bytes) { byteContent = bytes.Clone() as byte[]; }
    }
}
