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
using SharpMedia.Math.Matrix;
using SharpMedia.AspectOriented;
using SharpMedia.Math;

namespace SharpMedia.Graphics.Vector.Mappers
{
    /// <summary>
    /// Transforms the mapping coordinates.
    /// </summary>
    /// <remarks>It is not thread safe.</remarks>
    public sealed class TransformMapper : IMapper
    {
        #region Private Members
        IMapper internalMapper;
        Matrix3x3f transform = Matrix3x3f.Identity;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the internal mapper.
        /// </summary>
        public IMapper InternalMapper
        {
            get
            {
                return internalMapper;
            }
            [param: NotNull]
            set
            {
                internalMapper = value;
            }
        }

        /// <summary>
        /// Gets or sets the transform.
        /// </summary>
        public Matrix3x3f Transform
        {
            get
            {
                return transform;
            }
            set
            {
                transform = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// The mapper with transform.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="transform"></param>
        public TransformMapper([NotNull] IMapper mapper, Matrix3x3f transform)
        {
            this.internalMapper = mapper;
            this.transform = transform;
        }

        /// <summary>
        /// Internal mapper constructor.
        /// </summary>
        /// <param name="mapper"></param>
        public TransformMapper([NotNull] IMapper mapper)
        {
            this.internalMapper = mapper;
        }

        #endregion

        #region IMapper Members

        public Vector2f[] Generate(object shape, Vector2f[] position)
        {
            // We generate result.
            Vector2f[] res = internalMapper.Generate(shape, position);

            // We now scale them.
            for (int i = 0; i < res.Length; i++)
            {
                // We transform in homogenus coordiantes.
                res[i] = transform * res[i];
            }

            return res;
        }

        #endregion
    }
}
