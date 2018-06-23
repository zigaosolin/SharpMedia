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
using SharpMedia.AspectOriented;
using SharpMedia.Graphics.Shaders;
using SharpMedia.Math;

namespace SharpMedia.Graphics.Vector.Fills
{

    /// <summary>
    /// A radial fill, given a center point and radius.
    /// </summary>
    public class RadialFill : IFill
    {
        #region Private Members
        float radius = 1.0f;
        Colour innerColour = Colour.White;
        Colour outerColour = Colour.Black;
        Vector2f centerPosition = new Vector2f(0, 0);
        #endregion

        #region Public Members

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RadialFill()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="inner"></param>
        /// <param name="outer"></param>
        public RadialFill(Vector2f center, float radius, Colour inner, Colour outer)
        {
            this.innerColour = inner;
            this.outerColour = outer;
            this.centerPosition = center;
            this.radius = radius;
        }

        /// <summary>
        /// Radius of radial fill.
        /// </summary>
        public float Radius
        {
            get
            {
                return radius;
            }
            [param: MinFloat(0.0f)]
            set
            {
                radius = value;
            }
        }

        /// <summary>
        /// Gets or sets center position.
        /// </summary>
        public Vector2f CenterPosition
        {
            get
            {
                return centerPosition;
            }
            set
            {
                centerPosition = value;
            }
        }

        /// <summary>
        /// Gets or sets the inner colour.
        /// </summary>
        public Colour InnerColour
        {
            get
            {
                return innerColour;
            }
            set
            {
                innerColour = value;
            }
        }

        /// <summary>
        /// Gets or set s the outer colour.
        /// </summary>
        public Colour OuterColour
        {
            get
            {
                return outerColour;
            }
            set
            {
                outerColour = value;
            }
        }

        #endregion

        #region IFill Members

        public object[] ParameterValues
        {
            get 
            {
                return new object[] { radius, centerPosition, innerColour.RGBA, outerColour.RGBA };
            }
        }

        public void Compile(ShaderCompiler compiler, ShaderCompiler.Operand position, ShaderCompiler.Operand texCoord, ShaderCompiler.Operand[] attributes,
            ShaderCompiler.Operand borderDistance, ShaderCompiler.Operand[] constants, ShaderCompiler.Operand outputColour)
        {
            ShaderCompiler.Operand radius = constants[0];
            ShaderCompiler.Operand centerPos = constants[1];
            ShaderCompiler.Operand colourInner = constants[2];
            ShaderCompiler.Operand colourOuter = constants[3];

            // We now compute the distance.
            // float dist = |centerPos-textCoord|;
            // float f = min(1.0f, dist);
            // return (1-f)*inner + f*outer;
            ShaderCompiler.Operand dist = compiler.Call(ShaderFunction.Length, compiler.Sub(centerPos, texCoord));
            ShaderCompiler.Operand f = compiler.Min(dist, compiler.CreateFixed(PinFormat.Float, Pin.NotArray, 1.0f));
            compiler.Add(compiler.Mul(compiler.Sub(compiler.CreateFixed(PinFormat.Float, Pin.NotArray, 1.0f), f), colourInner),
                compiler.Mul(f, colourOuter), outputColour);
        }

        public bool IsInstanceDependant
        {
            get { return true; }
        }

        public uint CustomAttributeCount
        {
            get { return 0; }
        }

        public SharpMedia.Math.Vector4f CalcCustomAttribute(uint id, object shape, SharpMedia.Math.Vector2f position)
        {
            throw new ArgumentException("Id invalid.");
        }

        #endregion

        #region IInterface Members

        public Type TargetOperationType
        {
            get { return typeof(FillElementOperation); }
        }

        public ParameterDescription[] AdditionalParameters
        {
            get 
            {
                return new ParameterDescription[]
                {
                    new ParameterDescription("Radius", new Pin(PinFormat.Float, Pin.NotArray, null)),
                    new ParameterDescription("CenterPosition", new Pin(PinFormat.Floatx2, Pin.NotArray, null)),
                    new ParameterDescription("InnerColour", new Pin(PinFormat.Floatx4, Pin.NotArray, null)),
                    new ParameterDescription("OuterColour", new Pin(PinFormat.Floatx4, Pin.NotArray, null))
                };
            }
        }

        #endregion

        #region IEquatable<IFill> Members

        public bool Equals(IFill other)
        {
            if (other is RadialFill)
            {
                RadialFill fill = (RadialFill)other;
                if (!Vector2f.NearEqual(fill.centerPosition, centerPosition)) return false;
                if (innerColour != fill.innerColour) return false;
                if (outerColour != fill.outerColour) return false;
                return true;
            }
            return false;
        }

        #endregion
    }
}
