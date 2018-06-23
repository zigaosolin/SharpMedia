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

namespace SharpMedia.Components.TextConsole
{

    /// <summary>
    /// A silent console, for applications running in background.
    /// </summary>
    public sealed class SilentConsole : ITextConsole
    {
        #region Private Members
        System.IO.TextReader input;
        System.IO.TextWriter output;
        System.IO.TextWriter error;
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="error"></param>
        public SilentConsole(System.IO.TextReader input,
            System.IO.TextWriter output,
            System.IO.TextWriter error)
        {
            this.input = input;
            this.output = output;
            this.error = error;
        }

        public SilentConsole()
        {
            this.input = System.IO.TextReader.Null;
            this.output = System.IO.TextWriter.Null;
            this.error = System.IO.TextWriter.Null;
        }

        #endregion

        #region ITextConsole Members

        public int Width
        {
            get { return 0; }
        }

        public int Height
        {
            get { return 0; }
        }

        public string ReadLine()
        {
            return In.ReadLine();
        }

        public System.IO.TextReader In
        {
            get { return input; }
        }

        public Key ReadKey()
        {
            return new Key((char)input.Read());
        }

        public void WriteLine(string s)
        {
            output.WriteLine(s);
        }

        public void Write(string s)
        {
            output.Write(s);
        }

        public void WriteLine(string s, params object[] p)
        {
            output.WriteLine(s, p);
        }

        public void Write(string s, params object[] p)
        {
            output.Write(s, p);
        }

        public System.IO.TextWriter Out
        {
            get { return output; }
        }

        public System.IO.TextWriter Error
        {
            get { return error; }
        }

        public void SetPosition(int x, int y)
        {
        }

        public int X
        {
            get { return 0; }
        }

        public int Y
        {
            get { return 0; }
        }

        public void Clear()
        {
        }

        #endregion
    }
}
