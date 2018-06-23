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
    /// <summary>
    /// A Textual Console
    /// </summary>
    public interface ITextConsole
    {
        /// <summary>
        /// Width of a character line, in characters
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Height of the console, in lines
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Read a line of text (same as Input.ReadLine).
        /// </summary>
        /// <returns>A line of text is returned</returns>
        String ReadLine();

        /// <summary>
        /// The input reader.
        /// </summary>
        TextReader In { get; }

        /// <summary>
        /// Blocks the caller until a key is pressed in the console
        /// </summary>
        Key ReadKey();

        /// <summary>
        /// Writes a line of text to the console
        /// </summary>
        /// <param name="s"></param>
        void WriteLine(string s);

        /// <summary>
        /// Writes a string of text to the console
        /// </summary>
        /// <param name="s"></param>
        void Write(string s);

        /// <summary>
        /// Writes a line of text to the console, String.Format style
        /// </summary>
        void WriteLine(string s, params object[] p);

        /// <summary>
        /// Writes a string of text to the console, String.Format style
        /// </summary>
        void Write(string s, params object[] p);

        /// <summary>
        /// The output stream.
        /// </summary>
        TextWriter Out { get; }

        /// <summary>
        /// Error stream.
        /// </summary>
        TextWriter Error { get; }

        /// <summary>
        /// Sets the position for the next read
        /// </summary>
        /// <param name="x">X coordinate, in range [0, width)</param>
        /// <param name="y">Y coordinate, in range [0, height)</param>
        void SetPosition(int x, int y);

        /// <summary>
        /// Current X coordinate
        /// </summary>
        int X { get; }

        /// <summary>
        /// Current Y coordinate
        /// </summary>
        int Y { get; }

        /// <summary>
        /// Clears the console and sets X and Y to zero
        /// </summary>
        void Clear();
    }
}
