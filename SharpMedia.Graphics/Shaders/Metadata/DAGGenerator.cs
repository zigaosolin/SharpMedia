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
using SharpMedia.Graphics.Shaders.Operations;

namespace SharpMedia.Graphics.Shaders.Metadata
{

    /// <summary>
    /// A DAG generator is an entry element for all metadata programming. It
    /// can create inputs, you can bind pins as outputs and you want.
    /// </summary>
    public class DAGGenerator
    {
        #region Private Members
        DAG dag;
        #endregion

        #region Properties

        /// <summary>
        /// Accesses a DAG generated using metadata programming.
        /// </summary>
        public DAG DAG
        {
            get
            {
                return dag;
            }
        }

        #endregion

        #region Fixed Construction

        /// <summary>
        /// Creates fixed float.
        /// </summary>
        public Floatx1 Fixed(float f)
        {
            Constant c = Constant.FromScalar(null, f);
            return new Floatx1(c.Outputs[0], this);
        }

        #endregion

    }
}
