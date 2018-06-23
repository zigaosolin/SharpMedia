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
using SharpMedia.Graphics.Shaders;
using SharpMedia.Graphics.States;

namespace SharpMedia.Graphics.Vector.Fills
{
    /// <summary>
    /// A gradient UV fill.
    /// </summary>
    internal abstract class GradientUVFill : GradientFill
    {
        public override GradientType GradientType
        {
            get { return GradientType.UV; }
        }
    }

    /// <summary>
    /// Gradient fill with up to 4 weights/colours.
    /// </summary>
    internal class GradientUV4 : GradientUVFill
    {
        #region Implementation

        public override object[] ParameterValues
        {
            get 
            {
                object[] ret = new object[4 + 2 + 1];

                int i;
                for (i = 0; i < colours.Length; i++)
                {
                    ret[i] = colours[i].RGBA;
                }
                for (; i < 4; i++)
                {
                    ret[i] = Colour.Black;
                }

                // We add weights.
                ret[4] = positions.Length > 0 ? positions[0] : 1.0f;
                ret[5] = positions.Length > 1 ? positions[1] : 1.0f;
                ret[6] = scaling;

                return ret;
            
            }
        }

        public override void Compile(ShaderCompiler compiler, ShaderCompiler.Operand position, ShaderCompiler.Operand texCoord,
            ShaderCompiler.Operand[] attributes, ShaderCompiler.Operand borderDistance, ShaderCompiler.Operand[] constants, ShaderCompiler.Operand outputColour)
        {
            // We first bind to out names.
            ShaderCompiler.Operand colour0 = constants[0];
            ShaderCompiler.Operand colour1 = constants[1];
            ShaderCompiler.Operand colour2 = constants[2];
            ShaderCompiler.Operand colour3 = constants[3];
            ShaderCompiler.Operand position0 = constants[4];
            ShaderCompiler.Operand position1 = constants[5];
            ShaderCompiler.Operand scaling = constants[6];
            ShaderCompiler.Operand one = compiler.CreateFixed(PinFormat.Float, Pin.NotArray, 1.0f);

            // float t = texcoord.x + texcoord.y;
            // t = t - floor(t);
            ShaderCompiler.Operand scaled = compiler.Mul(scaling, compiler.Swizzle(texCoord, SwizzleMask.XY));
            ShaderCompiler.Operand t = compiler.Add(compiler.Swizzle(scaled, SwizzleMask.X), compiler.Swizzle(scaled, SwizzleMask.Y));
            t = compiler.Sub(t, compiler.Call(ShaderFunction.Floor, t)); 

            // if( t < position0) {
            //   float f = t/pos0;
            //   outColour = f*c1 + (1.0-f)*c0;
            // } else {
            compiler.BeginIf(compiler.Compare(CompareFunction.LessEqual, t, position0));
            ShaderCompiler.Operand f = compiler.Div(t, position0);
            compiler.Mov(compiler.Add(compiler.Mul(f, colour1), compiler.Mul(compiler.Sub(one, f), colour0)), outputColour);
            compiler.Else();
            // if(t < position1)
            //   float f = (t-pos0)/(pos1-pos0);
            //   outColour = f*c2 + (1.0-f)*c1;
            // } else {
            compiler.BeginIf(compiler.Compare(CompareFunction.LessEqual, t, position1));
            f = compiler.Div(compiler.Sub(t, position0), compiler.Sub(position1, position0));
            compiler.Mov(compiler.Add(compiler.Mul(f, colour2), compiler.Mul(compiler.Sub(one, f), colour1)), outputColour);

            // else {
            //   float f = (t-pos1)/(1-pos1);
            //   outColour = f*c3 + (1.0-f)*c2;
            compiler.Else();
            f = compiler.Div(compiler.Sub(t, position1), compiler.Sub(one, position1));
            compiler.Mov(compiler.Add(compiler.Mul(f, colour3), compiler.Mul(compiler.Sub(one, f), colour2)), outputColour);

            compiler.EndIf();
            compiler.EndIf();
            
            
        }

        public override bool IsInstanceDependant
        {
            get { return false; }
        }

        public override uint CustomAttributeCount
        {
            get { return 0; }
        }

        public override SharpMedia.Math.Vector4f CalcCustomAttribute(uint id, object shape, SharpMedia.Math.Vector2f position)
        {
            throw new InvalidOperationException("Id invalid.");
        }

        public override ParameterDescription[] AdditionalParameters
        {
            get
            {
                return new ParameterDescription[]
                {
                    new ParameterDescription("Colour0", new Pin(PinFormat.Floatx4, Pin.NotArray, null)),
                    new ParameterDescription("Colour1", new Pin(PinFormat.Floatx4, Pin.NotArray, null)),
                    new ParameterDescription("Colour2", new Pin(PinFormat.Floatx4, Pin.NotArray, null)),
                    new ParameterDescription("Colour3", new Pin(PinFormat.Floatx4, Pin.NotArray, null)),
                    new ParameterDescription("Position0", new Pin(PinFormat.Float, Pin.NotArray, null)),
                    new ParameterDescription("Position1", new Pin(PinFormat.Float, Pin.NotArray, null)),
                    new ParameterDescription("Scaling", new Pin(PinFormat.Floatx2, Pin.NotArray, null))
                };
            }
        }

        #endregion
    }
}
