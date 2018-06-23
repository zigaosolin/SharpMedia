using System;
using System.Collections.Generic;
using System.Text;
using SharpMedia.Graphics.Vector.Fills;

namespace SharpMedia.Graphics.Vector.Documents
{

    /// <summary>
    /// A VG document based fill object.
    /// </summary>
    [Serializable]
    public sealed class VGFill : VGObject
    {
        #region Private Members
        IFill fill;
        #endregion

        #region Overrides

        public override object UnderlayingObject
        {
            get { return fill; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a named fill.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fill"></param>
        public VGFill(string name, IFill fill)
            : base(name)
        {
            this.fill = fill;
        }

        /// <summary>
        /// Creates an un-nammed (inline) fill.
        /// </summary>
        /// <param name="fill"></param>
        public VGFill(IFill fill)
        {
            this.fill = fill;
        }

        #endregion
    }
}
