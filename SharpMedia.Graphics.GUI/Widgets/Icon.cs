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
using SharpMedia.Graphics.Vector;
using SharpMedia.Math.Shapes.Compounds;
using SharpMedia.Math.Matrix;
using SharpMedia.Math;
using SharpMedia.AspectOriented;
using SharpMedia.Math.Shapes;
using System.Collections.ObjectModel;

namespace SharpMedia.Graphics.GUI.Widgets
{

    /// <summary>
    /// Icon data, in uniform coordinates [0,1]x[0,1].
    /// </summary>
    [Serializable]
    public sealed class IconData
    {
        #region Private Members
        IShapef shape;
        IPathf path;
        IMapper mapper;
        Pen pen;
        IFill fill;
        #endregion

        #region Properties

        /// <summary>
        /// A triangle mesh.
        /// </summary>
        public IShapef Shape
        {
            get { return shape; }
        }

        /// <summary>
        /// Gets path.
        /// </summary>
        public IPathf Path
        {
            get { return path; }
        }

        /// <summary>
        /// Custom mapper.
        /// </summary>
        public IMapper Mapper
        {
            get { return mapper; }
        }

        /// <summary>
        /// Is it filled, solid.
        /// </summary>
        public bool Solid
        {
            get { return pen == null; }
        }

        /// <summary>
        /// A pen.
        /// </summary>
        public Pen Pen
        {
            get { return pen; }
        }

        /// <summary>
        /// Gets fill, also for pen fill.
        /// </summary>
        public IFill Fill
        {
            get
            {
                if (fill != null) return fill;
                return pen.Fill;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Icon data with fill and mesh.
        /// </summary>
        /// <param name="fill"></param>
        /// <param name="mesh"></param>
        public IconData([NotNull] Pen pen, [NotNull] IPathf path)
            : this(pen, path, null)
        {

        }

        /// <summary>
        /// Icon data with pen and mesh.
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="mesh"></param>
        public IconData([NotNull] IFill fill, [NotNull] IShapef shape)
            : this(fill, shape, null)
        {

        }

        /// <summary>
        /// Pen with path initialization.
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="mesh"></param>
        /// <param name="mapper"></param>
        public IconData([NotNull] Pen pen, [NotNull] IPathf path, IMapper mapper)
        {
            this.pen = pen;
            this.path = path;
            this.mapper = mapper;
        }

        /// <summary>
        /// Shape with fill initialization.
        /// </summary>
        /// <param name="fill"></param>
        /// <param name="mesh"></param>
        /// <param name="mapper"></param>
        public IconData([NotNull] IFill fill, [NotNull] IShapef shape, IMapper mapper)
        {
            this.fill = fill;
            this.shape = shape;
            this.mapper = mapper;
        }

        #endregion
    }

    /// <summary>
    /// An icon based on vector graphics.
    /// </summary>
    [Serializable]
    public class Icon : Area
    {
        #region Private Members
        protected internal List<IconData> iconData = new List<IconData>();
        #endregion

        #region Constructors

        protected Icon(object dummy)
            : base(dummy)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Icon()
            : base((object)null)
        {
        }

        /// <summary>
        /// Constructor with icon data.
        /// </summary>
        /// <param name="data"></param>
        public Icon(params IconData[] data)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Icon data as read-only.
        /// </summary>
        public List<IconData> IconData
        {
            get
            {
                return new List<IconData>(iconData);
            }
            set
            {
                AssertMutable();
                iconData = value;
            }
        }

        #endregion
    }
}
