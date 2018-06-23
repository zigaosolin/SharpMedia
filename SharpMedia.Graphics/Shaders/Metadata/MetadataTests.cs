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

#if SHARPMEDIA_TESTSUITE
    public class MetadataTests
    {

        public ShaderCode Swizzle()
        {
            CodeGenerator generator = new CodeGenerator(BindingStage.VertexShader);

            Floatx3 p = generator.InputFloatx3(PinComponent.Position);
            Float4x4 m = generator.CreateFloat4x4("Matrix");
            p.XYZ = (new Floatx4(p, 1.0f) * m).XYZ;

            return generator.ShaderCode;

        }

        public ShaderCode ManualCode()
        {

            CodeGenerator generator = new CodeGenerator(BindingStage.VertexShader);

            Floatx4 trans = generator.Execute<MultiplyOperation>(generator.InputFloatx4(PinComponent.Position),
                                        generator.CreateFloat4x4("Matrix"))[0] as Floatx4;

            return generator.ShaderCode;
        }

        public ShaderCode Loops()
        {
            CodeGenerator generator = new CodeGenerator(BindingStage.VertexShader);

            UIntegerx1 n = generator.Fixed((uint)10);
            UIntegerx1 c = generator.Fixed((uint)1);

            // We begin loop.
            Loop<UIntegerx1> loop = generator.BeginLoop(n, c);
            {
                // Loop data must be used.
                loop.Value += loop.IterationIndex;
            }
            // End loop.
            generator.EndLoop(loop);

            // We can move the reference back.
            c = loop.Value;

            generator.Output(PinComponent.Position, c);

            return generator.ShaderCode;
        }

        public ShaderCode SamplingLoading()
        {
            CodeGenerator generator = new CodeGenerator(BindingStage.VertexShader);
            Texture2DBinder<Floatx3> texture = generator.CreateTexture2D<Floatx3>("Texture");
            SamplerBinder sampler = generator.CreateSampler("Sampler");
            Floatx3 sampled = texture.Sample(sampler, generator.InputFloatx2(PinComponent.TexCoord0));
            return generator.ShaderCode;
        }


        public ShaderCode Arrays()
        {
            CodeGenerator generator = new CodeGenerator(BindingStage.VertexShader);
            UIntegerx1 i = generator.InputUInteger1(PinComponent.TexCoord0);
            PinArray<Floatx1> arr = generator.CreateFloatx1Array("MyArray", 10);
            arr[i] = arr[(int)2] * 2.0f + arr[(uint)0] * -4.0f;

            return generator.ShaderCode;
        }

        public ShaderCode Matrix()
        {
            CodeGenerator generator = new CodeGenerator(BindingStage.VertexShader);
            Floatx4 position = generator.InputFloatx4(PinComponent.Position);
            Float4x4 matrix = generator.CreateFloat4x4("GlobalTransform");

            position = position * matrix;

            return generator.ShaderCode;
        }

        public ShaderCode Simulate()
        {
            CodeGenerator generator = new CodeGenerator(BindingStage.PixelShader);
            Floatx1 dt = generator.ConstantFloatx1("DeltaTime");
            Floatx1 mass = generator.ConstantFloatx1("Mass");

            // We extract data.
            Floatx2 position = generator.InputFloatx2(PinComponent.Position);
            Floatx2 velocity = generator.InputFloatx2(PinComponent.TexCoord0);
            Floatx2 force = generator.InputFloatx2(PinComponent.TexCoord1);

            // We update force.
            velocity += force * mass * dt;
            position += velocity * dt;

            // We now update data.
            generator.Output(PinComponent.Position, position);
            generator.Output(PinComponent.TexCoord0, velocity);

            return generator.ShaderCode;
        }

        public ShaderCode Conditional()
        {
            CodeGenerator generator = new CodeGenerator(BindingStage.VertexShader);
            Floatx1 f = generator.InputFloatx1(PinComponent.Position);

            f = generator.Branch((f < 2.0f) & (f > 0.0f), f, generator.Fixed(2.0f));


            return generator.ShaderCode;
            
        }
    }
#endif
}
