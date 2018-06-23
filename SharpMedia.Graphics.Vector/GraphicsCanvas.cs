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
using System.Threading;
using SharpMedia.Graphics.States;
using SharpMedia.Graphics.Shaders.Operations;
using SharpMedia.Math.Shapes;
using SharpMedia.Graphics.Vector.Fills;
using SharpMedia.Math.Shapes.Compounds;
using System.Runtime.InteropServices;
using SharpMedia.Math.Matrix;
using SharpMedia.Math.Shapes.Storage;
using SharpMedia.Math.Shapes.Storage.Builders;
using SharpMedia.Graphics.Vector.Transforms;
using SharpMedia.Graphics.Vector.Mappers;
using SharpMedia.Math.Shapes.Algorithms;

namespace SharpMedia.Graphics.Vector
{

    /// <summary>
    /// A graphics canvas info class.
    /// </summary>
    internal sealed class GraphicsCanvasInfo : ICanvasInfo
    {
        #region Private Members
        GraphicsCanvas root;
        #endregion

        #region Constructors

        public GraphicsCanvasInfo(GraphicsCanvas root)
        {
            this.root = root;
        }

        #endregion

        #region ICanvasInfo Members

        public Vector2i CanvasPixelSize
        {
            get { return root.viewport.Dimensions; }
        }

        public Vector2f CanvasUnitSize
        {
            get { return root.unitSize; }
        }

        public Vector2i ToPixelPosition(Vector2f canvasPosition)
        {
            Vector2f v = Vector2f.ComponentDivision(canvasPosition, root.unitSize);
            v = Vector2f.ComponentMultiply(v, new Vector2f(root.viewport.Dimensions.X, root.viewport.Dimensions.Y));
            return new Vector2i((int)(v.X + 0.5f), (int)(v.Y + 0.5f));
        }

        public Vector2f ToCanvasPosition(Vector2i pixelPosition)
        {
            Vector2f v = Vector2f.ComponentDivision(
                new Vector2f((float)pixelPosition.X, (float)pixelPosition.Y),
                new Vector2f((float)root.viewport.Width, (float)root.viewport.Height));
            return Vector2f.ComponentMultiply(v, root.unitSize);
        }

        public float ToCanvasSize(float pixelSize)
        {
            // We take Y into account for now.
            float v = (float)pixelSize / (float)root.viewport.Height;
            return v * root.unitSize.Y;
        }

        public float ToPixelSize(float canvasSize)
        {
            float v = canvasSize / root.unitSize.Y;
            return v * root.viewport.Dimensions.Y;
        }

        public float PixelErrorTolerance
        {
            get { return root.pixelErrorTolerance; }
        }

        public float TesselationResolution
        {
            get 
            {
                float res1 = root.unitSize.X * root.pixelErrorTolerance / (float)root.viewport.Width;
                float res2 = root.unitSize.Y * root.pixelErrorTolerance / (float)root.viewport.Height;

                return MathHelper.Min(res1, res2);
            }
        }

        #endregion
    }

    /// <summary>
    /// Graphics canvas implementation.
    /// </summary>
    public sealed class GraphicsCanvas : IDeviceCanvas, IDisposable
    {
        #region Private Structures

        /// <summary>
        /// Vertex transform.
        /// </summary>
        class DataTransform
        {
            /// <summary>
            /// Is it CPU or GPU processed.
            /// </summary>
            public bool ProcessCPU;

            /// <summary>
            /// The transform used.
            /// </summary>
            public ITransform Transform;

            public DataTransform(ITransform transform, bool cpu)
            {
                this.Transform = transform;
                this.ProcessCPU = cpu;
            }
        }

        /// <summary>
        /// A vertex data.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        struct VertexData
        {

            public Vector2f Position;
            public Vector2f TexCoord0;
            public Vector4f CustomAttribute0;
            public uint FillID;

            public VertexData(Vector2f position, Vector2f texCoord, Vector4f custom0,
                uint fillID)
            {
                Position = position;
                TexCoord0 = texCoord;
                CustomAttribute0 = custom0;
                FillID = fillID;
            }

            public static VertexFormat Format =
                VertexFormat.Parse("P.Fx2 T0.Fx2 U0.Fx4 U1.UI");
        }

