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

namespace SharpMedia.Math.Shapes.Storage.Builders
{


    /// <summary>
    /// A topology set builder.
    /// </summary>
    public interface ITopologySetBuilder<T> : ITopologySet
        where T : ITopologySet
    {
        /// <summary>
        /// Maximum number ofcontrol points pre-allocated by builder.
        /// </summary>
        /// <remarks>Returns uint.MaxValue if dynamic resizing is allowed.</remarks>
        uint MaxControlPoints { get; }

        /// <summary>
        /// Finishes building. The builder is invalid after this is called.
        /// </summary>
        /// <returns></returns>
        T Finish();


    }
}
