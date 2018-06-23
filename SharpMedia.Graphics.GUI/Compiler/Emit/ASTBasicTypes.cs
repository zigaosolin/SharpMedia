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
using System.Globalization;

namespace SharpMedia.Graphics.GUI.Compiler.Emit
{
    /// <summary>
    /// A string value.
    /// </summary>
    internal class ASTString : IEmittable
    {
        #region Private Members
        string data = string.Empty;
        #endregion

        #region Public Members

        /// <summary>
        /// The value.
        /// </summary>
        public string Value
        {
            get { return data; }
            set { data = value; }
        }
        #endregion

        #region IEmittable Members

        public void Emit(CompileContext context, System.IO.TextWriter writer)
        {
            writer.Write("\"{0}\"", data);
        }

        #endregion

        #region IElement Members

        public List<IElement> Children
        {
            get { return new List<IElement>(); }
        }

        #endregion
    }

    /// <summary>
    /// A int value.
    /// </summary>
    internal class ASTInt : IEmittable
    {
        #region Private Members
        int data = 0;
        #endregion

        #region Public Members

        /// <summary>
        /// The value.
        /// </summary>
        public int Value
        {
            get { return data; }
            set { data = value; }
        }
        #endregion

        #region IEmittable Members

        public void Emit(CompileContext context, System.IO.TextWriter writer)
        {
            writer.Write(data.ToString(CultureInfo.InvariantCulture.NumberFormat));
        }

        #endregion

        #region IElement Members

        public List<IElement> Children
        {
            get { return new List<IElement>(); }
        }

        #endregion
    }

    /// <summary>
    /// A uinz value.
    /// </summary>
    internal class ASTUInt : IEmittable
    {
        #region Private Members
        uint data = 0;
        #endregion

        #region Public Members

        /// <summary>
        /// The value.
        /// </summary>
        public uint Value
        {
            get { return data; }
            set { data = value; }
        }
        #endregion

        #region IEmittable Members

        public void Emit(CompileContext context, System.IO.TextWriter writer)
        {
            writer.Write(data.ToString(CultureInfo.InvariantCulture.NumberFormat));
        }

        #endregion

        #region IElement Members

        public List<IElement> Children
        {
            get { return new List<IElement>(); }
        }

        #endregion
    }

    /// <summary>
    /// A bool value.
    /// </summary>
    internal class ASTBool : IEmittable
    {
        #region Private Members
        bool data = false;
        #endregion

        #region Public Members

        /// <summary>
        /// The value.
        /// </summary>
        public bool Value
        {
            get { return data; }
            set { data = value; }
        }
        #endregion

        #region IEmittable Members

        public void Emit(CompileContext context, System.IO.TextWriter writer)
        {
            writer.Write(data.ToString(CultureInfo.InvariantCulture.NumberFormat));
        }

        #endregion

        #region IElement Members

        public List<IElement> Children
        {
            get { return new List<IElement>(); }
        }

        #endregion
    }

    /// <summary>
    /// A string value.
    /// </summary>
    internal class ASTFloat : IEmittable
    {
        #region Private Members
        float data = 0.0f;
        #endregion

        #region Public Members

        /// <summary>
        /// The value.
        /// </summary>
        public float Value
        {
            get { return data; }
            set { data = value; }
        }
        #endregion

        #region IEmittable Members

        public void Emit(CompileContext context, System.IO.TextWriter writer)
        {
            writer.Write(data.ToString(CultureInfo.InvariantCulture.NumberFormat));
        }

        #endregion

        #region IElement Members

        public List<IElement> Children
        {
            get { return new List<IElement>(); }
        }

        #endregion
    }
}