        #endregion

        #region Private Members
        // Data needed by info.
        object syncRoot = new object();
        internal Vector2f unitSize;
        internal Region2i viewport;
        internal float pixelErrorTolerance = 1.0f;

        // The info seperate class.
        GraphicsCanvasInfo info;
        
        // Rendering state data.
        GraphicsDevice device;
        RenderTargetView renderTarget;
        ConstantBufferView vertexConstants;
        TypelessBuffer pixelConstants;
        Stack<Region2f[]> clippingRegions = new Stack<Region2f[]>();
        Stack<DataTransform> positionTransforms = new Stack<DataTransform>();
        Stack<DataTransform> textureTransforms = new Stack<DataTransform>();
        uint locks = 0;
        List<IFill> fills = new List<IFill>();
        GeometryBatch batch;
        #endregion

        #region Static Members

        /// <summary>
        /// We allocate 8 buffers per batch.
        /// </summary>
        public const uint CyclicBufferCount = 8;

        /// <summary>
        /// Maximum number of vertices per batch.
        /// </summary>
        /// <remarks>
        /// Each buffer can hold up to this amount of vertices.
        /// </remarks>
        public const ulong MaxVerticesInBatch = 5 * 1024;

        /// <summary>
        /// Maximum indices per batch.
        /// </summary>
        /// <remarks>We ensure enough indices.</remarks>
        public const ulong MaxIndicesInBatch = MaxVerticesInBatch * 3;

        // Static, reusable shader.
        ShaderCode vertexShaderCode;
        ShaderCode pixelShaderCode;
        DepthStencilState depthStencilState;
        BlendState blendState;
        RasterizationState rasterizationState;

