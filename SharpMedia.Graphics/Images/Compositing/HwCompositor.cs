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
    public sealed class HwCompositor : IDisposable
    {
        #region Private Members
        GraphicsDevice device;
        IHwResourceProvider provider;

        // Hardware resource data.
        ShaderCode vertexShader;
        ShaderCode pixelShader;
        RasterizationState rasterizationState;
        DepthStencilState depthStencilState;
        BlendState blendState;

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
                pixelShader.InputOperation.AddInput(PinComponent.Position, PinFormat.Floatx2);

                ConstantOperation interfaceOp = pixelShader.CreateConstant("Composite", PinFormat.Interface, Pin.DynamicArray);

                // We use the compositing operation.
                CompositingOperation op = new CompositingOperation();
                op.BindInputs(pixelShader.InputOperation.PinAsOutput(PinComponent.Position), 
                    interfaceOp.Outputs[0]);

                // Compositing is bound to output.
                pixelShader.OutputOperation.AddComponentAndLink(PinComponent.Colour, op.Outputs[0]);
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

        }


        #endregion

        #region Public Members

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="provider"></param>
        internal HwCompositor(GraphicsDevice device, IResourceProvider provider)
        {
            this.device = device;
            this.provider = provider as IHwResourceProvider;

            Initialize();
        }

        /// <summary>
        /// Composites to source 2.
        /// </summary>
        /// <param name="compositeOperation"></param>
        public void CompositeToSource2(IBinaryOperation compositeOperation, BlendState blendOverwrite,
            Vector2f minCoord, Vector2f maxCoord)
        {
            // 1) We first prepare the providers.
            List<IOperation> relavantOperations = CompositionHelper.RelavantOperations(compositeOperation);

            // 2) We extract processors.
            List<ICompositeInterface> processors = new List<ICompositeInterface>(relavantOperations.Count + 1);
            for (int i = 0; i < relavantOperations.Count; i++)
            {
                processors.Add(relavantOperations[i].PixelProcessor);
            }

            processors.Add(compositeOperation.PixelProcessor);

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

            ConstantBufferView constantBuffer =
                ResourceProvider.Provide(layout.MinimumBufferSizeInBytes).CreateConstantBuffer(layout);

            // 6) We fill buffer.
            constantBuffer.Map(MapOptions.Write);
            try
            {
                for (int i = 0; i < processors.Count; i++)
                {
                    InterfaceHelper.FillInterfaceConstants(string.Format("Composite[{i}]", i), processors[i],
                        constantBuffer, processors[i].ParameterValues);
                }
            }
            finally
            {
                constantBuffer.UnMap();
            }

            

            // 7) We prepare geometry.
            

            // We create quad geometry
            Geometry geometry = CreateQuadGeometry(minCoord, maxCoord);


            // ) We render the composition.
            GraphicsDevice device = Device;
            using (DeviceLock l = device.Lock())
            {
                // We set our state objects.
                device.SetBlendState(bstate, Colour.White, 0xFFFFFFFF);
                device.SetDepthStencilState(dstate, 0);
                device.SetRasterizationState(rstate);

                // We prepare to render.
                device.SetVertexShader(vshader.Compile(device, vparams) as VShader, geometry, null, null, null);
                device.SetGeometryShader(null, null, null, null, null);
                device.SetPixelShader(pshader.Compile(device, pparams) as PShader, samplers.ToArray(), textures.ToArray(), new ConstantBufferView[] { constantBuffer },
                    new RenderTargetView[] { compositeOperation.Source2.DestinationView }, null);

                // We render it.
                if (geometry.IndexBuffer != null)
                {
                    device.DrawIndexed(0, 6, 0);
                }
                else
                {
                    device.Draw(0, 6);
                }
            }

            // We do not use constant buffer anymore.
            ResourceProvider.Unused(constantBuffer.TypelessResource as TypelessBuffer);
        }

        #endregion

        #region Properties

        public GraphicsDevice Device
        {
            get { return device; }
        }

        /// <summary>
        /// A quad geometry (possibly cached). It is owned by resources.
        /// </summary>
        public Geometry CreateQuadGeometry(Vector2f min, Vector2f max)
        {
            return null;
        }

        public IHwResourceProvider ResourceProvider
        {
            get { return provider; }
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

            vertexShader = null;
            pixelShader = null;
        }

        #endregion
    }
}
