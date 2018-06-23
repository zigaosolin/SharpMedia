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
using SharpMedia.Graphics;
using SharpMedia.Components.Database;
using SharpMedia.Shell;
using SharpMedia.Components.Configuration;
using SharpMedia.Database;
using SharpMedia.Components.Configuration;
using SharpMedia.Components;

namespace SharpMedia.Tools.Painter
{

    /// <summary>
    /// A painter program.
    /// </summary>
    public sealed class Painter : Shell.WindowShellApplication
    {
        #region Private Members
        GraphicsService graphicsService;
        IWindowManager windowManager;

        // Required data.
        RootWindow painterWindow;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a painter.
        /// </summary>
        public Painter()
        {
        }

        #endregion

        #region Properties

        [Required]
        public GraphicsService GraphicsService
        {
            get
            {
                return graphicsService;
            }
            set
            {
                graphicsService = value;
            }
        }

        [Required]
        public IWindowManager WindowManager
        {
            get
            {
                return windowManager;
            }
            set
            {
                windowManager = value;
            }
        }

        #endregion

        #region Overrides

        public override int StartDocument(SharpMedia.Components.Applications.DocumentParameter[] parameters)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