        void Initialize()
        {
            // Initialize shader code.
            vertexShaderCode = new ShaderCode(BindingStage.VertexShader);

            {
                // We register inputs.
                vertexShaderCode.InputOperation.AddInput(PinComponent.Position, PinFormat.Floatx2);  //< Position.
                vertexShaderCode.InputOperation.AddInput(PinComponent.TexCoord0, PinFormat.Floatx2); //< Texture coordinate.
                vertexShaderCode.InputOperation.AddInput(PinComponent.User0, PinFormat.Floatx4);     //< Custom attribute 0.
                vertexShaderCode.InputOperation.AddInput(PinComponent.User1, PinFormat.UInteger);    //< Fill ID.


                // Position transform array (dynamically sized).
                ConstantOperation positionTransformOp = vertexShaderCode.CreateConstant("PositionTransform",
                    PinFormat.Float4x4, Pin.NotArray, null);

                // Texture transform array (dynamically sized).
                ConstantOperation textureTransformOp = vertexShaderCode.CreateConstant("TextureTransform",
                    PinFormat.Float4x4, Pin.NotArray, null);

                // We expand the position.
                ExpandOperation expandPos = new ExpandOperation(PinFormat.Floatx4, ExpandType.AddOnesAtW);
                expandPos.BindInputs(vertexShaderCode.InputOperation.PinAsOutput(PinComponent.Position));

                // We transform position by matrix.
                MultiplyOperation multiply = new MultiplyOperation();
                multiply.BindInputs(positionTransformOp.Outputs[0], expandPos.Outputs[0]);
                Pin position = multiply.Outputs[0];

                // We expand texture coordinate.
                ExpandOperation expandTex = new ExpandOperation(PinFormat.Floatx4, ExpandType.AddOnesAtW);
                expandTex.BindInputs(vertexShaderCode.InputOperation.PinAsOutput(PinComponent.TexCoord0));

                // We transform by matrix.
                MultiplyOperation multiply2 = new MultiplyOperation();
                multiply2.BindInputs(textureTransformOp.Outputs[0], expandTex.Outputs[0]);
                Pin texcoord = multiply2.Outputs[0];


                // We register outputs.
                vertexShaderCode.OutputOperation.AddComponentAndLink(PinComponent.Position, position);
                vertexShaderCode.OutputOperation.AddComponentAndLink(PinComponent.TexCoord0, texcoord);
                vertexShaderCode.OutputOperation.AddComponentAndLink(PinComponent.User0,
                    vertexShaderCode.InputOperation.PinAsOutput(PinComponent.User0));
                vertexShaderCode.OutputOperation.AddComponentAndLink(PinComponent.User1,
                    vertexShaderCode.InputOperation.PinAsOutput(PinComponent.User1));
            }
            vertexShaderCode.Immutable = true;

            pixelShaderCode = new ShaderCode(BindingStage.PixelShader);

            {
                // We register inputs.
                pixelShaderCode.InputOperation.AddInput(PinComponent.Position, PinFormat.Floatx4);  //< Position.
                pixelShaderCode.InputOperation.AddInput(PinComponent.TexCoord0, PinFormat.Floatx4); //< Texture coordinate.
                pixelShaderCode.InputOperation.AddInput(PinComponent.User0, PinFormat.Floatx4);     //< Custom attribute 0.
                pixelShaderCode.InputOperation.AddInput(PinComponent.User1, PinFormat.UInteger);     //< Fill ID.

                // We resgister interface constants.
                ConstantOperation interfaceConstant = pixelShaderCode.CreateConstant("Fills", PinFormat.Interface,
                    Pin.DynamicArray, null);

                // TODO: distance from border must be "evaluated".

                // We  convert position/tex coordinate.
                SwizzleOperation swizzlePos = new SwizzleOperation(SwizzleMask.XY);
                swizzlePos.BindInputs(pixelShaderCode.InputOperation.PinAsOutput(PinComponent.Position));
                Pin position = swizzlePos.Outputs[0];

                SwizzleOperation swizzleTex = new SwizzleOperation(SwizzleMask.XY);
                swizzleTex.BindInputs(pixelShaderCode.InputOperation.PinAsOutput(PinComponent.TexCoord0));
                Pin texcoord = swizzleTex.Outputs[0];

                // We now add the fill operation.
                FillElementOperation fillOperation = new FillElementOperation();
                fillOperation.BindInputs(position, texcoord, pixelShaderCode.InputOperation.PinAsOutput(PinComponent.User0),
                    pixelShaderCode.InputOperation.PinAsOutput(PinComponent.User1), interfaceConstant.Outputs[0]);

                // The output is colour.
                pixelShaderCode.OutputOperation.AddComponentAndLink(PinComponent.RenderTarget0, fillOperation.Outputs[0]);

            }
            pixelShaderCode.Immutable = true;

            // Depth-stencil state.
            depthStencilState = new DepthStencilState();
            depthStencilState.DepthTestEnabled = false;
            depthStencilState.DepthWriteEnabled = false;

            // Blend state
            blendState = new BlendState();
            blendState.AlphaBlendDestination = BlendOperand.One;
            blendState.AlphaBlendSource = BlendOperand.Zero;
            blendState.AlphaBlendOperation = BlendOperation.Add;

            blendState.BlendDestination = BlendOperand.SrcAlphaInverse;
            blendState.BlendSource = BlendOperand.SrcAlpha;
            blendState.BlendOperation = BlendOperation.Add;

            // We enable blending.
            blendState[0] = true;

            // Rasterization state.
            rasterizationState = new RasterizationState();
            rasterizationState.FrontFacing = Facing.CCW;
            rasterizationState.CullMode = CullMode.None;
            rasterizationState.FillMode = FillMode.Solid;
            rasterizationState.MultiSamplingEnabled = true; //< May change that in future.


            // We intern all states.
            depthStencilState = StateManager.Intern(depthStencilState);
            blendState = StateManager.Intern(blendState);
            rasterizationState = StateManager.Intern(rasterizationState);
        }
        #endregion

        #region Private Methods

        void AssertLocked()
        {
            if (locks <= 0)
            {
                throw new InvalidOperationException("The canvas is not locked.");
            }
        }

