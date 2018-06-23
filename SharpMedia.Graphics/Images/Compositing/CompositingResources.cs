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
using SharpMedia.Graphics.Shaders.Operations;
using SharpMedia.Graphics.States;
using SharpMedia.Math;

namespace SharpMedia.Graphics.Images.Compositing
{
    /// <summary>
    /// A hardware compositor.
    /// </summary>
    public sealed class CompositingResources : IDisposable
    {
        #region Private Members
        GraphicsDevice device;

        // Hardware resource data.
        ShaderCode vertexShader;
        ShaderCode pixelShader;
        RasterizationState rasterizationState;
        DepthStencilState depthStencilState;
        BlendState blendState;
        VertexFormat vertexFormat = VertexFormat.Parse("P.Fx2");
        Geometry alignedQuad;
        TypelessBuffer pixelTypelessConstantBuffer;
        #endregion

        #region Private Methods

        void Initialize()
        {
            // 1) We create vertex shader first.
            {
                vertexShader = new ShaderCode(BindingStage.VertexShader);
                vertexShader.InputOperation.AddInput(PinComponent.Position, PinFormat.Floatx2);

                

                // We now extend position
                ExpandOperation expandPositionOp = new ExpandOperation(PinFormat.Floatx4, ExpandType.AddOnesAtW);
                expandPositionOp.BindInputs(vertexShader.InputOperation.PinAsOutput(PinComponent.Position));
                Pin position = expandPositionOp.Outputs[0];

                // We now output position.
                vertexShader.OutputOperation.AddComponentAndLink(PinComponent.Position, position);
            }

            vertexShader.Immutable = true;

            // 2) We create pixel shader.
            {
                pixelShader = new ShaderCode(BindingStage.PixelShader);
                pixelShader.InputOperation.AddInput(PinComponent.Position, PinFormat.Floatx4);

                // We first use only XY.
                SwizzleOperation swizzleOp = new SwizzleOperation(SwizzleMask.XY);
                swizzleOp.BindInputs(pixelShader.InputOperation.PinAsOutput(PinComponent.Position));

                ConstantOperation constant = vertexShader.CreateConstant("Offset", PinFormat.Floatx2);

                // We first offset.
                SubstractOperation subOp = new SubstractOperation();
                subOp.BindInputs(swizzleOp.Outputs[0], constant.Outputs[0]);

                ConstantOperation interfaceOp = pixelShader.CreateConstant("Composite", PinFormat.Interface, Pin.DynamicArray);

                // We use the compositing operation.
                CompositingOperation op = new CompositingOperation();
                op.BindInputs(subOp.Outputs[0],
                    interfaceOp.Outputs[0]);

                // Compositing is bound to output.
                pixelShader.OutputOperation.AddComponentAndLink(PinComponent.RenderTarget0, op.Outputs[0]);
            }

            pixelShader.Immutable = true;

            // 3) Initialize states.

            // Depth-stencil state.
            depthStencilState = new DepthStencilState();
            depthStencilState.DepthTestEnabled = false;
            depthStencilState.DepthWriteEnabled = false;

            // Blend state (no blending default).
            blendState = new BlendState();


            // Rasterization state.
            rasterizationState = new RasterizationState();
            rasterizationState.FrontFacing = Facing.CCW;
            rasterizationState.CullMode = CullMode.None;
            rasterizationState.FillMode = FillMode.Solid;
            rasterizationState.MultiSamplingEnabled = true;


            // We intern all states.
            depthStencilState = StateManager.Intern(depthStencilState);
            blendState = StateManager.Intern(blendState);
            rasterizationState = StateManager.Intern(rasterizationState);

            // 4) We create geometry.
            alignedQuad = new Geometry(device);
            alignedQuad.AssociateBuffers = true;

            {
                // We create vertex buffer 
                VertexBufferView vertexBuffer = VertexBufferView.Create(device, vertexFormat, Usage.Default,
                    CPUAccess.None, GraphicsLocality.DeviceOrSystemMemory,
                    new Vector2f(-1.0f, -1.0f), new Vector2f(1.0f, -1.0f),
                    new Vector2f(1.0f, 1.0f), new Vector2f(-1.0f, -1.0f),
                    new Vector2f(1.0f, 1.0f), new Vector2f(-1.0f, 1.0f));

                alignedQuad[0] = vertexBuffer;
            }

            // 5) We create pixel typeless buffer for constant buffer.
            pixelTypelessConstantBuffer = new TypelessBuffer(Usage.Dynamic, BufferUsage.ConstantBuffer,
                CPUAccess.Write, GraphicsLocality.DeviceOrSystemMemory, ConstantBufferView.MaxSize);

        }

        void ListOperations(ICompositingOperation op, List<ICompositingOperation> list)
        {
            if (op.Source1 != null) ListOperations(op.Source1, list);
            if (op.Source2 != null) ListOperations(op.Source2, list);

            list.Add(op);
        }


