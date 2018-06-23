using System;
using System.Collections.Generic;
using System.Text;
using SharpMedia.Components.Applications;
using SharpMedia.Components.Configuration;
using SharpMedia.Components.Configuration.ComponentProviders;

namespace SharpMedia.Tools
{

    /// <summary>
    /// Configures a tool. If output path is present, we output configured tool
    /// as RunTool with configuration. Otherwise, we run tool at the end.
    /// </summary>
    [AutoParametrize(PassUnmatchedArguments=false)]
    public class ConfigureTool : Application
    {
        #region Private Members
        string toolClassName;
        string automaticUIClassName = "SharpMedia.Tools.Textural.TextAutomaticUI";
        string outputPath = null;
        #endregion

        #region Private Members

        /// <summary>
        /// A tool name.
        /// </summary>
        [Required, DefaultName("ToolName")]
        public string ToolName
        {
            get { return toolClassName; }
            set { toolClassName = value; }
        }

        /// <summary>
        /// The automatic UI used.
        /// </summary>
        public string AutomaticUIName
        {
            get { return automaticUIClassName; }
            set { automaticUIClassName = value; }
        }

        /// <summary>
        /// The output path of tool.
        /// </summary>
        /// <remarks>The common paths are "System/Tools/*"</remarks>
        public string OutputPath
        {
            get { return outputPath; }
            set { outputPath = value; }
        }

        #endregion

        #region Implementation

        public override int Start(string verb, params string[] arguments)
        {
            // 1) We first parse class name.
            Type toolClass = Type.GetType(ToolName);
            if (toolClass == null)
            {
                console.WriteLine("The tool class '{0}' does not exist, cannot find it.", ToolName);
                return 1;
            }

            if (!Common.IsTypeSameOrDerived(typeof(ITool), toolClass))
            {
                console.WriteLine("The tool class '{0}' does not implement ITool interface, cannot run as tool", ToolName);
                return 1;
            }

            // 2) We now extract parameters and apply defaults.
            Parameters.IToolParameter[] parameters = Parameters.ParameterProcessor.Extract(toolClass, database);
            arguments = Parameters.ParameterProcessor.Apply(parameters, arguments, console);

            // 3) We extract automatic UI toolkit.
            Type automaticUIClass = Type.GetType(automaticUIClassName);
            if (automaticUIClass == null)
            {
                console.WriteLine("The automatic UI class '{0}' does not exist, cannot find it.", automaticUIClass);
                return 1;
            }

            if (!Common.IsTypeSameOrDerived(typeof(IAutomaticUI), automaticUIClass))
            {
                console.WriteLine("The automatic UI class '{0}' does not implement IAutomaticUI" +
                    "interface, cannot run as tool configurator.", automaticUIClass);
                return 1;
            }

            // 4) We create automatic UI.
            IAutomaticUI ui = componentDirectory.ConfigureInlineComponent(
                new ConfiguredComponent(automaticUIClassName)) as IAutomaticUI;

            // 5) We run parameter aquiring.
            if (!ui.Run(toolClass, parameters))
            {
                console.WriteLine("Configuration failed.");
                return 0;
            }

            // 6) Check if parameters are set (warnings).
            for (int i = 0; i < parameters.Length; i++)
            {
                if (!parameters[i].Attribute.IsOptional &&
                    !parameters[i].IsSet)
                {
                    console.WriteLine("Warning: parameter {0} is not " +
                        "optional and is not set", parameters[i].Name);
                }
            }

            // 7) We have to options; either run or output.
            if (outputPath != null)
            {
                throw new NotImplementedException();
            }
            else
            {
                // We run after configuration (set parameters manually).
                RunTool runTool = new RunTool();
                runTool.BatchedParameters = 
                    Parameters.ParameterProcessor.Unroll(parameters); //< We unroll them as batched
                runTool.Console = console;
                runTool.Database = database;
                runTool.ToolName = toolClassName;


                return runTool.Start(verb, arguments);
            }

            
        }

        #endregion
    }
}
