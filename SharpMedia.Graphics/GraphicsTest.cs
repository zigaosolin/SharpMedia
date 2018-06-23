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
using SharpMedia.Testing;
using SharpMedia.Graphics.Shaders;
using SharpMedia.Graphics.Shaders.Operations;
using SharpMedia.Math;
using System.Threading;
using SharpMedia.Resources;
using SharpMedia.Graphics.States;
using SharpMedia.Graphics.Shaders.Metadata;

namespace SharpMedia.Graphics
{
    /// <summary>
    /// A graphics testing suite.
    /// </summary>
    public abstract class GraphicsTest
    {
        protected Window window;

        /// <summary>
        /// Factory that is used for all tests.
        /// </summary>
        public abstract GraphicsService Service
        {
            get;
        }


        /// <summary>
        /// Simple initializa
        /// </summary>
        public GraphicsDevice InitializeDevice()
        {
            GraphicsService init = Service;

            // Create device.
            RenderTargetParameters p = new RenderTargetParameters();
            p.BackBufferWidth = 1024;
            p.BackBufferHeight = 768;
            p.MultiSampleType = 1;
            p.MultiSampleQuality = 0;
            p.Format = PixelFormat.Parse("R.UN8 G.UN8 B.UN8 A.UN8");
            p.Windowed = true;

            return init.CreateDevice(false, false, p, out window);
        }

        /// <summary>
        /// Simple black screen initialization.
        /// </summary>
        //[CorrectnessTest]
        public void SimpleInit()
        {
            using (GraphicsDevice device = InitializeDevice())
            {
                bool isClosed = false;
                window.Closed += delegate(Window w)
                {
                    isClosed = true;
                };

                for (uint i = 0; i < 1000; i++)
                {
                    window.DoEvents();

                    if (!isClosed)
                    {
                        SwapChain chain = device.SwapChain;

                        device.Enter();
                        try
                        {
                            // We just clear.
                            device.Clear(chain, Colour.Black);
                            

                        }
                        finally
                        {
                            device.Exit();
                        }

                        chain.Present();
                        Thread.Sleep(10);
                    }
                    
                }

            }
        }

        /// <summary>
        /// Simple black screen initialization.
        /// </summary>
        //[CorrectnessTest]
        public void CustomRT()
        {
            using (GraphicsDevice device = InitializeDevice())
            {

                TypelessTexture2D texture = new TypelessTexture2D(Usage.Default, TextureUsage.RenderTarget, CPUAccess.None,
                    PixelFormat.Parse("R.UN8 G.UN8 B.UN8 A.UN8"), 512, 512, 1, GraphicsLocality.DeviceOrSystemMemory, null);

                RenderTargetView view = texture.CreateRenderTarget(0);

                bool isClosed = false;
                window.Closed += delegate(Window w)
                {
                    isClosed = true;
                };

                for (uint i = 0; i < 1000; i++)
                {
                    window.DoEvents();

                    if (!isClosed)
                    {
                        SwapChain chain = device.SwapChain;

                        device.Enter();
                        try
                        {
                            // We just clear.
                            device.Clear(chain, Colour.Blue);
                            device.Clear(view, Colour.Black);

                            

                        }
                        finally
                        {
                            device.Exit();
                        }

                        chain.Present();
                        Thread.Sleep(10);
                    }

                }

            }
        }

