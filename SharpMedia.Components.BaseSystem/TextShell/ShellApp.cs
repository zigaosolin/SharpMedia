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
using SharpMedia.Components.Applications;
using SharpMedia.Components.Database;
using SharpMedia.Components.Configuration;
using System.Threading;
using SharpMedia.Database;
using System.Reflection;
using SharpMedia.Scripting.CompilerFacilities;
using System.Collections;
using SharpMedia.Components.BaseSystem.TextShell.ExecutionTree;
using SharpMedia.Components.TextConsole;

namespace SharpMedia.Components.BaseSystem.TextShell
{
    /// <summary>
    /// Report level.
    /// </summary>
    public enum ReportLevel
    {
        Full,
        NoTrace,
        Minimal,
        None
    }

    /// <summary>
    /// The Shell application
    /// </summary>
    public sealed class ShellApp : Application
    {
        #region Private Members
        private string prompt = "{0} $ ";
        private Type[] shellCommandTypes = new Type[] { typeof(CommonShellCommands) };
        private Node<object> workingDirectory = null;
        private IApplicationInstances applicationInstances;
        private IComponentDirectory currentComponentDirectory;
        private SortedDictionary<string, string> aliases = new SortedDictionary<string,string>();
        private ITextConsole silentConsole = null; //= new SilentConsole();
        private string[] executableSearchPaths = new string[]
            {
                "System/Applications/",
                "System/Applications/Components/"
            };
        private ReportLevel reportLevel = ReportLevel.NoTrace;
        #endregion

        #region Properties

        /// <summary>
        /// Prompt string.
        /// </summary>
        public string Prompt
        {
            get { return prompt;  }
            set { prompt = value; }
        }

        /// <summary>
        /// Shell report level.
        /// </summary>
        [Persistant]
        public ReportLevel ReportLevel
        {
            get { return reportLevel; }
            set { reportLevel = value; }
        }

        /// <summary>
        /// Silent console, for background actions.
        /// </summary>
        [DefaultName("SharpMedia.SilentConsole")]
        public ITextConsole SilentConsole
        {
            get { return silentConsole; }
            set { silentConsole = value; }
        }

        /// <summary>
        /// Executable search paths.
        /// </summary>
        [Persistant]
        public string[] ExecutableSearchPaths
        {
            get { return executableSearchPaths; }
            set { executableSearchPaths = value; }
        }

        /// <summary>
        /// Shell commands.
        /// </summary>
        public Type[] ShellCommandTypes
        {
            get { return shellCommandTypes; }
            set { shellCommandTypes = value; }
        }

        /// <summary>
        /// Aliases of shell application.
        /// </summary>
        [Persistant]
        public SortedDictionary<string, string> Aliases
        {
            get { return aliases; }
            set { aliases = value; }
        }

        /// <summary>
        /// Working directory of shell.
        /// </summary>
        [Required, DefaultName("WorkingDirectory")]
        public Node<Object> WorkingDirectory
        {
            get { return workingDirectory; }
            set { workingDirectory = value; }
        }

        /// <summary>
        /// Application instances.
        /// </summary>
        [Required]
        public IApplicationInstances ApplicationInstances
        {
            get { return applicationInstances; }
            set { applicationInstances = value; }
        }

        #endregion

        #region Private Members

        internal void RegisterParallel(IApplicationInstance instance)
        {
            // For now ignore it, in future we may allow managment, switch to it or something like that.
        }

        /// <summary>
        /// Creates silent context for parallel execution.
        /// </summary>
        internal ExecutionContext CreateParallelContext(ExecutionContext context, IExecutionCommand command)
        {
            // Optimization.
            if (command is ConstantCommand) return context;

            ExecutionContext newContext = context.Clone();
            newContext.Input = silentConsole.In;
            newContext.Output = silentConsole.Out;
            newContext.Error = silentConsole.Error;
            newContext.IsParallel = true;

            return newContext;
        }

        /// <summary>
        /// Creates a context for sub-command execution.
        /// </summary>
        internal ExecutionContext CreateSubContext(ExecutionContext context, IExecutionCommand command)
        {
            // Optimization.
            if (command is ConstantCommand) return context;

            ExecutionContext newContext = context.Clone();
            newContext.Input = silentConsole.In;
            newContext.Output = silentConsole.Out;
            newContext.Error = silentConsole.Error;
            newContext.IsParallel = false;

            return newContext;
        }

        bool ParseLine(string line)
        {
            if (line.Trim().ToLower() == "exit") return false;

            // We extract command.
            IExecutionCommand command = null;
            try
            {
                command = ExecutionCommandParser.Parse(line);
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Parsing failed: {0}", ex);
            }

            if (command == null) return true;

            try
            {
                ExecutionContext context = new ExecutionContext();
                context.Error = Console.Error;
                context.Input = Console.In;
                context.Output = Console.Out;
                context.ShellApp = this;
                
                object[] dummy;
                int r = command.Exec(context, out dummy);
                if (r < 0)
                {
                    context.Error.WriteLine("Executing command failed with result {0}.", r);
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Execution failed: {0}", ex);
            }
            

            return true;
        }

        #endregion

        #region Overrides

        public override int Start(string verb, string[] args)
        {
            while (true)
            {
                Console.Write(String.Format(prompt, WorkingDirectory.Path));
                string line = Console.ReadLine();

                if (!ParseLine(line)) break;
                Console.WriteLine(string.Empty);
            }
            return 1;
        }

        private int StartScript(Script scr)
        {
            foreach (string line in scr.Lines)
            {
                if (!ParseLine(line)) break;
            }

            return 1;
        }

        #endregion
    }
}
