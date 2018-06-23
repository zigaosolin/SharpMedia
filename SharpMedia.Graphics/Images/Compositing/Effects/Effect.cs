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
using SharpMedia.Math;

namespace SharpMedia.Graphics.Images.Compositing.Effects
{

    /// <summary>
    /// A base class for all effects
    /// </summary>
    public abstract class Effect : ICompositingOperation
    {
        #region Private Members
        ICompositingOperation source;
        #endregion

        #region ICompositingOperation Members

        public OperationType OperationType
        {
            get { return OperationType.OneSource; }
        }

        public ICompositingOperation Source1
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
            }
        }

        public ICompositingOperation Source2
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public abstract ICompositeInterface Interface
        {
            get;
        }

        public virtual Vector2i Size
        {
            get { return source.Size; }
        }

        #endregion
    }
}