        [CorrectnessTest]
        public unsafe void TriangleTest()
        {
            using (GraphicsDevice device = InitializeDevice())
            {
                // We create shaders.
                VShader vshader;
                PShader pshader;
                using (ShaderCompiler compiler = device.CreateShaderCompiler())
                {

                    // Vertex shader (copy paste position).
                    compiler.Begin(BindingStage.VertexShader);
                    ShaderCompiler.Operand position = compiler.CreateInput(PinFormat.Floatx4, PinComponent.Position);
                    compiler.Output(position, PinComponent.Position);
                    vshader = compiler.End(null) as VShader;

                    // And pixel shader.
                    compiler.Begin(BindingStage.PixelShader);
                    ShaderCompiler.Operand colour = compiler.CreateFixed(PinFormat.Floatx4, Pin.NotArray, new Vector4f(1, 0, 0, 1));
                    compiler.Output(colour, PinComponent.RenderTarget0);
                    pshader = compiler.End(null) as PShader;

                }

                // We create triangles.
                Geometry geometry = new Geometry();
                TypelessBuffer buffer = new TypelessBuffer(Usage.Static, 
                    BufferUsage.VertexBuffer, CPUAccess.None, GraphicsLocality.DeviceOrSystemMemory, 4 * 4 * 3);

                byte[] d = buffer.Map(MapOptions.Read);
                fixed (byte* p = d)
                {
                    float* data = (float*)p;

                    // Vertex 0:
                    data[0] = -0.5f; data[1] = -0.5f; data[2] = 0.0f; data[3] = 1.0f;
                    
                    // Vertex 1:
                    data[4] = 0.5f; data[5] = -0.5f; data[6] = 0.0f; data[7] = 1.0f;

                    // Vertex 2:
                    data[8] = 0.0f; data[9] = 0.5f; data[10] = 0.0f; data[11] = 1.0f;
                }
                buffer.UnMap();
                

                // Create vertex buffer and view.
                VertexBufferView vbuffer = buffer.CreateVertexBuffer(VertexFormat.Parse("P.Fx4"));

                // We construct geometry.
                geometry[0] = vbuffer;
                geometry.Topology = Topology.Triangle;

                // Blend state.
                BlendState blendState = new BlendState();
                blendState[0] = false;

                blendState = StateManager.Intern(blendState);

                RasterizationState rastState = new RasterizationState();
                rastState.CullMode = CullMode.None;
                rastState.FillMode = FillMode.Solid;

                rastState = StateManager.Intern(rastState);

                DepthStencilState depthState = new DepthStencilState();
                depthState.DepthTestEnabled = false;
                depthState.DepthWriteEnabled = false;

                depthState = StateManager.Intern(depthState);

                // Enter rendering loop.
                bool isClosed = false;
                window.Closed += delegate(Window w)
                {
                    isClosed = true;
                };

                while (!isClosed )
                {
                    window.DoEvents();

                    if (!isClosed)
                    {
                        SwapChain chain = device.SwapChain;

                        device.Enter();
                        try
                        {
                            // We just clear.
                            device.Clear(chain, Colour.Black);

                            // Set blend/rast.
                            device.SetBlendState(blendState, Colour.White, 0xFFFFFFFF);
                            device.RasterizationState = rastState;
                            device.SetDepthStencilState(depthState, 0);

                            // Sets states.
                            device.SetVertexShader(vshader, geometry, null, null, null);
                            device.SetPixelShader(pshader, null, null, null, new RenderTargetView[] { chain }, null);

                            device.Viewport = new Region2i(0, 0, (int)chain.Width, (int)chain.Height);

                            // Render.
                            device.Draw(0, 3);
                        }
                        finally
                        {
                            device.Exit();
                        }

                        chain.Present();
                        Thread.Sleep(10);
                    }

                }


                // Dispose all.
                vshader.Dispose();
                pshader.Dispose();
                geometry.Dispose();
                vbuffer.Dispose();
                buffer.Dispose();

            }
        }

        [CorrectnessTest]
        public void FullscreenChange()
        {
            using (GraphicsDevice device = InitializeDevice())
            {
                bool isClosed = false;
                window.Closed += delegate(Window w)
                {
                    isClosed = true;
                };
                
                for (uint i = 0; i < 1000; i++)
                {
                    if (i == 1)
                    {
                        device.SwapChain.Reset(1280, 1024, null, true);
                    }

                    if (i == 20)
                    {
                        device.SwapChain.Reset(640, 480, null, false);
                    }

                    window.DoEvents();

                    if (!isClosed)
                    {
                        SwapChain chain = device.SwapChain;

                        device.Enter();
                        try
                        {
                            // We just clear.
                            device.Clear(chain, Colour.Black);


                        }
                        finally
                        {
                            device.Exit();
                        }

                        chain.Present();
                        Thread.Sleep(10);
                    }

                }

            }
        }

