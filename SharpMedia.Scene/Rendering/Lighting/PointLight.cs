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
using SharpMedia.Graphics;

namespace SharpMedia.Scene.Lighting
{

    
    /// <summary>
    /// A point light source.
    /// </summary>
    public class PointLight : ILight
    {
        #region Private Members
        Colour lightColour = Colour.Black;
        #endregion


        #region ILight Members

        public SharpMedia.Graphics.Shaders.ShaderCompiler.Operand Lit(SharpMedia.Graphics.Shaders.ShaderCompiler compiler, SharpMedia.Graphics.Shaders.ShaderCompiler.Operand position, SharpMedia.Graphics.Shaders.ShaderCompiler.Operand normal, Materials.IBRDFInterface material)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IInterface Members

        public Type TargetOperationType
        {
            get { throw new NotImplementedException(); }
        }

        public SharpMedia.Graphics.Shaders.ParameterDescription[] AdditionalParameters
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
