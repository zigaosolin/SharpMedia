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
using SharpMedia.AspectOriented;

namespace SharpMedia.Scripting.CSharp
{
    /// <summary>
    /// A C# script, actually a compiled assembly.
    /// </summary>
    [Serializable, Scene.SceneComponent(QueryType=typeof(IScript))]
    public sealed class CSharpScript : IScript
    {
        #region Private Members
        Assembly assembly;
        string entryPoint;

        Type type;
        MethodInfo methodInfo;

        internal CSharpScript([NotNull] Assembly assembly, string entryPoint)
        {
            this.assembly = assembly;
            this.entryPoint = entryPoint;

            // We extract method/type.
            type = assembly.GetType(entryPoint, false);
            if (type == null)
            {
                // We now resolve the method's type.
                type = assembly.GetType(entryPoint.Substring(0, entryPoint.LastIndexOf('.')), true);
                methodInfo = type.GetMethod(entryPoint.Substring(entryPoint.LastIndexOf('.') + 1));
            }
        }
        #endregion

        #region IScript Members

        public ScriptType ScriptType
        {
            get 
            {
                return methodInfo != null ? ScriptType.Method : ScriptType.Class;
            }
        }

        public Type ObjectType
        {
            get { return type; }
        }

        public MethodInfo MethodInfo
        {
            get { return methodInfo; }
        }

        public Assembly AssemblyInfo
        {
            get { return assembly; }
        }

        #endregion

    }
}
