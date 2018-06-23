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
using SharpMedia.Graphics.GUI.Widgets;
using SharpMedia.Graphics.GUI.Themes.Default.Renderers;
using SharpMedia.Graphics.GUI.Styles;
using SharpMedia.Graphics.Vector.Fills;
using SharpMedia.Graphics.GUI.Metrics;
using SharpMedia.Graphics.Vector;
using SharpMedia.Graphics.GUI.Widgets.Containers;
using SharpMedia.Graphics.Vector.Fonts;
using SharpMedia.Graphics.GUI.Styles.SubStyles;

namespace SharpMedia.Graphics.GUI.Themes.Default
{

    /// <summary>
    /// A default theme instance.
    /// </summary>
    public sealed class DefaultTheme : IGuiTheme
    {
        #region Private Members
        SortedList<string, Style> styleMap = new SortedList<string, Style>();
        SortedList<string, IGuiRenderer> rendererMap = new SortedList<string, IGuiRenderer>();
        #endregion

        #region Public Constructors

        public DefaultTheme(Font defaultFont)
        {

            // AREA
            rendererMap.Add(typeof(Area).FullName, new AreaRenderer());

            // Area style: white with no border.
            {
                Area.AreaStyle normalStyle = new Area.AreaStyle();

                normalStyle.Background = new BackgroundStyle(new SolidFill(Colour.White));
                normalStyle.Border = null;

                Style xstyle = Style.CreateImmutable<Area.AreaStyle>(true,
                    new KeyValuePair<StyleState, Area.AreaStyle>(CommonStyleStates.Normal, normalStyle));

                styleMap.Add(typeof(Area).FullName, xstyle);
            }

            // LABEL
            rendererMap.Add(typeof(Label).FullName, new LabelRenderer());

            // Label style: white with black border, black text, blue selection.
            {
                Label.LabelStyle style = new Label.LabelStyle();


                // For now we do not have font library.
                style.SelectedTextFont = new PartialFontStyle(new SolidFill(Colour.White));
                style.TextFont = new FontStyle(defaultFont, 24, 1.2f, TextOptions.None,
                    new SolidFill(Colour.Black));
                style.Background = new BackgroundStyle(new SolidFill(Colour.White));
                style.Border = null;
                style.SelectionBackground = new BackgroundStyle(new SolidFill(Colour.Blue));


                Label.LabelStyle pointerOver = new Label.LabelStyle();
                pointerOver.Parent = style;

                Label.LabelStyle focused = new Label.LabelStyle();
                focused.Parent = pointerOver;

                Style xstyle = Style.CreateImmutable<Label.LabelStyle>(true,
                    new KeyValuePair<StyleState, Label.LabelStyle>(CommonStyleStates.Normal, style),
                    new KeyValuePair<StyleState, Label.LabelStyle>(CommonStyleStates.Focused, focused),
                    new KeyValuePair<StyleState, Label.LabelStyle>(CommonStyleStates.PointerOver, pointerOver));

                styleMap.Add(typeof(Label).FullName, xstyle);
            }

            // BUTTON:
            rendererMap.Add(typeof(Button).FullName, new ButtonRenderer());

            {
                Button.ButtonStyle normalStyle = new Button.ButtonStyle();

                normalStyle.Background = new BackgroundStyle(new SolidFill(Colour.White));
                normalStyle.Border = new BorderStyle(new Pen(new SolidFill(Colour.Black), 0.0f, 3.0f, LineMode.Dot));

                Button.ButtonStyle pointerOverStyle = new Button.ButtonStyle();
                pointerOverStyle.Parent = normalStyle;

                Button.ButtonStyle clickedStyle = new Button.ButtonStyle();
                clickedStyle.Parent = pointerOverStyle;
                clickedStyle.Border = new BorderStyle(new Pen(new SolidFill(Colour.Red), 0.0f, 3.5f, LineMode.Dot));

                Style xstyle = Style.CreateImmutable<Button.ButtonStyle>(true,
                    new KeyValuePair<StyleState, Button.ButtonStyle>(CommonStyleStates.Normal, normalStyle),
                    new KeyValuePair<StyleState, Button.ButtonStyle>(CommonStyleStates.Clicked, clickedStyle),
                    new KeyValuePair<StyleState, Button.ButtonStyle>(CommonStyleStates.PointerOver, pointerOverStyle));

                styleMap.Add(typeof(Button).FullName, xstyle);

            }

            // POINTER
            rendererMap.Add(typeof(Standalone.GuiPointer).FullName, new PointerRenderer());

            // Pointer style:
            {
                Standalone.GuiPointer.GuiPointerStyle style = new SharpMedia.Graphics.GUI.Standalone.GuiPointer.GuiPointerStyle();
                style.Background = new BackgroundStyle(new SolidFill(Colour.Black));
                style.Border = null;

                Style xstyle = Style.CreateImmutable<Standalone.GuiPointer.GuiPointerStyle>(true,
                    new KeyValuePair<StyleState, Standalone.GuiPointer.GuiPointerStyle>(CommonStyleStates.Normal, style));

                styleMap.Add(typeof(Standalone.GuiPointer).FullName, xstyle);
            }
        }

