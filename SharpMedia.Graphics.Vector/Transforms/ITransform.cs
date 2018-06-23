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
using SharpMedia.Math;

namespace SharpMedia.Graphics.Vector.Transforms
{
    /// <summary>
    /// The data type to preprocess.
    /// </summary>
    public enum PreprocessDataType
    {
        /// <summary>
        /// The data is an array of Vector2f. The array can be written to and provided as
        /// a result.
        /// </summary>
        Positions,

        /// <summary>
        /// The data is an array of Vector2f as texture coordinates. The array can be written to.
        /// </summary>
        TextureCoordinates,

        /// <summary>
        /// The data to transform is pen width, as float. Float is also the result type.
        /// </summary>
        PenWidth,

        /// <summary>
        /// The data to transform is tesselation resolution, as float. Float is also the result type.
        /// </summary>
        TesselationResolution
    }


    /// <summary>
    /// A geometry transform.
    /// </summary>
    public interface ITransform : ICloneable<ITransform>
    {
        /// <summary>
        /// Is preprocessing transform needed.
        /// </summary>
        bool NeedsPreprocess { get; }

        /// <summary>
        /// Gets the runtime form.
        /// </summary>
        /// <value>The matrix form.</value>
        Matrix4x4f RuntimeForm { get; }

        /// <summary>
        /// Preprocesses the data.
        /// </summary>
        /// <param name="type">The data type.</param>
        /// <param name="data">The input data, read-write in case of arrays</param>
        /// <remarks>The actual data depends on type. If preprocessing is needed, all types
        /// must be implemented. </remarks>
        object Preprocess(PreprocessDataType type, object data);

    }

}
