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
using SharpMedia.Graphics.Vector;
using SharpMedia.Graphics.GUI.Widgets;
using SharpMedia.Math.Shapes;
using SharpMedia.Math;
using SharpMedia.Graphics.GUI.Styles;
using SharpMedia.Graphics.Vector.Fills;
using SharpMedia.Graphics.GUI.Themes;
using System.Threading;
using SharpMedia.Graphics.GUI.Widgets.Containers;
using SharpMedia.Graphics.GUI.Metrics;
using SharpMedia.Graphics.Vector.Fonts;
using SharpMedia.Input;

namespace SharpMedia.Graphics.GUI
{

#if SHARPMEDIA_TESTSUITE
    
    
    public class GuiTest
    {

        public void SimpleGUI(IDeviceCanvas canvas, IGuiTheme theme, Input.InputService input)
        {
            canvas.PointsPerPixel = 0.001f;

            using (GuiManager manager = new GuiManager(canvas))
            {
                

                // We create a simple area.
                Area area = new Area();

                
                

                using (area.Enter())
                {
                    // We create a hiearchical style.
                    Style style = Style.Create<Area.AreaStyle>();

                    Area.AreaStyle astyle = new Area.AreaStyle();
                    astyle.Background.Fill = new SolidFill(Colour.LightBlue);
                    astyle.Border.Pen = new Pen(new SolidFill(Colour.Gray), 0.003f, 0.0f, LineMode.Square);
                    style.AddStyle(CommonStyleStates.Normal, astyle);

                    astyle = new Area.AreaStyle();
                    astyle.Background.Fill = new SolidFill(Colour.LightGreen);
                    astyle.Border.Pen = new Pen(new SolidFill(Colour.Gray), 0.003f, 0.0f, LineMode.Square);
                    style.AddStyle(CommonStyleStates.PointerOver, astyle);
                    

                    // We apply style.
                    area.Style = style;

                    area.PreferredRect = GuiRect.CreateRectangle(new GuiVector2(new Vector2f(0.1f, 0.1f)),
                        new GuiVector2(new Vector2f(0.4f, 0.4f)));
                }

                
                Button button = new Button(
                        GuiRect.CreateRectangle(
                          new GuiVector2(new Vector2f(0.1f, 0.6f)),
                          new GuiVector2(new Vector2f(0.4f, 0.9f))
                        ),
                        "Click me!", null);

                Label label = new Label();

                using (label.Enter())
                {
                    label.Text = "SharpMedia rocks!";

                    label.PreferredRect = GuiRect.CreateRectangle(new GuiVector2(new Vector2f(0.6f, 0.6f)),
                        new GuiVector2(new Vector2f(0.9f, 0.9f)));
                    label.TextSelectedRange = new Vector2i(1, 5);
                }

                Label label2 = new Label();
                using (label2.Enter())
                {
                    label2.PreferredRect = GuiRect.CreateRectangle(new GuiVector2(new Vector2f(0.6f, 0.1f)),
                        new GuiVector2(new Vector2f(0.9f, 0.4f)));
                    label2.IsEnabled = false;

                    label.Events.TextSelect += delegate(Label xlabel, Vector2i sel)
                    {
                        using (label2.Enter())
                        {
                            label2.Text = xlabel.SelectedText;
                        }
                    };

                    button.Events.ButtonClicked += delegate(Button button2)
                    {
                        using (label2.Enter())
                        {
                            label2.Text += label2.Text;
                        }
                    };
                }

                Container sheet = new Container();
                using (sheet.Enter())
                {
                    sheet.AddChild(area);
                    sheet.AddChild(label2);
                    sheet.AddChild(button);
                    sheet.AddChild(label);
                }

                // We apply theme.
                theme.AutomaticApply(sheet, true);

                manager.RootObject = sheet;
                manager.PreRendering += new Action<GuiManager>(PreRendering);
                manager.Rendered += new Action<GuiManager>(PostRendering);

                EventPump pump = new EventPump(input.CreateDevice(InputDeviceType.Mouse),
                    input.CreateDevice(InputDeviceType.Keyboard),
                    input.CreateDevice(InputDeviceType.Cursor));
                EventProcessor processor = new EventProcessor(pump);
                
                // We bind input.
                Standalone.InputRouter router = new Standalone.InputRouter(manager,
                    processor, theme.ObtainStyle(typeof(Standalone.GuiPointer), null),
                    theme.ObtainRenderer(typeof(Standalone.GuiPointer), null), null);

                bool end = false;



                canvas.Device.SwapChain.Window.Closed += delegate(Window w) { end = true; };
                DateTime time = DateTime.Now;
                while (!end)
                {
                    canvas.Device.SwapChain.Window.DoEvents();
                    while (processor.Process() != null) ;

                    // We update iut.
                    DateTime t = DateTime.Now;
                    manager.Update((float)(t - time).TotalSeconds);
                    time = t;

                    // We render it.
                    manager.Render();

                    Console.WriteLine(canvas.Device.DevicePerformance.CurrentFPS);

                    canvas.Device.SwapChain.Present();      
                }

                // Must dispose before it, devices bound so also disposed.
                pump.Dispose();


            }

        }

        void PreRendering(GuiManager obj)
        {
            IDeviceCanvas canvas = obj.Canvas as IDeviceCanvas;
            GraphicsDevice device = canvas.Device;

            device.Clear(canvas.Target, Colour.White);
            
        }

        void PostRendering(GuiManager obj)
        {
            
        }
    }

#endif
}
