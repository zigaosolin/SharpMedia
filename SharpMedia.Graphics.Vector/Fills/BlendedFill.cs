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
using SharpMedia.Graphics.Shaders;

namespace SharpMedia.Graphics.Vector.Fills
{
    /// <summary>
    /// A fill composite as blend.
    /// </summary>
    public class BlendedFill : IFill
    {
        #region Private Members
        IFill fill1;
        IFill fill2;
        float alpha;
        #endregion

        #region Public Members

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BlendedFill()
        {
        }

        /// <summary>
        /// Creates a blended fill.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="alpha"></param>
        public BlendedFill(IFill first, IFill second, float alpha)
        {
            this.fill1 = first;
            this.fill2 = second;
            this.Alpha = alpha;
        }

        /// <summary>
        /// The first fill.
        /// </summary>
        public IFill FirstFill
        {
            get
            {
                return fill1;
            }
            set
            {
                fill1 = value;
            }
        }

        /// <summary>
        /// The first fill.
        /// </summary>
        public IFill SecondFill
        {
            get
            {
                return fill2;
            }
            set
            {
                fill2 = value;
            }
        }

        /// <summary>
        /// The alpha used.
        /// </summary>
        public float Alpha
        {
            get
            {
                return alpha;
            }
            [param: MinFloat(0.0f), MaxFloat(1.0f)]
            set
            {

                alpha = value;
            }
        }

        #endregion

        #region Static Members

        /// <summary>
        /// Blends two fills.
        /// </summary>
        public static IFill BlendFills(IFill first, IFill second, float alpha)
        {
            // Some quick helpers.
            if (first == second) return first;
            if (first == null) return second;
            if (second == null) return first;
            if (MathHelper.NearEqual(alpha, 0.0f) || alpha < 0.0f) return first;
            if (MathHelper.NearEqual(alpha, 1.0f) || alpha > 1.0f) return second; 

            if (first is SolidFill && second is SolidFill)
            {
                return new SolidFill(new Colour((first as SolidFill).Colour.RGBA * (1.0f - alpha) +
                    (second as SolidFill).Colour.RGBA * alpha));
            }
            // TODO: other special case.
            else
            {
                return new BlendedFill(first, second, alpha);
            }
        }

        #endregion

        #region IFill Members

        public object[] ParameterValues
        {
            get 
            {
                object[] array1 = fill1.ParameterValues;
                object[] array2 = fill2.ParameterValues;

                object[] ret = new object[array1.Length + array2.Length + 1];
                int i = 0;
                for (int j = 0; j < array1.Length; j++)
                {
                    ret[i++] = array1[j];
                }
                for (int j = 0; j < array2.Length; j++)
                {
                    ret[i++] = array2[j];
                }

                ret[i] = alpha;

                // We return merged array.
                return ret;
            }
        }

        public void Compile(ShaderCompiler compiler, ShaderCompiler.Operand position, 
            ShaderCompiler.Operand texCoord, ShaderCompiler.Operand[] attributes, 
            ShaderCompiler.Operand borderDistance, ShaderCompiler.Operand[] constants, 
            ShaderCompiler.Operand outputColour)
        {
            // 1) We copy attributes.
            ShaderCompiler.Operand[] att1 = new ShaderCompiler.Operand[fill1.CustomAttributeCount];
            ShaderCompiler.Operand[] att2 = new ShaderCompiler.Operand[fill2.CustomAttributeCount];

            for (int i = 0; i < att1.Length; i++)
            {
                att1[i] = attributes[i];
            }

            for (int i = 0; i < att2.Length; i++)
            {
                att2[i] = attributes[i + att1.Length];
            }

            // 2) We copy constants.
            ShaderCompiler.Operand[] const1 = new ShaderCompiler.Operand[fill1.AdditionalParameters.Length];
            ShaderCompiler.Operand[] const2 = new ShaderCompiler.Operand[fill2.AdditionalParameters.Length];

            for (int i = 0; i < const1.Length; i++)
            {
                const1[i] = constants[i];
            }

            for (int i = 0; i < const2.Length; i++)
            {
                const2[i] = constants[i + const1.Length];
            }

            // 3) We compute partial colours.
            ShaderCompiler.Operand colour1 = compiler.CreateTemporary(PinFormat.Floatx4, Pin.NotArray);
            ShaderCompiler.Operand colour2 = compiler.CreateTemporary(PinFormat.Floatx4, Pin.NotArray);

            fill1.Compile(compiler, position, texCoord, att1, borderDistance, const1, colour1);
            fill2.Compile(compiler, position, texCoord, att2, borderDistance, const2, colour2);

            // 4) We combine them.
            compiler.Mov(compiler.Add(compiler.Mul(compiler.Sub(compiler.CreateFixed(PinFormat.Float,
                Pin.NotArray, 1.0f), constants[constants.Length - 1]), colour1),
                compiler.Mul(constants[constants.Length - 1], colour2)), outputColour);

        }

        public bool IsInstanceDependant
        {
            get { return false; }
        }

        public uint CustomAttributeCount
        {
            get { return fill1.CustomAttributeCount + fill2.CustomAttributeCount; }
        }

        public Vector4f CalcCustomAttribute(uint id, object shape, Vector2f position)
        {
            throw new Exception("The method or operation is not implemented.");
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
                // We merge them
                ParameterDescription[] desc1 = fill1.AdditionalParameters;
                ParameterDescription[] desc2 = fill2.AdditionalParameters;

                ParameterDescription[] ret = new ParameterDescription[desc1.Length + desc2.Length + 1];

                desc1.CopyTo(ret, 0);
                desc2.CopyTo(ret, desc1.Length);
                ret[ret.Length - 1] = new ParameterDescription("AlphaBlend", new Pin(PinFormat.Float, Pin.NotArray, null));

                return ret;
            
            }
        }

        #endregion

        #region IEquatable<IFill> Members

        public bool Equals(IFill other)
        {
            if(object.ReferenceEquals(this, other)) return true;

            if (other.GetType() == typeof(BlendedFill))
            {
                BlendedFill other2 = other as BlendedFill;
                if (other2.alpha == this.alpha && other2.fill1.Equals(fill1) 
                    && other2.fill2.Equals(fill1)) return true;
            }
            return false;
        }

        #endregion
    }
}
