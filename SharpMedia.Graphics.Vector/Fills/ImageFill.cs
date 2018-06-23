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
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics.Vector.Fills
{
    /// <summary>
    /// A texture fill. Texture coordinates are used to load pixels.
    /// </summary>
    /// <remarks>The texture view is not owned by the fill and must be disposed seperatelly.</remarks>
    public class ImageFill : IFill
    {
        #region IFill Members

        public object[] ParameterValues
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void Compile(SharpMedia.Graphics.Shaders.ShaderCompiler compiler, SharpMedia.Graphics.Shaders.ShaderCompiler.Operand position, SharpMedia.Graphics.Shaders.ShaderCompiler.Operand texCoord, SharpMedia.Graphics.Shaders.ShaderCompiler.Operand[] attributes, SharpMedia.Graphics.Shaders.ShaderCompiler.Operand borderDistance, SharpMedia.Graphics.Shaders.ShaderCompiler.Operand[] constants, SharpMedia.Graphics.Shaders.ShaderCompiler.Operand outputColour)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsInstanceDependant
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public uint CustomAttributeCount
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Vector4f CalcCustomAttribute(uint id, object shape, Vector2f position)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IInterface Members

        public Type TargetOperationType
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public SharpMedia.Graphics.Shaders.ParameterDescription[] AdditionalParameters
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region IEquatable<IFill> Members

        public bool Equals(IFill other)
        {
            if (other is ImageFill)
            {
                ImageFill fill = (ImageFill)other;
                return true;
            }
            return false;
        }

        #endregion
    }
}
