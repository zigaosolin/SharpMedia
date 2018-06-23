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
using SharpMedia.Graphics.Images.Compositing;
using SharpMedia.Graphics.Images.Compositing.Operations;
using SharpMedia.Testing;
using SharpMedia.Components.Configuration;
using SharpMedia.Math;
using SharpMedia.Graphics.States;

namespace SharpMedia.Graphics
{
  

    /// <summary>
    /// A compositing framework allows hardware images composition.
    /// </summary>
    /// <remarks>It is not thread safe.</remarks>
    public sealed class Compositor : IDisposable
    {
        #region Private Members
        CompositingResources hardwareResources;

        // A stacked arhictecture.
        Stack<ICompositingOperation> results = new Stack<ICompositingOperation>();
        #endregion

        #region Properties


        /// <summary>
        /// Indicator whether the compositor framework is hardware accelerated.
        /// </summary>
        public bool IsHardware
        {
            get
            {
                return hardwareResources != null;
            }
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Creates a compositor.
        /// </summary>
        public Compositor()
        {
        }

        /// <summary>
        /// Creates a compositor with device.
        /// </summary>
        /// <param name="device"></param>
        public Compositor(GraphicsDevice device)
        {
            this.hardwareResources = new CompositingResources(device);
        }

        /// <summary>
        /// Flushes and clears results.
        /// </summary>
        public void Clear()
        {
            results.Clear();
        }

        /// <summary>
        /// Pushes the image on top of stack.
        /// </summary>
        /// <param name="image"></param>
        public void Push([NotNull] TextureView image)
        {
            if (image.Depth != 1 || image.FaceCount != 1)
            {
                throw new ArgumentException("Only 2D images supported.");
            }

            results.Push(new TextureSource(image));
        }

        /// <summary>
        /// Pushes a sampled texture on top of stack.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="state"></param>
        public void Push([NotNull] TextureView image, SamplerState state)
        {
            if (image.Depth != 1 || image.FaceCount != 1)
            {
                throw new ArgumentException("Only 2D images supported.");
            }

            results.Push(new SampledTextureSource(image, state));
        }

        /// <summary>
        /// Pushes the colour acting as a constant source.
        /// </summary>
        /// <param name="colour"></param>
        public void Push([NotNull] Colour colour)
        {
            results.Push(new ColourSource(colour));
        }

        /// <summary>
        /// Adds two images together (sums them).
        /// </summary>
        public void Add()
        {
            Call(new AddSources());
        }

        /// <summary>
        /// Blends image with other image.
        /// </summary>
        public void Blend(Vector4f inFactor, Vector4f outFactor)
        {
            Call(new BlendSources(inFactor, outFactor));
        }

        /// <summary>
        /// Blends image with other image.
        /// </summary>
        public void BlendTo(States.BlendOperation operation,
            States.BlendOperand source, States.BlendOperand dest, 
            float factor, RenderTargetView target)
        {
            BlendTo(0, 0, operation, source, dest, factor, target);
        }

        /// <summary>
        /// Blends image with other image at specified positions.
        /// </summary>
        public void BlendTo(uint x, uint y,
            States.BlendOperation operation, States.BlendOperand source,
            States.BlendOperand dest, float factor, RenderTargetView target)
        {
            BlendState state = new BlendState();
            state.BlendOperation = operation;
            state.BlendSource = source;
            state.BlendDestination = dest;


            BlendTo(x, y, state, new Colour(factor, factor, factor, 1), target);
            
        }

        /// <summary>
        /// Blends image with blend state.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="state"></param>
        /// <param name="colour"></param>
        /// <param name="view"></param>
        public void BlendTo(uint x, uint y, BlendState state,
            Colour colour, RenderTargetView view)
        {
            CallTerminal(new BlendToTarget(new Vector2i((int)x, (int)y), state, colour), view);
        }

        /// <summary>
        /// Resizes the topmost image.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="depth"></param>
        public void Resize(uint width, uint height)
        {
            Resize(BuildImageFilter.Nearest, width, height);
        }

        /// <summary>
        /// Resizes the topmost image.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="depth"></param>
        public void Resize(BuildImageFilter filter, uint width, uint height)
        {

        }

        /// <summary>
        /// Moves (copies) topmost image to image under it.
        /// </summary>
        public void CopyTo(RenderTargetView target)
        {
            CopyTo(0, 0, target);
        }

        /// <summary>
        /// Moves (copies) topmost image to image under it to desired location.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void CopyTo(uint x, uint y, RenderTargetView target)
        {
            CallTerminal(new CopyToTarget(new Vector2i((int)x, (int)y)), target);
        }


        /// <summary>
        /// Applies effect to topmost image.
        /// </summary>
        /// <param name="effectType"></param>
        public void Call(Type operationType)
        {
            if (!Common.IsTypeSameOrDerived(operationType, typeof(ICompositingOperation)))
            {
                throw new ArgumentException("The type '{0}' is not a compositing effect", operationType.FullName);
            }

            Call((ICompositingOperation)Activator.CreateInstance(operationType));
        }

        /// <summary>
        /// Applies effect to topmost image and writes result to the top.
        /// </summary>
        /// <param name="effect"></param>
        public void Call(ICompositingOperation operation)
        {
            switch (operation.OperationType)
            {
                case OperationType.NoSource:
                    break;
                case OperationType.OneSource:
                    operation.Source1 = results.Pop();
                    break;
                case OperationType.TwoSources:
                    operation.Source1 = results.Pop();
                    operation.Source2 = results.Pop();
                    break;
            }

            results.Push(operation);   
        }

        /// <summary>
        /// Calls a terminal operation.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="target"></param>
        public void CallTerminal(ITerminalCompositingOperation operation, RenderTargetView target)
        {
            Call(operation);

            (results.Pop() as ITerminalCompositingOperation).CompositeTo(hardwareResources, target);
        }
        


        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (hardwareResources != null)
            {
                hardwareResources.Dispose();
                hardwareResources = null;
            }
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE

    [TestSuite]
    internal class CompositorTest : IDisposable
    {
        GraphicsDevice device;

        // The sample image.
        TextureView sampleImage;
        RenderTargetView renderTarget;

        [Required]
        public GraphicsDevice Device 
        {
            get { return device; }
            set { device = value; }
        }

        public CompositorTest()
        {
            byte[] data = new byte[]
            {
                // Red.
                255, 0, 0, 255,

                // Green.
                0, 255, 0, 255,

                // Blue.
                0, 0, 255, 255,

                // White
                255, 255, 255, 255
            };

            TypelessTexture2D t = new TypelessTexture2D(Usage.Default, TextureUsage.Texture, CPUAccess.Read,
                            PixelFormat.Parse("R.UN8 R.UN8 G.UN8 A.UN8"), 2, 2, 1, 
                            GraphicsLocality.DeviceOrSystemMemory, new byte[][] { data });

            t.DisposeOnViewDispose = true;
            sampleImage = t.CreateTexture();


            t = new TypelessTexture2D(Usage.Default, TextureUsage.Texture | TextureUsage.RenderTarget,
                CPUAccess.Read, PixelFormat.Parse("R.UN8 R.UN8 G.UN8 A.UN8"), 4, 4, 1, 
                GraphicsLocality.DeviceOrSystemMemory, null);

            renderTarget = t.CreateRenderTarget();
        }

        [CorrectnessTest]
        public unsafe void HardwareComposition1()
        {
            Compositor compositor = new Compositor(device);

            // We composite image.
            compositor.Push(sampleImage);
            compositor.Resize(BuildImageFilter.Nearest, 4, 4);
            compositor.CopyTo(renderTarget);

            Assert.AreEqual(renderTarget.Width, 4);
            Assert.AreEqual(renderTarget.Height, 4);

            // We check for correctness
            using (Mipmap mipmap = renderTarget.Map(MapOptions.Read, 0))
            {
                // We examine the contents.
                fixed (byte* pp = mipmap.Data)
                {
                    uint* chunk = (uint*)pp;

                    for (int x = 0; x < 4; x++)
                    {
                        for (int y = 0; y < 4; y++)
                        {
                            if (x < 2 && y < 2) Assert.AreEqual(chunk[y * 4 + x], 0xFF0000FF);
                            if (x > 2 && y < 2) Assert.AreEqual(chunk[y * 4 + x], 0x00FF00FF);
                            if (x < 2 && y > 2) Assert.AreEqual(chunk[y * 4 + x], 0x0000FFFF);
                            else                Assert.AreEqual(chunk[y * 4 + x], 0xFFFFFFFF);
                        }
                    }
                }
            }
        }


        #region IDisposable Members

        public void Dispose()
        {
            renderTarget.Dispose();
            sampleImage.Dispose();
        }

        #endregion
    }

#endif
}