        unsafe void Flush(bool rebatch)
        {

            if (batch.VertexCount == 0)
            {
                if (!rebatch)
                {
                    batch.EndBatch();
                }

                return;
            }

            // We flush the buffer, first end batch.
            batch.EndBatch();


            // We update vertex constants.
            vertexConstants.Map(MapOptions.Write);

            try
            {
                DataTransform vtransform = positionTransforms.Peek();
                if (!vtransform.ProcessCPU)
                {
                    vertexConstants.SetConstant("PositionTransform", 
                        Matrix4x4f.CreateTranslate(new Vector3f(-unitSize.X, -unitSize.Y, 0)) *
                        Matrix4x4f.CreateScale(new Vector3f(2.0f, 2.0f, 2.0f)) *
                        vtransform.Transform.RuntimeForm);
                }
                else
                {
                    vertexConstants.SetConstant("PositionTransform", Matrix4x4f.Identity);
                }

                DataTransform ttransform = textureTransforms.Peek();
                if (!ttransform.ProcessCPU)
                {
                    vertexConstants.SetConstant("TextureTransform", ttransform.Transform.RuntimeForm);
                }
                else
                {
                    vertexConstants.SetConstant("TextureTransform", Matrix4x4f.Identity);
                }
            }
            finally
            {
                vertexConstants.UnMap();
            }

            // Vertex Shader:
            FixedShaderParameters vparams = vertexShaderCode.FixedParameters;
            vparams.AddLayout(0, vertexConstants.Layout);
            VShader vshader = vertexShaderCode.Compile(device, vparams) as VShader;

            // Pixel Shaders:
            FixedShaderParameters pparams = pixelShaderCode.FixedParameters;

            // We set interfaces.
            pparams.SetInterfaceArray("Fills", fills);

            // We now set per parameter data.
            ConstantBufferLayoutBuilder builder = new ConstantBufferLayoutBuilder();

            List<TextureView> textures = new List<TextureView>();
            List<SamplerState> samplers = new List<SamplerState>();

            // We add per-fill data.
            for (int i = 0; i < fills.Count; i++)
            {
                IFill fill = fills[i];
                string prefix = string.Format("Fills[{0}]", i);

                InterfaceHelper.ApplyInterfaceConstants(prefix, fill, builder, pparams, textures,
                    samplers, fill.ParameterValues);
            }

            // We create view and fill data.
            ConstantBufferLayout layout = builder.CreateLayout();
            ConstantBufferView pixelConstantsView = pixelConstants.CreateConstantBuffer(layout);
            pparams.AddLayout(0, layout);

            // TODO: this may not be needed for optimized setting.
            pixelConstantsView.Map(MapOptions.Write);
            try
            {

                for (int i = 0; i < fills.Count; i++)
                {
                    IFill fill = fills[i];
                    string prefix = string.Format("Fills[{0}]", i);

                    InterfaceHelper.FillInterfaceConstants(prefix, fill,
                        pixelConstantsView, fill.ParameterValues);
                }
            }
            finally
            {
                pixelConstantsView.UnMap();
            }

            // Finally compile.
            PShader pshader = pixelShaderCode.Compile(device, pparams) as PShader;


            
            using (DeviceLock devLock = device.Lock())
            {
                // We now draw using data, set all states.
                device.SetBlendState(blendState, Colour.Black, 0xFFFFFFFF);
                device.SetDepthStencilState(depthStencilState, 0);
                device.SetRasterizationState(rasterizationState);

                if (viewport.Width == viewport.Height && viewport.Height == 0)
                {
                    device.Viewport = new Region2i(new Vector2i(0, 0), new Vector2i((int)renderTarget.Width, (int)renderTarget.Height));
                }
                else
                {
                    device.Viewport = viewport;

                }
                // We bind stages.
                device.SetVertexShader(vshader, batch, null, null, new ConstantBufferView[] { vertexConstants });
                device.SetPixelShader(pshader, samplers.ToArray(), textures.ToArray(),
                    new ConstantBufferView[] { pixelConstantsView },
                    new RenderTargetView[] { renderTarget }, null);
                device.SetGeometryShader(null, null, null, null, null);


                // We now draw.
                device.DrawIndexed(0, batch.IndexCount, 0);

                // We clear state.
                device.SetVertexShader(null, null, null, null, null);
                device.SetPixelShader(null, null, null, null, null, null);

            }

            pixelConstantsView.Dispose();

            // Fills and data is irrelavant.
            fills.Clear();

            // We rebatch if not completelly ended.
            if (rebatch)
            {
                batch.BeginBatch();
            }

        }

