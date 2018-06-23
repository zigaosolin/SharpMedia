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
using System.IO;
using SharpMedia.AspectOriented;

namespace SharpMedia.Scripting
{
    /// <summary>
    /// Script engine allows script import.
    /// </summary>
    public static class ScriptEngine
    {
        #region Private Members
        static object syncRoot = new object();
        static SortedDictionary<Type, IScriptCompiler> type2compiler = new SortedDictionary<Type,IScriptCompiler>();
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the script compiler.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="compiler">The compiler.</param>
        public static void AddScriptCompiler([NotNull] IScriptCompiler compiler)
        {
            if (type2compiler.ContainsKey(compiler.ScriptCodeType))
            {
                throw new ArgumentException("The type " + compiler.ScriptCodeType + " already registered.");
            }
            type2compiler[compiler.ScriptCodeType] = compiler;
        }

        /// <summary>
        /// Removes the script compiler.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static bool RemoveScriptCompiler([NotNull] IScriptCompiler compiler)
        {
            return  type2compiler.Remove(compiler.ScriptCodeType);
        }

        /// <summary>
        /// Imports script from stream.
        /// </summary>
        /// <param name="data">The data of script.</param>
        /// <returns>The script.</returns>
        public static IScript Import([NotNull] object data)
        {
            // We make sure we extract the data.
            IScriptCompiler compiler;
            lock (syncRoot)
            {
                compiler = type2compiler[data.GetType()];
            }

            // We compile with compiler.
            return compiler.Compile(data);
        }

        /// <summary>
        /// A helper - imports script and runs its default method or creates the
        /// object defined by script.
        /// </summary>
        /// <param name="data">The data of script.</param>
        /// <param name="parameters">Method or objects parameters.</param>
        /// <returns>Newly created object or return value of default script.</returns>
        public static object ImportAndRun([NotNull] object data, params object[] parameters)
        {
            IScript script = Import(data);

            switch (script.ScriptType)
            {
                case ScriptType.Method:
                    return script.MethodInfo.Invoke(null, parameters);
                case ScriptType.Class:
                    return Activator.CreateInstance(script.ObjectType, parameters);
                default:
                    throw new NotSupportedException();
            }
        }

        #endregion

    }
}