        #endregion

        #region Public Members

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="provider"></param>
        internal CompositingResources(GraphicsDevice device)
        {
            this.device = device;

            Initialize();
        }

        /// <summary>
        /// Composites to source.
        /// </summary>
        /// <param name="compositeOperation"></param>
        public void CompositeToSource(ICompositingOperation compositeOperation, 
            BlendState blendOverwrite, Colour blendColour, Region2i viewport, RenderTargetView target)
        {
            // 1) We first prepare the providers.
            List<ICompositingOperation> relavantOperations = new List<ICompositingOperation>();
            ListOperations(compositeOperation, relavantOperations);

            // 2) We extract processors.
            List<ICompositeInterface> processors = new List<ICompositeInterface>(relavantOperations.Count);
            for (int i = 0; i < relavantOperations.Count; i++)
            {
                processors.Add(relavantOperations[i].Interface);
            }

            // 3) We prepare the shader.
            ShaderCode pshader = PixelShader;
            ShaderCode vshader = VertexShader;
            States.BlendState bstate = blendOverwrite != null ? blendOverwrite : DefaultBlendState;
            States.DepthStencilState dstate = DefaultDepthStencilState;
            States.RasterizationState rstate = DefaultRasterizationState;

            FixedShaderParameters pparams = pshader.FixedParameters;
            FixedShaderParameters vparams = vshader.FixedParameters;
            ConstantBufferLayoutBuilder builder = new ConstantBufferLayoutBuilder();
            List<TextureView> textures = new List<TextureView>();
            List<States.SamplerState> samplers = new List<States.SamplerState>();

            // We set interface array.
            pparams.SetInterfaceArray("Composite", processors);

            builder.AppendElement("Offset", PinFormat.Floatx2);

            // 4) We fill parameters and builder.
            for (int i = 0; i < processors.Count; i++)
            {
                string name = string.Format("Composite[{0}]", i);

                InterfaceHelper.ApplyInterfaceConstants(name, processors[i], builder, pparams, textures, samplers,
                    processors[i].ParameterValues);
            }

            // 5) We obtain layout big enough.
            ConstantBufferLayout layout = builder.CreateLayout();
            pparams.AddLayout(0, layout);

            ConstantBufferView constantBuffer = pixelTypelessConstantBuffer.CreateConstantBuffer(layout);
            // 6) We fill buffer.
            constantBuffer.Map(MapOptions.Write);
            try
            {
                constantBuffer.SetConstant("Offset", new Vector2f((float)viewport.X, (float)viewport.Y));

                for (int i = 0; i < processors.Count; i++)
                {
                    InterfaceHelper.FillInterfaceConstants(string.Format("Composite[{0}]", i), processors[i],
                        constantBuffer, processors[i].ParameterValues);
                }
            }
            finally
            {
                constantBuffer.UnMap();
            }

            

            // 7) We prepare geometry.
            

            // We get quad geometry
            Geometry geometry = alignedQuad;


            // ) We render the composition.
            GraphicsDevice device = Device;
            using (DeviceLock l = device.Lock())
            {
                // We set our state objects.
                device.SetBlendState(bstate, blendColour, 0xFFFFFFFF);
                device.SetDepthStencilState(dstate, 0);
                device.SetRasterizationState(rstate);
                device.Viewport = viewport;

                // We prepare to render.
                device.SetVertexShader(vshader.Compile(device, vparams) as VShader, geometry, null, null, null);
                device.SetGeometryShader(null, null, null, null, null);
                device.SetPixelShader(pshader.Compile(device, pparams) as PShader, samplers.ToArray(), textures.ToArray(), new ConstantBufferView[] { constantBuffer },
                    new RenderTargetView[] { target }, null);

                // We render it.
                if (geometry.IndexBuffer != null)
                {
                    device.DrawIndexed(0, 6, 0);
                }
                else
                {
                    device.Draw(0, 6);
                }

                device.SetPixelShader(null, null, null, null, null, null);
            }

            // We do not use constant buffer anymore.
            constantBuffer.Dispose();
            
        }

        #endregion

        #region Properties

        public GraphicsDevice Device
        {
            get { return device; }
        }

        public ShaderCode PixelShader
        {
            get { return pixelShader; }
        }

        public ShaderCode VertexShader
        {
            get { return vertexShader; }
        }

        public RasterizationState DefaultRasterizationState
        {
            get { return rasterizationState; }
        }

        public BlendState DefaultBlendState
        {
            get { return blendState; }
        }

        public DepthStencilState DefaultDepthStencilState
        {
            get { return depthStencilState; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (device == null) return;

            vertexShader.Dispose();
            pixelShader.Dispose();
            alignedQuad.Dispose();
            pixelTypelessConstantBuffer.Dispose();

            alignedQuad = null;
            vertexShader = null;
            pixelShader = null;

            device = null;
        }

        #endregion
    }
}