        public void Dispose(bool fin)
        {
            lock (syncRoot)
            {
                batch.Dispose();
                pixelShaderCode.Dispose();
                vertexShaderCode.Dispose();

                if (!fin)
                {
                    GC.SuppressFinalize(this);
                }
            }
        }

        ~GraphicsCanvas()
        {
            Dispose(true);
        }

        #endregion

        #region Public Members

        public unsafe GraphicsCanvas([NotNull] GraphicsDevice device,
                              [NotNull] RenderTargetView renderTarget, 
                              Vector2f canvasUnits)
        {
            info = new GraphicsCanvasInfo(this);

            // Initialize shaders.
            Initialize();

            this.unitSize = canvasUnits;
            this.device = device;
            this.renderTarget = renderTarget;

            this.batch = Geometry.CreateBatch(VertexData.Format, new IndexFormat(true),
                MaxVerticesInBatch, MaxIndicesInBatch, CyclicBufferCount);

            // We also immediatelly bind it to device.
            this.batch.BindToDevice(device);

            // We create vertex constants buffer.
            {
                TypelessBuffer vertexConstBuffer = new TypelessBuffer(Usage.Dynamic, BufferUsage.ConstantBuffer,
                    CPUAccess.Write, GraphicsLocality.DeviceOrSystemMemory, 16 * 4 * 2);
                vertexConstBuffer.DisposeOnViewDispose = true;

                ConstantBufferLayoutBuilder vertexBufferLayout = new ConstantBufferLayoutBuilder();
                vertexBufferLayout.AppendElement("PositionTransform", PinFormat.Float4x4);
                vertexBufferLayout.AppendElement("TextureTransform", PinFormat.Float4x4);

                vertexConstants = vertexConstBuffer.CreateConstantBuffer(vertexBufferLayout.CreateLayout());
            }

            // We create pixel constants buffer.
            pixelConstants = new TypelessBuffer(Usage.Dynamic, BufferUsage.ConstantBuffer, CPUAccess.Write,
                GraphicsLocality.DeviceOrSystemMemory, ConstantBufferView.MaxSize);


        }

        #endregion

        #region IDeviceCanvas Members

        /// <summary>
        /// Gets or sets render target.
        /// </summary>
        public RenderTargetView Target
        {
            get
            {
                return renderTarget;
            }
            [param: NotNull]
            set
            {
                lock (syncRoot)
                {
                    if (locks > 0)
                    {
                        throw new InvalidOperationException("Cannot change render target in Begin/End pair.");
                    }
                    renderTarget = value;
                }
            }
        }

        public GraphicsDevice Device
        {
            get
            {
                return device;
            }
        }

        #endregion

        #region IDeviceCanvas Members

        public Region2i Viewport
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICanvas Members

        public ICanvasInfo CanvasInfo
        {
            get { return info; }
        }

        public float PixelErrorTolerance
        {
            get
            {
                return pixelErrorTolerance;
            }
            set
            {
                pixelErrorTolerance = value;
            }
        }

        public ITransform Transform
        {
            get
            {
                return positionTransforms.Peek().Transform;
            }
            set
            {
                AssertLocked();

                DataTransform transform = positionTransforms.Peek();

                // We check for no changes.
                if (transform == value)
                {
                    return;
                }

                if (!transform.ProcessCPU)
                {
                    // We flush all geometry.
                    Flush(true);
                }

                positionTransforms.Pop();
                if (value != null)
                {
                    positionTransforms.Push(new DataTransform(value, transform.ProcessCPU));
                }
                else
                {
                    positionTransforms.Push(new DataTransform(new LinearTransform(), transform.ProcessCPU));
                }
            }
        }