        [CorrectnessTest]
        public void TextureUsageCases()
        {
            TypelessTexture2D texture = new TypelessTexture2D(Usage.Default, TextureUsage.Texture | TextureUsage.RenderTarget,
                CPUAccess.None, PixelFormat.Parse("R.T8 G.T8 B.T8 A.T8"), 2, 2, 1, GraphicsLocality.DeviceOrSystemMemory, null);

            TextureView textureView = texture.CreateTexture(PixelFormat.Parse("R.UN8 G.UN8 B.UN8 A.UN8"));
            
            // Makes sure we know when it was disposed.
            texture.Disposed += delegate(IResource who)
            {
                TypelessTexture2D t = who as TypelessTexture2D;
                Console.Write("Texture disposed.");
            };

            // Make sure texture is disposed when all views are disposed (default is false).
            texture.DisposeOnViewDispose = true;

            // We fill data.
            using (Mipmap mipmap = texture.Map(MapOptions.Write, 0))
            {
                // We fill the mipmap.
                unsafe
                {
                    fixed (byte* b = mipmap.Data)
                    {
                        uint* data = (uint*)b;
                        data[0] = Colour.Green.PackedRGBA;
                        data[1] = Colour.Black.PackedRGBA;
                        data[2] = Colour.White.PackedRGBA;
                        data[3] = Colour.Green.PackedRGBA;
                    }
                }

            }

            // texture.UnLock(), Could also call mipmap.Dispose() but here not required since
            // using statement takes care of it.

            // Mipmap disposed here, cannot be used. At this point, texture
            // is still not bound to device, creation reuses the same buffer.
            GraphicsDevice device = InitializeDevice();
            try
            {
                device.Enter();
                textureView.BindToDevice(device);
                // Hardware support is now true.

                // We can render with it if we want ...



            }
            finally
            {
                // Makes sure we exit state.
                device.Exit();
            }


            // We dispose at the end, texture also disposed (no more views).
            textureView.Dispose();
            
        }

        [CorrectnessTest]
        public unsafe void DAGUsageCase2()
        {
            // We first initialize our shader.
            ShaderCode code = new ShaderCode(BindingStage.VertexShader);
            {
                // We write a simple Tranform code:
                code.InputOperation.AddInput(PinComponent.Position, PinFormat.Floatx3);
                Pin positon = code.InputOperation.PinAsOutput(PinComponent.Position);


                // We create 3 interations.
                LoopOperation loop = new LoopOperation();
                loop.InputOperation.BindInputs(code.CreateFixed((uint)3).Outputs[0], positon);

                AddOperation add = new AddOperation();
                add.BindInputs(loop.InputOperation.Outputs[1], code.CreateFixed(new Vector3f(1,0,0)).Outputs[0]);

                loop.OutputOperation.BindInputs(add.Outputs[0]);

                ExpandOperation expand = new ExpandOperation(PinFormat.Floatx4, ExpandType.AddOnesAtW);
                expand.BindInputs(loop.OutputOperation.Outputs[0]);

                // We just bind transformed position to output.
                code.OutputOperation.AddComponentAndLink(PinComponent.Position, expand.Outputs[0]);
            }

            // Immutate it.
            code.Immutable = true;

            // We now compile it.
            FixedShaderParameters p = code.FixedParameters;
            
            using(GraphicsDevice device = InitializeDevice())
            {

                code.Compile(device, p).Dispose();
            }

        }

