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
using SharpMedia.Database;
using SharpMedia.Components.TextConsole;
using SharpMedia.Components.Configuration;
using System.IO;

namespace SharpMedia.Scripting
{

    /// <summary>
    /// A script run application. Given scripts, it will run it.
    /// </summary>
    /// <remarks>Can run scripts one after another.</remarks>
    public sealed class ScriptRunApp : Application
    {
        #region Private Members
        bool detailedOutput = false;
        bool terminateOnError = false;

        private TextWriter errorStream
        {
            get { return console.Error; }
        }
        #endregion

        #region Properties

        /// <summary>
        /// Is detailed output is desired or not.
        /// </summary>
        public bool DetailedOutput
        {
            get { return detailedOutput; }
            set { detailedOutput = value; }
        }

        /// <summary>
        /// Should terminate if one script failed.
        /// </summary>
        public bool TerminateOnError
        {
            get { return terminateOnError; }
            set { terminateOnError = value; }
        }

        public ScriptRunApp()
        {
        }

        #endregion

        #region Execution

        public override int Start(string verb, string[] arguments)
        {
            if(arguments.Length < 1)
            {
                throw new ArgumentException("Must have at least one argument representing the script to execute.");
            }

            bool errorsPresent = false;

            // We run each script.
            foreach (string path in arguments)
            {
                Node<object> node = this.database.Find(path);
                if (node == null)
                {
                    errorStream.WriteLine("Script '{0}' could not be run because it was not found.", path);
                    continue;
                }

                try
                {
                    if (detailedOutput)
                    {
                        errorStream.WriteLine("Running script {0}", path);
                    }

                    ScriptEngine.ImportAndRun(node.Object);
                }
                catch (Exception ex)
                {
                    errorsPresent = true;
                    errorStream.WriteLine("Script run failed: {0}", ex.Message);

                    if (detailedOutput)
                    {
                        errorStream.WriteLine(ex.StackTrace);
                    }

                    if (terminateOnError) break;
                }
            }


            // We returns with errors present.
            return errorsPresent ? -1 : 0;
        }

        #endregion
    }
}
