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
using SharpMedia.Input;
using SharpMedia.AspectOriented;
using System.Collections.ObjectModel;
using SharpMedia.Math;
using System.Threading;
using SharpMedia.Graphics.GUI.Styles;
using SharpMedia.Graphics.GUI.Themes;

namespace SharpMedia.Graphics.GUI.Standalone
{
    

    /// <summary>
    /// Input router that routes input events to GuiManager. It is used in GUI standalone mode.
    /// </summary>
    /// <remarks>This class is used, if GUI is used without Window services.</remarks>
    public sealed class InputRouter
    {
        #region Private Members
        EventProcessor processor;
        Sensitivity sensitivity;

        GuiManager manager;
        IUserInteractive interactive { get { return manager; } }
        GuiPointer pointer;
        
        #endregion

        #region Routing Delegates

        void KeyUp(InputEvent ev)
        {
            interactive.OnKeyRelease(pointer, ev.KeyCode, ev.KeyboardModifiers);
        }

        void KeyDown(InputEvent ev)
        {
            interactive.OnKeyPress(pointer, ev.KeyCode, ev.KeyboardModifiers, ev.EventModifiers);
        }     

        #endregion

        #region Public Members

        /// <summary>
        /// Input router constructor.
        /// </summary>
        public InputRouter([NotNull] GuiManager manager,
            [NotNull] EventProcessor processor, Style style, 
            IGuiRenderer pointerRenderer, Sensitivity sensitivity)
        {
            if (sensitivity == null) sensitivity = new Sensitivity();

            this.processor = processor;
            this.manager = manager;
            this.sensitivity = sensitivity;

            processor.KeyDown += KeyDown;
            processor.KeyUp += KeyUp;
         

            this.pointer = new GuiPointer(manager, processor,
                style, pointerRenderer, sensitivity);
            manager.AddNLObject(pointer);
        }
         
        /// <summary>
        /// The mouse used.
        /// </summary>
        public EventProcessor Processor
        {
            get
            {
                return processor;
            }
        }

        /// <summary>
        /// The pointer of input router.
        /// </summary>
        public IPointer Pointer
        {
            get
            {
                return pointer;
            }
        }

        #endregion

    }

}
