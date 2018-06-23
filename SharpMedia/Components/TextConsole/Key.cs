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

namespace SharpMedia.Components.TextConsole
{
    [Flags]
    public enum Modifier : int
    {
        None = 0,
        Alt = 1,
        Shift = 2,
        Ctrl = 4
    }

    /// <summary>
    /// A Keyboard key
    /// </summary>
    [Ignore]
    public class Key
    {
        #region Private Members
        private char translatedCharacter;
        private Modifier modifierKeys;
        #endregion

        #region Public members

        
        public char TranslatedCharacter
        {
            get { return translatedCharacter; }
            set { translatedCharacter = value; }
        }

        
        /// <summary>
        /// Modifier keys
        /// </summary>
        public Modifier ModifierKeys
        {
            get { return modifierKeys; }
            set { modifierKeys = value; }
        }

        /// <summary>
        /// Console key info constructor.
        /// </summary>
        /// <param name="keyInfo"></param>
        public Key(ConsoleKeyInfo keyInfo)
        {
            this.translatedCharacter = keyInfo.KeyChar;
            this.modifierKeys = Modifier.None;
            if ((keyInfo.Modifiers & ConsoleModifiers.Alt) != 0)
            {
                this.modifierKeys |= Modifier.Alt;
            }

            if ((keyInfo.Modifiers & ConsoleModifiers.Shift) != 0)
            {
                this.modifierKeys |= Modifier.Shift;
            }

            if ((keyInfo.Modifiers & ConsoleModifiers.Control) != 0)
            {
                this.modifierKeys |= Modifier.Ctrl;
            }
        }

        /// <summary>
        /// Key constructor.
        /// </summary>
        /// <param name="key"></param>
        public Key(char key)
        {
            this.translatedCharacter = key;
            // TODO: modifiers
        }

        #endregion
    }
}
