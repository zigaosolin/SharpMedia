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
using SharpMedia.Graphics.States;
using System.Threading;
using SharpMedia.Graphics.Vector;
using SharpMedia.Components.Applications;
using SharpMedia.Components.Configuration;
using System.Reflection;
using SharpMedia.Graphics.Vector.Fonts;
using SharpMedia.Graphics.Vector.Fonts.SVG;


namespace SharpMedia.Graphics.Driver.Direct3D10.Test
{
    /// <summary>
    /// A testing application.
    /// </summary>
    public sealed class TestApplication : Application
    {
        #region Private Members
        Input.InputService input;
        GraphicsService service;
        RenderTargetParameters renderTargetParameters;
        #endregion

        /// <summary>
        /// Optional configuration for Render target paramteters.
        /// </summary>
        public RenderTargetParameters RenderTargetParameters
        {
            set
            {
                renderTargetParameters = value;
            }
        }

        public TestApplication([Required] GraphicsService service)
        {
            this.service = service;
        }

        /// <summary>
        /// A standalone application runner (if not run by COS).
        /// </summary>
        public static void Main()
        {
            TestApplication app = new TestApplication(
                new GraphicsService(new Driver.Direct3D10.D3D10GraphicsServiceView(
                new D3D10GraphicsService())));
            
            app.Start("", new string[0]);
        }

        public override int Start(string verb, string[] args)
        {
            // We initialize the service.
            if (service.DeviceExists) throw new InvalidOperationException("Test application cannot be contructed, " +
                "device already exists.");

            if (renderTargetParameters == null)
            {
                // We use defaults.
                renderTargetParameters = new RenderTargetParameters();
                renderTargetParameters.Windowed = true;
                renderTargetParameters.FormatCommon = CommonPixelFormatLayout.X8Y8Z8W8_UNORM;
                renderTargetParameters.DepthStencilCommonFormat = CommonPixelFormatLayout.D24_UNORM_S8_UINT;
                renderTargetParameters.BackBufferCount = 1;
                renderTargetParameters.BackBufferWidth = 800;
                renderTargetParameters.BackBufferHeight = 600;
                renderTargetParameters.MultiSampleType = 4;
                renderTargetParameters.MultiSampleQuality = 3;
                

            }

            
            Window window;
            Font font = new Font(SVGFontProvider.ImportSVG("dressel.svg"));
            using (GraphicsDevice device =
                service.CreateDevice(true, false, renderTargetParameters, out window))
            {
                // We craete input device.
                using (input = new Input.InputService(new Input.Driver.DirectInput.DIInput()))
                {
                    /*
                    DefaultTheme theme = new DefaultTheme(font);
                    input.Initialize(window);

                    bool isClosed = false;
                    window.Closed += delegate(Window w) { isClosed = true; };
                    IWindowManager manager = new DefaultWindowManager(device, device.SwapChain, input, theme);

                    Container container = new Container();

                    // We create a simple area.
                    Area area = new Area();
                    using (area.Enter())
                    {
                        Area.AreaStyle s = new Area.AreaStyle();
                        area.PreferredRect = new GuiRect(RectangleMode.MinMax,
                            new GuiVector2(new Vector2f(0.2f, 0.2f)),
                            new GuiVector2(new Vector2f(0.8f, 0.8f)));
                        s.Background = new BackgroundStyle(new SolidFill(Colour.Red));
                        area.Style = Style.Create<Area.AreaStyle>(null, 
                            new KeyValuePair<StyleState, Area.AreaStyle>(CommonStyleStates.Normal, s));
                    }

                    using (container.Enter())
                    {
                        container.AddChild(area);
                    }

                    
                    theme.AutomaticApply(container, true);

                    

                    RootWindow client = new RootWindow(device, manager, "Window Title", "SomeGroup",
                        WindowOptions.Close|WindowOptions.Maximimize|WindowOptions.Minimize, new Vector2i(100, 100), new Vector2i(300,300),
                        null, null, WindowState.Normal, container, 1, 0);

                    client.GuiManager.PreRendering += delegate(GuiManager guiManager)
                    {
                        guiManager.Canvas.Device.Clear(guiManager.Canvas.Target, Colour.Blue);
                    };

                    client.IsVisible = true;

                    manager.PreRendering += delegate(IWindowManager y) 
                    { 
                        device.Clear(device.SwapChain, Colour.Yellow); 
                    };
                    manager.PostRendering += delegate(IWindowManager z)
                    {
                        device.SwapChain.Present();
                    };

                    while (!isClosed)
                    {
                        client.GuiManager.Invalidate();
                        client.GuiManager.Render();
                        (manager as IWindowManagerControl).Update();
                        window.DoEvents();
                    }
                    (manager as IWindowManagerControl).Dispose();
                    
                    */

                    
                    /*
                    input.Initialize(window);

                    // We default to font rendering 2.
                    if (args.Length == 0)
                    {
                        
                        IGuiTheme theme = new DefaultTheme(font);
                        GuiTest test = new GuiTest();

                        using (GraphicsCanvas canvas = new GraphicsCanvas(device, device.SwapChain, new Vector2f(1, 1)))
                        {
                            test.SimpleGUI(canvas, theme, input);

                        }


                        // We now run demos.
                        
                        VectorTest test = new VectorTest();
                        test.FontRendering2(device);
                    }
                    /*
                    else
                    {
                        
                        VectorTest test = new VectorTest();
                        foreach (string arg in args)
                        {
                            MethodInfo info = typeof(VectorTest).GetMethod(arg);
                            if (info != null)
                            {
                                try
                                {
                                    info.Invoke(test, new object[] { device });
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("Could not run or error method {0}.", arg);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Could not run method {0}.", arg);
                            }
                        }
                    }*/
                    //}
                }

            }
            return 0;
            
        }
    }
}