        public ITransform MappingTransform
        {
            get
            {
                return textureTransforms.Peek().Transform;
            }
            set
            {
                AssertLocked();

                DataTransform transform = textureTransforms.Peek();

                // We check for no changes.
                if (transform == value)
                {
                    return;
                }

                if (!transform.ProcessCPU)
                {
                    // We flush all geometry.
                    Flush(true);
                }

                textureTransforms.Pop();
                if (value != null)
                {
                    textureTransforms.Push(new DataTransform(value, transform.ProcessCPU));
                }
                else
                {
                    textureTransforms.Push(new DataTransform(new LinearTransform(), transform.ProcessCPU));
                }
            }
        }

        public Region2f[] ClippingRegions
        {
            get
            {
                return clippingRegions.Peek();
            }
            set
            {
                AssertLocked();

                clippingRegions.Pop();
                clippingRegions.Push(value);
            }
        }

        public void Begin(CanvasRenderFlags flags)
        {
            bool cpuTextureTrans = ((int)flags & (int)CanvasRenderFlags.SoftwareMappingTransform) != 0;
            bool cpuPosTrans = ((int)flags & (int)CanvasRenderFlags.SoftwarePositionTransform) != 0;

            if (locks == 0)
            {
                Monitor.Enter(syncRoot);

                positionTransforms.Push(new DataTransform(new LinearTransform(), cpuPosTrans));
                textureTransforms.Push(new DataTransform(new LinearTransform(), cpuTextureTrans));

                batch.BeginBatch();
            }
            else
            {
                Flush(true);

                positionTransforms.Push(new DataTransform(positionTransforms.Peek().Transform.Clone(), cpuPosTrans));
                textureTransforms.Push(new DataTransform(textureTransforms.Peek().Transform.Clone(), cpuTextureTrans));
            }
            locks++;
        }

        public void End()
        {
            locks--;
            if (locks < 0)
            {
                throw new InvalidOperationException("Too many End() calls (not synced with Begin() calls).");
            }

            // Must flush geometry.
            Flush(locks != 0);

            positionTransforms.Pop();
            textureTransforms.Pop();

            if (locks == 0)
            {
                Monitor.Exit(syncRoot);
            }
        }

        public void FillShape(IFill fill, IArea2f shape, IMapper mapper)
        {
            TriangleSoup2f soup = new TriangleSoup2f();
            shape.Tesselate(-info.TesselationResolution, soup);

            FillShape(fill, soup, mapper);
        }

