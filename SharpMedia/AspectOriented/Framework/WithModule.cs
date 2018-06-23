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
    public static class WithModule
    {
        /// <summary>
        /// Copies the module contents from one to the other
        /// </summary>
        /// <param name="from">Source module</param>
        /// <param name="to">Destination module</param>
        public static void Copy(ModuleDeclaration from, ref ModuleDeclaration to)
        {
            byte[] moduleData = Common.SerializeToArray(from);
            to = (ModuleDeclaration)Common.DeserializeFromArray(moduleData);
        }
    }
}
