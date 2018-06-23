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
using SharpMedia.Graphics.GUI;
using SharpMedia.Graphics.GUI.Widgets;
using SharpMedia.Graphics.GUI.Metrics;
using SharpMedia.Math;
using SharpMedia.Input;
using SharpMedia.Graphics.GUI.Widgets.Containers;


namespace SharpMedia.Shell
{


    
    /// <summary>
    /// A text console application.
    /// </summary>
    public sealed class ShellTextConsole : WindowShellApplication, 
                Components.TextConsole.ITextConsole
    {
        #region Private Members
        object syncRoot = new object();
        StringBuilder buffer = new StringBuilder();
        StringBuilder currentLine = new StringBuilder();
        BlockMode mode = BlockMode.NoBlocking;

        enum BlockMode
        {
            SingleCharacter,
            Line,
            NoBlocking
        }
        #endregion

        #region Private Methods

        void KeyPress(Area area, IPointer pointer, SharpMedia.Input.KeyCodes code,
                             SharpMedia.Input.KeyboardModifiers modeifiers, InputEventModifier inputModifier)
        {
            char chr = InputTranslator.Translate(code, modeifiers);
            buffer.Append(chr);
            currentLine.Append(chr);

            // FIXME: maximum buffer size.

            if (chr == '\n')
            {
                // We signal line read.
            }

        }

        #endregion

        #region Overrides

        public override int StartDocument(SharpMedia.Components.Applications.DocumentParameter[] parameters)
        {
            // We expect no parameters.
            if (parameters.Length != 0)
            {
                throw new Exception("ShellTextConsole expects no document parameters.");
            }

            // We first configure the appearance. We only use one label for all text.
            Container sheet = new Container();
            Label textArea = new Label();

            using (sheet.Enter())
            {
                sheet.AddChild(textArea);
            }

            using (textArea.Enter())
            {
                textArea.PreferredRect = new GuiRect(RectangleMode.MinMax,
                    new GuiVector2(Vector2f.Zero, Vector2f.Zero, new Vector2f(0, 0)),
                    new GuiVector2(Vector2f.Zero, Vector2f.Zero, new Vector2f(1, 1)));

                textArea.Events.KeyPress += KeyPress;
            }

            // We now create root.
            RootWindow root = CreateRoot(textArea);



            return 0;
        }

        

        #endregion

        #region ITextConsole Members

        public void Clear()
        {
            buffer.Length = 0;
        }

        public System.IO.TextWriter Error
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public int Height
        {
            get { return clientWindow.Size.Y; }
        }

        public System.IO.TextReader In
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public System.IO.TextWriter Out
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public SharpMedia.Components.TextConsole.Key ReadKey()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string ReadLine()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPosition(int x, int y)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Width
        {
            get { return clientWindow.Size.X; }
        }

        public void Write(string s, params object[] p)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Write(string s)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void WriteLine(string s, params object[] p)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void WriteLine(string s)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int X
        {
            get { return clientWindow.Position.X; }
        }

        public int Y
        {
            get { return clientWindow.Position.Y; }
        }

        #endregion
    }
}