        public void FillShape(IFill fill, TriangleSoup2f mesh, IMapper mapper)
        {
            if (mapper == null) mapper = new PositionMapper();

            // We first find "apropriate id" for instance independant fills
            uint fillID = uint.MaxValue;
            if (!fill.IsInstanceDependant)
            {
                for (int i = 0; i < fills.Count; i++)
                {
                    if (fills[i].GetType() == fill.GetType())
                    {
                        fillID = (uint)i;
                        break;
                    }
                }
            }
            else
            {
                int i;
                // We try to find exact (reference) match.
                for (i = 0; i < fills.Count; i++)
                {
                    if (fills[i] == fill)
                    {
                        fillID = (uint)i;
                        break;
                    }
                }

                // If we found it.
                if (i >= fills.Count)
                {
                    // We try to find compare match.
                    for (int j = 0; j < fills.Count; j++)
                    {
                        if (fills[j].Equals(fills))
                        {
                            fillID = (uint)j;
                            break;
                        }
                    }
                }
            }

            // We now insert fill if no appropriate was found.
            if (fillID == uint.MaxValue)
            {
                // We add it and save id.
                fillID = (uint)fills.Count;
                fills.Add(fill);
            }


            // We extract indices and vertices.
            List<Vector2f> vertices = mesh.Vertices;
            List<uint> indices = mesh.Indices;

            // We go for each triangle.
            int vertexCount = vertices.Count;
            VertexData[] triangleData = new VertexData[vertices.Count];

            

            // We prepare vertices.
            for (int i = 0; i < vertexCount; i++)
            {

                // We fill data.
                if (fill.CustomAttributeCount >= 1)
                {
                    triangleData[i].CustomAttribute0 = fill.CalcCustomAttribute(0, mesh, vertices[i]);

                    if (fill.CustomAttributeCount >= 2)
                    {

                        throw new NotSupportedException("Too many attributes.");
                    }
                }
                else
                {
                    triangleData[i].CustomAttribute0 = Vector3f.Zero;
                }


                // Fill ID.
                triangleData[i].FillID = fillID;

                // Positions.
                triangleData[i].Position = vertices[i];

            }

            // We generate them.
            Vector2f[] mappingCoord = mapper.Generate(mesh, vertices.ToArray());


            DataTransform ttransform = textureTransforms.Peek();

            // We do preprocessing.
            if (ttransform.Transform.NeedsPreprocess)
            {
                mappingCoord = (Vector2f[])ttransform.Transform.Preprocess(
                    PreprocessDataType.TextureCoordinates, mappingCoord);
            }

            if (ttransform.ProcessCPU)
            {
                Matrix4x4f matrix = ttransform.Transform.RuntimeForm;

                // We write them.
                for (int i = 0; i < vertexCount; i++)
                {
                    triangleData[i].TexCoord0 = (matrix * mappingCoord[i].Vec3).Vec2;
                }
            }
            else
            {

                // We write them.
                for (int i = 0; i < vertexCount; i++)
                {
                    triangleData[i].TexCoord0 = mappingCoord[i];
                }
            }

            // We may need to transform.
            DataTransform vtransform = positionTransforms.Peek();
            if (vtransform.ProcessCPU)
            {
                // We create matrix.
                Matrix4x4f matrix =
                        Matrix4x4f.CreateTranslate(new Vector3f(-unitSize.X, -unitSize.Y, 0)) *
                        Matrix4x4f.CreateScale(new Vector3f(2.0f, 2.0f, 2.0f)) *
                        vtransform.Transform.RuntimeForm;

                // We transform all points.
                for (int i = 0; i < triangleData.Length; i++)
                {
                    triangleData[i].Position = (matrix * new Vector4f(triangleData[i].Position.X,
                        triangleData[i].Position.Y, 0, 1)).Vec2;
                }

            }

            // Send data to batch.
            ulong r = batch.AddVertices(triangleData);

            // If we cannot draw it, we flush and try again.
            // FIXME: this is far from optimal.
            if (r != (ulong)triangleData.LongLength)
            {
                Flush(true);

                if (batch.AddVertices(triangleData) != (ulong)triangleData.LongLength)
                {
                    throw new NotSupportedException("Big vertex chunks not supported, consider creating bigger buffers.");
                }

                batch.AddVertices(triangleData);
            }

            // We now prepare indices.
            uint[] transIndices = new uint[indices.Count];
            uint offset = (uint)batch.VertexCount - (uint)triangleData.Length;
            int indexCount = indices.Count;
            for (int i = 0; i < indexCount; i++)
            {
                transIndices[i] = indices[i] + offset;
            }

            batch.AddIndices(transIndices);

        }

        public void DrawShape(Pen pen, IOutline2f outline, IMapper mapper)
        {
            /* TODO: need line soup implementation (probably line outline implementetation).
            // We create line soup and fill it with data.
            LineSoup2f lineSoup = new LineSoup2f();
            outline.Sample(-info.TesselationResolution, lineSoup);

            // Now we create outline.
            TriangleSoup2f triangleSoup = new TriangleSoup2f();
            OutlineTesselation.Tesselate(lineSoup.GetNonIndexedVertices(),
                pen.ToOutlineTesselationOptions(info), triangleSoup);

            FillShape(pen.Fill, triangleSoup, mapper);*/
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(false);
        }

        #endregion

    }
}
