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
using SharpMedia.Math;
using SharpMedia.Graphics.Shaders;

namespace SharpMedia.Graphics.Vector.Fills
{

    /// <summary>
    /// Gradient fill defines the base class for different gradients that are then
    /// optimized for their specific properties.
    /// </summary>
    /// <remarks>
    /// When gradient is created, number of colours is fixed. Other properties
    /// can change. The number of positions equals number of colours - 2.
    /// </remarks>
    public abstract class GradientFill : IFill
    {
        #region Protected Members
        protected Colour[] colours;
        protected float[] positions;
        protected Vector2f texcoordMixing = new Vector2f(1,1);
        #endregion

        #region Properties

        /// <summary>
        /// Number of colours.
        /// </summary>
        public uint ColourCount
        {
            get
            {
                return (uint)colours.Length;
            }
        }

        /// <summary>
        /// Number of colour position, first and the last one are fixed to 0.0 and 1.0.
        /// </summary>
        public uint PositionCount
        {
            get
            {
                return (uint)positions.Length;
            }
        }

        /// <summary>
        /// Colours of gradient.
        /// </summary>
        public Colour[] Colours
        {
            get
            {
                return colours.Clone() as Colour[];
            }
            [param: NotNull]
            set
            {
                if (value.Length != colours.Length)
                {
                    throw new ArgumentException(string.Format("The colours must be {0} element long", colours.Length));
                }

                for (int i = 0; i < colours.Length; i++)
                {
                    colours[i] = value[i];
                }
            }
        }

        /// <summary>
        /// Positions of gradient. First is assumed to be 0 and last to be 1.
        /// </summary>
        /// <remarks>Get wii return the copy of array, setting will also copy the array.</remarks>
        public float[] Positions
        {
            get
            {
                return positions.Clone() as float[];
            }
            [param: NotNull]
            set
            {
                if (value.Length != positions.Length)
                {
                    throw new ArgumentException(string.Format("The weight must be {0} element long", positions.Length));
                }

                if (value.Length > 0)
                {
                    if (value[0] < 0.0f || value[0] > 1.0f)
                    {
                        throw new ArgumentException("Positions in range [0,1] acceptable.");
                    }
                }

                // We check order and values.
                for (int i = 1; i < value.Length; i++)
                {
                    if (value[i] < value[i - 1] || value[i] > 1.0f)
                    {
                        throw new ArgumentException("Positions must be in range [0,1] and in accending order.");
                    }
                }

                for (int i = 0; i < positions.Length; i++)
                {
                    positions[i] = value[i];
                }
            }
        }

        /// <summary>
        /// Relative mixing for gradient position.
        /// </summary>
        public Vector2f Mixing
        {
            get
            {
                return texcoordMixing;
            }
            set
            {
                texcoordMixing = value;
            }
        }


        #endregion

        #region Static Methods

        /// <summary>
        /// Creates gradient with a given number of colours and gradient type.
        /// </summary>
        /// <param name="colours"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static GradientFill CreateGradient([MinUInt(2)] uint colours)
        {
            GradientFill fill = null;

            if (colours <= 4)
            {
                fill = new Gradient4Fill();
            }

            // We create and initialize it.
            if (fill != null)
            {
                fill.colours = new Colour[colours];
                fill.positions = new float[colours - 2];
                return fill;
            }

            throw new NotImplementedException("This kind of gradient not yet supported.");
        }

        /// <summary>
        /// Creates gradient with given colours, positions and type.
        /// </summary>
        /// <returns></returns>
        public static GradientFill CreateGradient([NotEmptyArray] Colour[] colours, 
            [NotNull] float[] positions, Vector2f texMixing)
        {
            if (colours.Length-2 != positions.Length)
            {
                throw new ArgumentException("Number of positions must be number of colours - 2.");
            }

            // We now create fill.
            GradientFill fill = CreateGradient((uint)colours.Length);
            fill.Colours = colours;
            fill.Positions = positions;
            fill.texcoordMixing = texMixing;

            return fill;
        }

        #endregion

        #region IFill Members

        public abstract object[] ParameterValues
        {
            get;
        }

        public abstract void Compile(ShaderCompiler compiler, ShaderCompiler.Operand position, ShaderCompiler.Operand texCoord,
            ShaderCompiler.Operand[] attributes, ShaderCompiler.Operand borderDistance, ShaderCompiler.Operand[] constants,
            ShaderCompiler.Operand outputColour);

        public abstract bool IsInstanceDependant
        {
            get;
        }

        public abstract uint CustomAttributeCount
        {
            get;
        }

        public abstract Vector4f CalcCustomAttribute(uint id, object shape, Vector2f position);

        #endregion

        #region IInterface Members

        public Type TargetOperationType
        {
            get { return typeof(FillElementOperation); }
        }

        public abstract SharpMedia.Graphics.Shaders.ParameterDescription[] AdditionalParameters
        {
            get;
        }

        #endregion

        #region IEquatable<IFill> Members

        public abstract bool Equals(IFill other);

        #endregion
    }
}
