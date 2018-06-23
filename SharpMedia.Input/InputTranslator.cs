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

namespace SharpMedia.Input
{
    /// <summary>
    /// Enumerator of keyboard modifiers.
    /// </summary>
    [Flags]
    public enum KeyboardModifiers
    {
        None = 0,
        LShift = 1,
        RShift = 2,
        LAlt = 4, 
        RAlt = 8,
        LCtrl = 16,
        RCtrl = 32,
        Win = 64
    }

    /// <summary>
    /// Can translate numeric codes based on language to characters.
    /// </summary>
    public static class InputTranslator
    {
        /// <summary>
        /// A simple english translation of key-code with modifiers to character.
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static char Translate(KeyCodes code, KeyboardModifiers modifiers)
        {
            throw new NotImplementedException();
        }

    }
}
