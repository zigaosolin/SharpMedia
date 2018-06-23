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
using SharpMedia.Components.Configuration;
using SharpMedia.Components.TextConsole;
using SharpMedia.Database;
using System.Reflection;
using SharpMedia.Components.Configuration.ComponentProviders;

namespace SharpMedia.Components.Applications
{
    /// <summary>
    /// A base application class with some automatic components and auto parametrization
    /// functionalities.
    /// </summary>
    public abstract class Application : IApplicationBase
    {
        #region Protected Members
        protected ITextConsole console;
        protected IComponentDirectory componentDirectory;
        protected DatabaseManager database;
        #endregion

        #region Components

        /// <summary>
        /// A database manager.
        /// </summary>
        [Required]
        public DatabaseManager Database
        {
            get { return database; }
            set { database = value; }
        }

        /// <summary>
        /// Standard output.
        /// </summary>
        [Required]
        public ITextConsole Console
        {
            get { return console; }
            set { console = value; }
        }

        /// <summary>
        /// Component directory.
        /// </summary>
        [Required]
        public IComponentDirectory ComponentDirectory
        {
            get { return componentDirectory; }
            set { componentDirectory = value; }
        }

        #endregion

        #region IApplicationBase Members

        public abstract int Start(string verb, params string[] arguments);

        #endregion
    }
}