        [CorrectnessTest]
        public unsafe void DAGUsageCases()
        {
            // We first initialize our shader.
            ShaderCode code = new ShaderCode(BindingStage.VertexShader);
            {
                // We write a simple Tranform code:
                code.InputOperation.AddInput(PinComponent.Position, PinFormat.Floatx3);
                Pin positon = code.InputOperation.PinAsOutput(PinComponent.Position);

                // We first need to expand our position to float4 (adding 1 at the end).
                ExpandOperation expand = new ExpandOperation(PinFormat.Floatx4, ExpandType.AddOnesAtW);
                expand.BindInputs(positon);
                Pin expPosition = expand.Outputs[0];

                // We now create constant transform matrix.
                ConstantOperation mvpConstant = code.CreateConstant("MVP", PinFormat.Float4x4);
                Pin MVP = mvpConstant.Outputs[0];

                // We multiply matrix and pin.
                MultiplyOperation mul = new MultiplyOperation();
                mul.BindInputs(MVP, expPosition);
                Pin transPosition = mul.Outputs[0];

                // We just bind transformed position to output.
                code.OutputOperation.AddComponentAndLink(PinComponent.Position, transPosition);
            }

            // Immutate it.
            code.Immutable = true;


            // We create constant buffer manually.
            ConstantBufferLayoutBuilder builder = new ConstantBufferLayoutBuilder();
            builder.AppendElement("MVP", PinFormat.Float4x4, Pin.NotArray);
            ConstantBufferLayout layout = builder.CreateLayout();

            // We now fill the data.
            FixedShaderParameters parameters = code.FixedParameters;
            parameters.AppendLayout(layout);
            if (!parameters.IsDefined)
            {
                throw new Exception();
            }


            

            using (GraphicsDevice device = InitializeDevice())
            {
                // We create shaders.

                // We have all parameters defined, compile the shader.
                VShader shader = code.Compile(device, parameters) as VShader;

                // Shader expects data in constant buffers.
                TypelessBuffer tbuffer = new TypelessBuffer(Usage.Dynamic, BufferUsage.ConstantBuffer, CPUAccess.Write, GraphicsLocality.DeviceOrSystemMemory, 4 * 4 * 4);
                ConstantBufferView constantBuffer = tbuffer.CreateConstantBuffer(layout);

                // We fill the buffer.
                constantBuffer.Map(MapOptions.Write);
                constantBuffer.SetConstant("MVP", new Math.Matrix.Matrix4x4f(1,0,0,0,
                                                                             0,1,0,0,
                                                                             0,0,1,0,
                                                                             -0.5f, -0.5f, 0, 1));
                constantBuffer.UnMap();

                PShader pshader;
                VShader vshader = shader;
                using (ShaderCompiler compiler = device.CreateShaderCompiler())
                {

                    // And pixel shader.
                    compiler.Begin(BindingStage.PixelShader);
                    ShaderCompiler.Operand colour = compiler.CreateFixed(PinFormat.Floatx4, Pin.NotArray, new Vector4f(1, 0, 0, 1));
                    compiler.Output(colour, PinComponent.RenderTarget0);
                    pshader = compiler.End(null) as PShader;

                }

                // We create triangles.
                Geometry geometry = new Geometry();
                TypelessBuffer buffer = new TypelessBuffer(Usage.Static,
                    BufferUsage.VertexBuffer, CPUAccess.None, GraphicsLocality.DeviceOrSystemMemory, 4 * 4 * 3);

                byte[] d = buffer.Map(MapOptions.Read);
                fixed (byte* p = d)
                {
                    float* data = (float*)p;

                    // Vertex 0:
                    data[0] = -0.5f; data[1] = -0.5f; data[2] = 0.0f; data[3] = 1.0f;

                    // Vertex 1:
                    data[4] = 0.5f; data[5] = -0.5f; data[6] = 0.0f; data[7] = 1.0f;

                    // Vertex 2:
                    data[8] = 0.0f; data[9] = 0.5f; data[10] = 0.0f; data[11] = 1.0f;
                }
                buffer.UnMap();


                // Create vertex buffer and view.
                VertexBufferView vbuffer = buffer.CreateVertexBuffer(VertexFormat.Parse("P.Fx4"));

                // We construct geometry.
                geometry[0] = vbuffer;
                geometry.Topology = Topology.Triangle;

                // Blend state.
                BlendState blendState = new BlendState();
                blendState[0] = false;

                blendState = StateManager.Intern(blendState);

                RasterizationState rastState = new RasterizationState();
                rastState.CullMode = CullMode.None;
                rastState.FillMode = FillMode.Solid;

                rastState = StateManager.Intern(rastState);

                DepthStencilState depthState = new DepthStencilState();
                depthState.DepthTestEnabled = false;
                depthState.DepthWriteEnabled = false;

                depthState = StateManager.Intern(depthState);

                // Enter rendering loop.
                bool isClosed = false;
                window.Closed += delegate(Window w)
                {
                    isClosed = true;
                };

                for (uint i = 0; i < 1000; i++)
                {
                    window.DoEvents();

                    if (!isClosed)
                    {
                        SwapChain chain = device.SwapChain;

                        device.Enter();
                        try
                        {
                            // We just clear.
                            device.Clear(chain, Colour.Black);

                            // Set blend/rast.
                            device.SetBlendState(blendState, Colour.White, 0xFFFFFFFF);
                            device.RasterizationState = rastState;
                            device.SetDepthStencilState(depthState, 0);

                            // Sets states.
                            device.SetVertexShader(vshader, geometry, null, null, new ConstantBufferView[] { constantBuffer });
                            device.SetPixelShader(pshader, null, null, null, new RenderTargetView[] { chain }, null);

                            device.Viewport = new Region2i(0, 0, (int)chain.Width, (int)chain.Height);

                            // Render.
                            device.Draw(0, 3);
                        }
                        finally
                        {
                            device.Exit();
                        }

                        chain.Present();
                        //Thread.Sleep(10);

                        Console.WriteLine(device.DevicePerformance.CurrentFPS);
                    }

                }


                // Dispose all.
                vshader.Dispose();
                pshader.Dispose();
                geometry.Dispose();
                vbuffer.Dispose();
                buffer.Dispose();

            }
        }



