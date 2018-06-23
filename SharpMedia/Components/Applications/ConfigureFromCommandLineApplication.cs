using System;
using System.Collections.Generic;
using System.Text;
using SharpMedia.Components.Configuration;

namespace SharpMedia.Components.Applications
{
    public class ConfigureFromCommandLineApplication : CommandLineApplication
    {
        /// <summary>
        /// The actual class that will implement the application interface
        /// </summary>
        [Required]
        public string ActualComponent { get; set; }

        public override int Start(string[] arguments)
        {
            Type t = Type.GetType(ActualComponent);
            if (t == null)
            {
                throw new Exception(
                    String.Format("Application component {0} not found", ActualComponent));
            }
        }
    }
}
