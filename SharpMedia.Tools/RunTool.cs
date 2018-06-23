using System;
using System.Collections.Generic;
using System.Text;
using SharpMedia.Components.Applications;
using SharpMedia.Components.Configuration;
using SharpMedia.Components.Configuration.ComponentProviders;

namespace SharpMedia.Tools
{

    /// <summary>
    /// A run tool application.
    /// </summary>
    [AutoParametrize(PassUnmatchedArguments=false)]
    public sealed class RunTool : Application
    {
        #region Private Members
        string toolClassName;
        string[] parameters = new string[0];
        #endregion

        #region Properties

        /// <summary>
        /// A tool class name.
        /// </summary>
        [Required]
        public string ToolName
        {
            get { return toolClassName; }
            set { toolClassName = value; }
        }

        /// <summary>
        /// Batched parameters, if pre-configured.
        /// </summary>
        [Persistant]
        public string[] BatchedParameters
        {
            get { return parameters; }
            set { parameters = value; }
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

            // 2) We prepare tool parameters.
            Parameters.IToolParameter[] toolParameters = Parameters.ParameterProcessor.Extract(toolClass, database);
            arguments = Parameters.ParameterProcessor.Apply(toolParameters,
                Common.ArrayMerge(parameters, arguments), console);

            // 3) We set directory.
            ComponentDirectory newDirectory = new ComponentDirectory(componentDirectory, "RunToolDirectory");

            for (int i = 0; i < toolParameters.Length; i++)
            {
                toolParameters[i].Apply(newDirectory);
            }

            // 4) We now run the tool.
            try
            {
                ITool tool = newDirectory.ConfigureInlineComponent(
                    new ConfiguredComponent(toolClassName)) as ITool;
                return tool.Run(arguments);
            }
            catch (Exception ex)
            {
                console.Write(ex.ToString());
                return 1;
            }
            
        }

        #endregion
    }
}