        [CorrectnessTest]
        public void DAGUsageCases2()
        {
            // We first initialize our shader.
            ShaderCode code = new ShaderCode(BindingStage.VertexShader);
            {
                // We write a simple Tranform code:
                code.InputOperation.AddInput(PinComponent.Position, PinFormat.Floatx3);
                Pin positon = code.InputOperation.PinAsOutput(PinComponent.Position);

                // We first need to expand our position to float4 (adding 1 at the end).
                ExpandOperation expand = new ExpandOperation(PinFormat.Floatx4, ExpandType.AddOnesAtW);
                expand.BindInputs(positon);
                Pin expPosition = expand.Outputs[0];

                // We now create constant transform matrix.
                ConstantOperation mvpConstant = code.CreateConstant("MVP", PinFormat.Float4x4);
                Pin MVP = mvpConstant.Outputs[0];

                // We multiply matrix and pin.
                MultiplyOperation mul = new MultiplyOperation();
                mul.BindInputs(MVP, expPosition);
                Pin transPosition = mul.Outputs[0];

                // We just bind transformed position to output.
                code.OutputOperation.AddComponentAndLink(PinComponent.Position, transPosition);
            }

            // We create constant buffer manually.
            ConstantBufferLayoutBuilder builder = new ConstantBufferLayoutBuilder();
            builder.AppendElement("MVP", PinFormat.Float4x4, Pin.NotArray);
            ConstantBufferLayout layout = builder.CreateLayout();

            // We now fill the data.
            FixedShaderParameters parameters = code.FixedParameters;
            parameters.AppendLayout(layout);
            if (!parameters.IsDefined)
            {
                throw new Exception();
            }

            GraphicsDevice device = InitializeDevice();

            // We have all parameters defined, compile the shader.
            VShader shader = code.Compile(device, parameters) as VShader;

            // Shader expects data in constant buffers.
            TypelessBuffer buffer = new TypelessBuffer(Usage.Default, BufferUsage.ConstantBuffer, CPUAccess.Write, GraphicsLocality.DeviceOrSystemMemory, 4 * 4 * 4);
            ConstantBufferView constantBuffer = buffer.CreateConstantBuffer(layout);

            // We fill the buffer.
            constantBuffer.Map(MapOptions.Write);
            constantBuffer.SetConstant("MVP", Math.Matrix.Matrix4x4f.Identity);
            constantBuffer.UnMap();


        }


        [CorrectnessTest]
        public void Batching()
        {
            

            using (GraphicsDevice device = InitializeDevice())
            {
                GeometryBatch geometry = Geometry.CreateBatch(VertexFormat.Parse("P.Fx3"),
                    new IndexFormat(false), 8, 24, 1);


                Vector3f[] vertices = new Vector3f[]
                {
                    new Vector3f(0, 0, 0),
                    new Vector3f(1, 0, 0),
                    new Vector3f(1, 1, 0)
                };

                geometry.BindToDevice(device);

                geometry.BeginBatch();
                geometry.AddVertices(vertices);
                geometry.EndBatch();


                geometry.Dispose();
            }
        }

        [CorrectnessTest]
        public void Metadata()
        {
            CodeGenerator codeGenerator = new CodeGenerator(BindingStage.VertexShader);

            
        }

        [CorrectnessTest]
        public void IndexBufferUsage()
        {

        }

        [CorrectnessTest]
        public void DirectConstantBuffer()
        {

        }
    }
}
