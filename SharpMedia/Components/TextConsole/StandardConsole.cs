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
using System.IO;

namespace SharpMedia.Components.TextConsole
{
    public class StandardConsole : ITextConsole
    {
        #region ITextConsole Members

        public int Width
        {
            get { return Console.BufferWidth; }
        }

        public int Height
        {
            get { return Console.BufferHeight; }
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public Key ReadKey()
        {
            return new Key(Console.ReadKey());
        }

        public void WriteLine(string s)
        {
            Console.WriteLine(s);
        }

        public void Write(string s)
        {
            Console.Write(s);
        }

        public void WriteLine(string s, params object[] p)
        {
            Console.WriteLine(s, p);
        }

        public void Write(string s, params object[] p)
        {
            Console.Write(s, p);
        }

        public void SetPosition(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }

        public int X
        {
            get { return Console.CursorLeft; }
        }

        public int Y
        {
            get { return Console.CursorTop; }
        }
        
        public void Clear()
        {
            Console.Clear();
            SetPosition(0, 0);
        }

        public TextReader In
        {
            get { return Console.In; }
        }

        public TextWriter Out
        {
            get { return Console.Out; }
        }

        public TextWriter Error
        {
            get { return Console.Error; }
        }

        #endregion
    }
}
