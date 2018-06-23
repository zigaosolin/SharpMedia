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
using SharpMedia.Graphics.Vector.Fills;
using SharpMedia.Math.Shapes;
using SharpMedia.Math;
using SharpMedia.Math.Shapes.Compounds;
using SharpMedia.Graphics.Vector.Fonts;
using SharpMedia.Graphics.Vector.Fonts.SVG;
using SharpMedia.Math.Shapes.Storage;
using SharpMedia.Graphics.Vector.Transforms;
using SharpMedia.Math.Shapes.Algorithms;

namespace SharpMedia.Graphics.Vector
{
#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    public class VectorTest
    {

        TextureView CreateSampleTexture()
        {
            // Non-mipmapped.
            TypelessTexture2D texture = new TypelessTexture2D(Usage.Default, 
                TextureUsage.Texture, CPUAccess.None, PixelFormat.Parse("R.UN8 G.UN8 B.UN8 A.UN8"),
                512, 512, 1, GraphicsLocality.DeviceOrSystemMemory, null);

            // We create data.
            byte[] data = texture.Map(MapOptions.Write, 0).Data;
            for (int x = 0; x < 512; x++)
            {
                for (int y = 0; y < 512; y++)
                {
                    int idx = (y * 512 + x) * 4;
                    data[idx] = (byte)((x * 256) / 512);
                    data[idx + 1] = (byte)((y * 256) / 512);
                    data[idx + 2] = (byte)(((x + y) * 256) / 1024);
                    data[idx + 3] = 100;  // Semi-Transparent

                }
            }
            texture.UnMap(0);

            texture.DisposeOnViewDispose = true;

            return texture.CreateTexture();


        }

        [CorrectnessTest]
        public void SimpleVG(GraphicsDevice device)
        {
            ICanvas canvas = new GraphicsCanvas(device, device.SwapChain, new Vector2f(1.0f, 1.0f));
            
            // We first create all needed fills.
            SolidFill solidFill = new SolidFill();
            solidFill.Colour = Colour.LightBlue;


            GradientFill graFill = GradientFill.CreateGradient(
                new Colour[] { new Colour(13, 185, 242, 255), new Colour(191, 234, 21, 255), 
                    new Colour(112, 62, 193, 255), new Colour(255, 242, 81, 255) },
                new float[] { 0.30f, 0.60f }, new Vector2f(1,1));


            TextureFill texFill = new TextureFill(CreateSampleTexture());

            RadialFill radFill = new RadialFill(new Vector2f(0.75f, 0.75f), 0.15f, Colour.White, Colour.Black);

            Quad2f rect = new Quad2f(Vector2f.Zero, new Vector2f(0.5f, 0), new Vector2f(0.5f, 0.5f), new Vector2f(0.0f, 0.5f));
            Quad2f rect2 = new Quad2f(new Vector2f(0.5f, 0.5f), new Vector2f(1, 0.5f), new Vector2f(1, 1), new Vector2f(0.5f, 1.0f));


            bool exit = false;

            device.SwapChain.Window.Closed += delegate(Window w) { exit = true; };

            float a = 0;

            while (!exit)
            {
                device.SwapChain.Window.DoEvents();

                using (DeviceLock l = device.Lock())
                {
                    device.Clear(device.SwapChain, Colour.Green);

                    device.SetViewports(new Region2i(0, 0, (int)device.SwapChain.Width, (int)device.SwapChain.Height));

                    // We render.
                    canvas.Begin(CanvasRenderFlags.None);

                    
                        canvas.FillShape(solidFill, rect, null);
                        canvas.FillShape(radFill, rect2, null);
                        canvas.Transform = new LinearTransform(
                            new Math.Matrix.Matrix4x4f(Math.MathHelper.Cos(a), -Math.MathHelper.Sin(a), 0, 0,
                                                       Math.MathHelper.Sin(a), Math.MathHelper.Cos(a), 0, 0,
                                                       0, 0, 1, 0,
                                                       0, 0, 0, 1));
                        canvas.FillShape(graFill, rect, null);
                        canvas.FillShape(texFill, rect2, null);
                    


                    canvas.End();
                }
                

                Console.WriteLine("FPS: {0}\nTrig Count: {1}", device.DevicePerformance.CurrentFPS,
                    device.DevicePerformance.MaxTrianglesPerFrame);

                a += 0.003f;

                device.SwapChain.Present();

            }

            texFill.Texture.Dispose();
        }

        [CorrectnessTest]
        public void PenVG(GraphicsDevice device)
        {
            ICanvas canvas = new GraphicsCanvas(device, device.SwapChain, new Vector2f(1.0f, 1.0f));

            // We first create all needed fills.
            SolidFill solidFill = new SolidFill(Colour.Red);
            Pen pen = new Pen(solidFill, 0.003f, 0.0f, OutlineEnd.Square);
            Bezier2f line = new Bezier2f(new Vector2f(0.1f, 0.5f), new Vector2f(0.3f, 1.0f),
                new Vector2f(0.9f ,0.5f));

            Bezier2f line2 = new Bezier2f(new Vector2f(0.1f, 0.5f), new Vector2f(0.3f, 0.0f),
                new Vector2f(0.9f, 0.5f));

            LineSegment2f seg = new LineSegment2f(new Vector2f(0, 0.5f), new Vector2f(0.7f, 0.6f));

            bool exit = false;

            device.SwapChain.Window.Closed += delegate(Window w) { exit = true; };

            float a = 0;

            while (!exit)
            {
                device.SwapChain.Window.DoEvents();

                using (DeviceLock l = device.Lock())
                {
                    device.Clear(device.SwapChain, Colour.Green);

                    device.SetViewports(new Region2i(0, 0, (int)device.SwapChain.Width, (int)device.SwapChain.Height));

                    line.A = 0.5f * new Vector2f(1,1) + new Vector2f(1,1) * 0.5f * MathHelper.Cos(a);
                    line2.B = 0.5f * new Vector2f(1, 1) + new Vector2f(1,1) * 0.5f * MathHelper.Sin(a);

                    // We render.
                    canvas.Begin(CanvasRenderFlags.None);

                    canvas.DrawShape(pen, line, null);
                    canvas.DrawShape(pen, seg, null);
                    canvas.DrawShape(pen, line2, null);

                    canvas.End();
                }

                device.SwapChain.Present();

                a += 0.01f;

                
            }

        }

        [CorrectnessTest]
        public void PenVG2(GraphicsDevice device)
        {
            ICanvas canvas = new GraphicsCanvas(device, device.SwapChain, new Vector2f(1.0f, 1.0f));

            // We first create all needed fills.
            SolidFill solidFill = new SolidFill(Colour.Red);
            Pen pen = new Pen(solidFill, 0.03f, 0.0f, OutlineEnd.Square);


            

            bool exit = false;

            device.SwapChain.Window.Closed += delegate(Window w) { exit = true; };

            float a = 0;

            while (!exit)
            {
                device.SwapChain.Window.DoEvents();

                using (DeviceLock l = device.Lock())
                {
                    device.Clear(device.SwapChain, Colour.Green);

                    device.SetViewports(new Region2i(0, 0, (int)device.SwapChain.Width, (int)device.SwapChain.Height));

  
                    // We render.
                    canvas.Begin(CanvasRenderFlags.None);

                    OutlineCompound2f b = new OutlineCompound2f(
                        new LineSegment2f(new Vector2f(0.1f, 0.5f), new Vector2f(0.5f, 0.5f)),
                        new LineSegment2f(new Vector2f(0.5f, 0.5f), new Vector2f(0.7f, 0.5f * (MathHelper.Sin(a) + 1.0f)))
                    );

            
                    canvas.DrawShape(pen, b, null);

                    canvas.End();
                }

                device.SwapChain.Present();
                
                a += 0.01f;
            }

        }

        [CorrectnessTest]
        public void FontRendering(GraphicsDevice device)
        {
            ICanvas canvas = new GraphicsCanvas(device, device.SwapChain, new Vector2f(1.0f, 1.0f));

            // We first create all needed fills.
            RadialFill solidFill = new RadialFill(new Vector2f(0.2f, 0.3f), .4f, Colour.Blue, Colour.LightBlue);
            Font font = new Font(SVGFontProvider.ImportSVG("vera.svg"));

            Pen pen = new Pen(solidFill, 0.01f, 0.0f, OutlineEnd.Square);

            bool exit = false;

            device.SwapChain.Window.Closed += delegate(Window w) { exit = true; };

            float a = 0;

            while (!exit)
            {
                device.SwapChain.Window.DoEvents();

                using (DeviceLock l = device.Lock())
                {
                    device.Clear(device.SwapChain, Colour.Green);

                    device.SetViewports(new Region2i(0, 0, (int)device.SwapChain.Width, (int)device.SwapChain.Height));

                    // We render.
                    canvas.Begin(CanvasRenderFlags.None);

                    canvas.PixelErrorTolerance = 0.01f + 0.001f * a;

                    Vector2f[] polygon1 = new Vector2f[]
                    {
                        new Vector2f(0.1f,0.1f),
                        new Vector2f(0.9f,0.1f),
                        new Vector2f(0.9f, 0.9f),
                        new Vector2f(0.15f,0.9f),
                        new Vector2f(0.5f, 0.5f),
                        new Vector2f(0.2f, 0.5f),
                        new Vector2f(0.1f, 0.9f)
                    };

                    Vector2f[] polygon2 = new Vector2f[]
                    {
                        new Vector2f(0,0),
                        new Vector2f(0.1f,0f),
                        new Vector2f(0.2f, 0.2f),
                        new Vector2f(0.54f,0.2f),
                        new Vector2f(0.61f, 0.0f),
                        new Vector2f(0.72f, 0.0f),
                        new Vector2f(0.42f, 0.8f),
                        new Vector2f(0.37f, 0.7f),
                        new Vector2f(0.51f, 0.29f),
                        new Vector2f(0.22f, 0.3f),
                        new Vector2f(0.37f, 0.71f),
                        new Vector2f(0.42f, 0.81f),
                        new Vector2f(0.3f, 0.8f)
                    };


                    canvas.FillShape(solidFill, new Polygon2f(polygon2), null);

                    //canvas.FillShape(solidFill, polygon2);
                    //canvas.DrawShape(pen, outline);

                    canvas.End();
                }

                device.SwapChain.Present();

                a += 0.01f;
            }
        }

        static TimeSpan span = new TimeSpan(0);

        [CorrectnessTest]
        public void FontRendering2(GraphicsDevice device)
        {
            using (ICanvas canvas = new GraphicsCanvas(device, device.SwapChain, new Vector2f(1.0f, 1.0f)))
            {

                // We first create all needed fills.
                //RadialFill solidFill = new RadialFill(new Vector2f(0.2f, 0.3f), .4f, Colour.Blue, Colour.LightBlue);
                SolidFill solidFill = new SolidFill(Colour.Black);
                Font font = new Font(SVGFontProvider.ImportSVG("vera.svg"));

                Pen pen = new Pen(solidFill, 0.007f, 0.0f, OutlineEnd.Square);

                bool exit = false;

                device.SwapChain.Window.Closed += delegate(Window w) { exit = true; };

                string txt = "ABC�DEFGHIJKLMNOPRS�TUVZ�abc�defghijklmnoprs�tuvz�123456789!?";//"CFGHIJKLMNSTUVZ��cfhklijmnstuvz��12357"; 

                float a = 0, b = 0;

                while (!exit)
                {
                    device.SwapChain.Window.DoEvents();

                    using (DeviceLock l = device.Lock())
                    {
                        device.Clear(device.SwapChain, Colour.White);

                        device.SetViewports(new Region2i(0, 0, (int)device.SwapChain.Width, (int)device.SwapChain.Height));



                        DateTime time = DateTime.Now;

                        // We render.
                        canvas.Begin(CanvasRenderFlags.None);


                        font.Render(pen.Fill, canvas, txt, 0.1f, TextOptions.UseCanvasUnits | TextOptions.Top, 1.5f,
                            new Vector2f(0, 0), new Vector2f(1, 1));
                        
                        canvas.End();

                        span += DateTime.Now - time;

                        Console.WriteLine("Span 1: {0}", span.TotalSeconds);
                    }

                    device.SwapChain.Present();

                    Console.WriteLine("FPS: {0}\nTrig Count: {1}", device.DevicePerformance.CurrentFPS,
                      device.DevicePerformance.MaxTrianglesPerFrame);

                    a += 0.002f;
                    b += 0.01f;

                    /*
                    if (a > 0.005f)
                    {
                        txt += (char)(txt[txt.Length - 1] + 1);
                        a = 0;
                    }*/
                }
            }
        }

    }
#endif
}