        #endregion

        #region IGuiTheme Members

        public Style ObtainStyle(Type objectType, object specifier)
        {
            Style style;
            if (styleMap.TryGetValue(objectType.FullName, out style)) return style;

            // We do it for base class.
            if (objectType.BaseType != null) return ObtainStyle(objectType.BaseType, specifier);

            return null;
        }

        public IGuiRenderer ObtainRenderer(Type objectType, object additionalData)
        {
            IGuiRenderer renderer;
            if (rendererMap.TryGetValue(objectType.FullName, out renderer)) return renderer;

            if (objectType.BaseType != null) return ObtainRenderer(objectType.BaseType, additionalData);

            return null;
        }

        public void AutomaticApply(object obj, bool transverse)
        {
            // We apply theme to object.
            if (obj is IWidget)
            {
                IWidget widget = obj as IWidget;

                widget.EnterChange();

                try
                {
                    if (widget.Style != null)
                    {
                        // We just append it as last.
                        Style s = widget.Style;
                        while (s.Parent != null) s = s.Parent;

                        s.Parent = ObtainStyle(obj.GetType(), null);
                    }
                    else
                    {
                        widget.Style = ObtainStyle(obj.GetType(), null);
                    }
                    widget.Skin = ObtainRenderer(obj.GetType(), null);
                }
                finally
                {
                    widget.ExitChange();
                }

                
            }
            else if (obj is IDisplayObject)
            {
                // FIXME:
                (obj as IDisplayObject).Style = ObtainStyle(obj.GetType(), null);
                (obj as IDisplayObject).Skin = ObtainRenderer(obj.GetType(), null);
            }


            if (obj is IDisplayObject)
            {
                if (transverse)
                {
                    IDisplayObject[] t = (obj as IDisplayObject).SubDisplayObjects;
                    if (t != null)
                    {
                        foreach (IDisplayObject child in t)
                        {
                            AutomaticApply(child, true);
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentException("Object is not a known type to theme.");
            }

            
        }

        public void AutomaticUnApply(object obj, bool transverse)
        {
            if (obj is IWidget)
            {
                IWidget widget = obj as IWidget;

                // We obtain style.
                Style styleToRemove = ObtainStyle(obj.GetType(), null);
                IGuiRenderer rendererToRemove = ObtainRenderer(obj.GetType(), null);

                widget.EnterChange();
                try
                {
                    if (widget.Style != null)
                    {
                        // We just append it as last.
                        Style s = widget.Style;

                        if (s.Parent == null)
                        {
                            if (s == styleToRemove)
                            {
                                widget.Style = null;
                            }
                        }
                        else
                        {
                            while (s.Parent.Parent != null) s = s.Parent;

                            if (s.Parent == styleToRemove)
                            {
                                s.Parent = null;
                            }
                        }
                    }

                    // Remove only if to this skin.
                    if (widget.Skin == rendererToRemove)
                    {
                        widget.Skin = null;
                    }
                }
                finally
                {
                    widget.ExitChange();
                }

            }
        }

        #endregion
    }
}
