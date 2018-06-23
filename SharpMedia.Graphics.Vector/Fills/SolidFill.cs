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
using SharpMedia.Graphics.Shaders;
using SharpMedia.Math;
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics.Vector.Fills
{

    /// <summary>
    /// A solid fill element. It fills shape with solid colour. It can be transparent.
    /// </summary>
    public class SolidFill : IFill
    {
        #region Private Members
        Colour colour;
        #endregion

        #region Constructors

        public SolidFill()
            : this(Colour.Black)
        {
        }

        public SolidFill(Colour colour)
        {
            this.colour = colour;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The fill colour.
        /// </summary>
        public Colour Colour
        {
            get
            {
                return colour;
            }
            set
            {
                colour = value;
            }
        }

        #endregion

        #region IFill Members

        public bool IsInstanceDependant
        {
            get
            {
                return false;
            }
        }

        public object[] ParameterValues
        {
            get { return new object[0]; }
        }

        public void Compile(ShaderCompiler compiler, ShaderCompiler.Operand position, ShaderCompiler.Operand texCoord,
            ShaderCompiler.Operand[] attributes, ShaderCompiler.Operand borderDistance, ShaderCompiler.Operand[] constants, 
            ShaderCompiler.Operand outputColour)
        {
            // The colour is saved in attribute.
            compiler.Mov(attributes[0], outputColour);
        }

        public uint CustomAttributeCount
        {
            get { return 1; }
        }

        public Vector4f CalcCustomAttribute([EqualUInt(0)] uint id, object shape, Vector2f positions)
        {
            return colour.RGBA;
        }

        #endregion

        #region IInterface Members

        public Type TargetOperationType
        {
            get { return typeof(FillElementOperation); }
        }

        public ParameterDescription[] AdditionalParameters
        {
            get { return new ParameterDescription[0]; }
        }

        #endregion

        #region IEquatable<IFill> Members

        public bool Equals(IFill other)
        {
            if (other is SolidFill)
            {
                SolidFill fill = (SolidFill)other;
                if (fill.colour != colour) return false;
                return true;
            }
            return false;
        }

        #endregion
    }
}
