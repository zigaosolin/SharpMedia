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
using SharpMedia.Scripting.CompilerFacilities;
using SharpMedia.Components.Applications;
using System.Reflection;
using SharpMedia.Database;
using System.Threading;

namespace SharpMedia.Components.BaseSystem.TextShell.ExecutionTree
{

    /// <summary>
    /// A method call command.
    /// </summary>
    [Serializable]
    internal sealed class MethodCallCommand : IExecutionCommand
    {
        #region Private Members
        IExecutionCommand methodName;
        IExecutionCommand[] parameters;
        #endregion

        #region Public Members

        /// <summary>
        /// Method call parameters.
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        public MethodCallCommand(IExecutionCommand methodName, IExecutionCommand[] parameters)
        {
            this.methodName = methodName;
            this.parameters = parameters;
        }

        #endregion

        #region IExecutionCommand Members

        public int Exec(ExecutionContext context, out object[] inlineOutputDump)
        {
            // We set at beginning to invalid.
            inlineOutputDump = CommonShellCommands.EmptyParameters;

            // 1) We first resolve the method's name.
            object[] outDummy;
            if (methodName.Exec(context.ShellApp.CreateSubContext(context, methodName), out outDummy) < 0)
            {
                return -1;
            }

            if (outDummy.Length != 1)
            {
                context.Error.WriteLine("The method name constist of too many inline returns.");
                return -1;
            }

            // We have the method's name.
            string resolvedMethodName = outDummy[0].ToString();

            // 2) We resolve all parameters.
            List<object> resolvedParameters = new List<object>();

            for (int i = 0; i < parameters.Length; i++)
            {
                // We execute each.
                if (parameters[i].Exec(context.ShellApp.CreateSubContext(context, parameters[i]), out outDummy) < 0)
                {
                    return -1;
                }

                resolvedParameters.AddRange(outDummy);
            }

            // 3a) handle aliases.
            string methodNameAlias;
            if (context.ShellApp.Aliases.TryGetValue(resolvedMethodName, out methodNameAlias))
            {
                resolvedMethodName = methodNameAlias;
            }


            // 3b) We now execute the method. We search in internal commands.
            for (int i = 0; i < context.ShellApp.ShellCommandTypes.Length; i++)
            {
                Type commands = context.ShellApp.ShellCommandTypes[i];

                foreach (MethodInfo info in commands.GetMethods(BindingFlags.Static | BindingFlags.Public))
                {
                    object[] attrs = info.GetCustomAttributes(typeof(ShellCommandAttribute), false);
                    if (attrs.Length == 0) continue;

                    // We now match attributes.
                    ShellCommandAttribute attr = attrs[0] as ShellCommandAttribute;

                    if (string.Compare(resolvedMethodName, info.Name, !attr.CaseSensitive) != 0) continue;

                    if (context.IsParallel && !attr.CanParallel)
                    {
                        context.Error.WriteLine("Command {0} failed: cannot be used parallel.", resolvedMethodName);
                        return -1;
                    }

                    // We have a method, we call it.
                    try
                    {
                        object[] inputParams = new object[] { context, resolvedParameters.ToArray(), null };
                        int returnVal = (int)info.Invoke(null, inputParams);

                        // We link to out parameter.
                        inlineOutputDump = (object[])inputParams[2];

                        return returnVal;
                    }
                    catch (Exception ex)
                    {
                        context.Error.WriteLine("Command {0} failed:\n {1}", resolvedMethodName, 
                            context.ShellApp.ReportLevel == ReportLevel.NoTrace ? ex.Message : ex.ToString());
                    }
                    return 1;
                }
            }

            // 3c) We search applications.
            try
            {
                string[] args = Array.ConvertAll(resolvedParameters.ToArray(), 
                    new Converter<object, string>(delegate(object someObj) { return someObj.ToString(); }));

                // We find application.
                Node<object> appNode = context.ShellApp.WorkingDirectory.Find(resolvedMethodName);
                if (appNode == null)
                {
                    // We try common paths.
                    for (int i = 0; i < context.ShellApp.ExecutableSearchPaths.Length; i++)
                    {
                        appNode = context.ShellApp.Database.Find(
                            context.ShellApp.ExecutableSearchPaths[i] + resolvedMethodName);
                        if (appNode != null) break;
                    }

                    if (appNode == null)
                    {
                        context.Error.WriteLine("Command '{0}' not understood", resolvedMethodName);
                        return -1;
                    }
                }

                // We run instance and wait for result.
                IApplicationInstance instance = context.ShellApp.ApplicationInstances.Run(appNode.Path, string.Empty, args);

                // FIXME: use 'lock for'
                if (!context.IsParallel)
                {
                    while (instance.IsRunning) Thread.Sleep(100);
                    if (instance.ExitException != null)
                    {
                        context.Error.WriteLine("Executing '{0}' failed, exeption thrown:\n {1}", resolvedMethodName,
                            context.ShellApp.ReportLevel == ReportLevel.NoTrace ? instance.ExitException.Message : instance.ExitException.ToString());
                    }

                    return instance.ExitResult;
                }
                else
                {
                    context.ShellApp.RegisterParallel(instance);
                } 
            }
            catch (Exception e)
            {
                context.Error.WriteLine("Executing '{0}' failed, exeption thrown:\n {1}", resolvedMethodName, 
                    context.ShellApp.ReportLevel == ReportLevel.NoTrace ? e.Message : e.ToString());
            }

            return 1;

        }

        #endregion
    }
}
