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
using SharpMedia.Graphics.GUI.Styles;

namespace SharpMedia.Graphics.GUI
{

    

    /// <summary>
    /// A style holds State/State Style collection.
    /// </summary>
    public sealed class Style : 
        IEnumerable<KeyValuePair<StyleState, IStateStyle>>, 
        IPreChangeNotifier
    {
        #region Private Members
        Type styleType;
        Style parent;
        bool themeProvided = false;
        Action<IPreChangeNotifier> onChange;
        SortedDictionary<StyleState, IStateStyle> states
            = new SortedDictionary<StyleState, IStateStyle>();
        #endregion

        #region Helper Methods

        void PreChanged()
        {
            Action<IPreChangeNotifier> t = onChange;
            if (t != null)
            {
                t(this);
            }
        }

        void ChangeChild(IPreChangeNotifier n)
        {
            PreChanged();
        }

        void UnLinkStyle(IStateStyle style, StyleState state)
        {
            style.OnChange -= ChangeChild;

            // We obtain parent style.
            if (parent == null) return;

            IStateStyle parentStyle = parent[state];
            if (parentStyle == null) return;

            // We now search for this style and try to remove it.
            while (style.Parent != null)
            {
                if (style.Parent == parentStyle)
                {
                    style.Parent = null;
                    return;
                }

                style = style.Parent;
            }
        }

        void LinkStyle(IStateStyle style, StyleState state)
        {
            style.OnChange += ChangeChild;

            // We obtain parent style.
            if(parent == null) return;

            IStateStyle parentStyle = parent[state];
            if (parentStyle == null) return;


            // We now find the last link we should fill in with our parent style.
            IStateStyle t = style;
            while (t.Parent != null) t = t.Parent;

            // We now link it to parent provided state.
            if (t != parentStyle)
            {
                t.Parent = parentStyle;
            }
        }

        IStateStyle DirectGetState(StyleState state)
        {
            IStateStyle style;
            if (states.TryGetValue(state, out style)) return style;

            if (parent != null)
            {
                return parent.DirectGetState(state);
            }

            return null;
        }

        #endregion

        #region Style Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        private Style(Type type)
        {
            this.styleType = type;
        }

        /// <summary>
        /// Common contruction.
        /// </summary>
        private Style(Style parent)
        {
            this.styleType = parent.styleType;
            this.parent = parent;
        }


        #endregion

        #region Static Methods

        /// <summary>
        /// Creates a typed style.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Style Create<T>() 
            where T : IStateStyle
        {
            return new Style(typeof(T));
        }

        /// <summary>
        /// Creates immutable style.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Style Create<T>(params KeyValuePair<StyleState, T>[] data) 
            where T : IStateStyle
        {
            Style style = new Style(typeof(T));
            foreach (KeyValuePair<StyleState, T> d in data)
            {
                style.AddStyle(d.Key, d.Value);
            }


            return style;
        }

        /// <summary>
        /// Creates a typed style.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Style Create<T>(Style parent) 
            where T : IStateStyle
        {
            Style s = Create<T>();
            s.Parent = parent;

            return s;
        }

        /// <summary>
        /// Creates immutable style.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Style Create<T>(Style parent, 
            params KeyValuePair<StyleState, T>[] data) where T : IStateStyle
        {
            Style style = Create<T>(data);
            style.Parent = parent;

            return style;
        }

        /// <summary>
        /// Creates immutable style.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Style CreateImmutable<T>(bool themeProvided) 
            where T : IStateStyle
        {
            

            Style style = Create<T>();
            style.themeProvided = themeProvided;
            style.OnChange += delegate(IPreChangeNotifier x)
            {
                throw new InvalidOperationException("Style is immutable");
            };

            return style;
        }

        /// <summary>
        /// Creates immutable style.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Style CreateImmutable<T>(bool themeProvided, params KeyValuePair<StyleState, T>[] data)
            where T : IStateStyle
        {
            
            Style style = Create<T>(data);
            style.themeProvided = themeProvided;
            style.OnChange += delegate(IPreChangeNotifier x) 
            { 
                throw new InvalidOperationException("Style is immutable"); 
            };

            return style;
        }

        /// <summary>
        /// Creates immutable style.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Style CreateImmutable<T>(bool themeProvided, Style parent, params KeyValuePair<StyleState, T>[] data)
            where T : IStateStyle
        {
            Style style = Create<T>(data);
            style.Parent = parent;
            style.themeProvided = themeProvided;
            style.OnChange += delegate(IPreChangeNotifier x)
            {
                throw new InvalidOperationException("Style is immutable");
            };

            return style;
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Obtains style state.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="state1"></param>
        /// <param name="state2"></param>
        /// <returns></returns>
        public float GetStyleState<T>(StyleAnimationController controller, out T state1, out T state2)
        {
            state1 = this.GetStyle<T>(controller.CurrentState);

            // Animating data.
            state2 = default(T);
            float alpha = 0.0f;
            if (controller.NextState != null)
            {
                state2 = this.GetStyle<T>(controller.NextState);
                alpha = controller.Weight;
            }

            return alpha;
        }

        /// <summary>
        /// The style's type.
        /// </summary>
        public Type Type
        {
            get
            {
                return styleType;
            }
        }

        /// <summary>
        /// Is style theme provided.
        /// </summary>
        public bool IsTheme
        {
            get
            {
                return themeProvided;
            }
        }

        /// <summary>
        /// The parent.
        /// </summary>
        public Style Parent
        {
            get
            {
                return parent;
            }
            set
            {
                if (parent == value) return;

                PreChanged();

                if (parent != null && parent.styleType != styleType)
                {
                    throw new ArgumentException("Mixing inheritance with different style types invalid.");
                }

                // We first unlink them.
                foreach (KeyValuePair<StyleState, IStateStyle> state in states)
                {
                    UnLinkStyle(state.Value, state.Key);
                }
                
                // We reset parent.
                parent = value;

                // We relink them
                foreach (KeyValuePair<StyleState, IStateStyle> state in states)
                {
                    LinkStyle(state.Value, state.Key);
                }
            }
        }

        /// <summary>
        /// Gets or sets the style. If state does not exist, null is returned.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public IStateStyle this[StyleState state]
        {
            get
            {
                IStateStyle stateStyle;
                while (true)
                {
                    stateStyle = DirectGetState(state);
                    if (stateStyle != null) break;

                    if (state.Redirect == null) break;

                    state = state.Redirect;
                }
                
                return stateStyle;
            }
            [param: NotNull]
            set
            {
                PreChanged();

                IStateStyle xstyle;
                if (states.TryGetValue(state, out xstyle))
                {
                    UnLinkStyle(xstyle, state);
                    states.Remove(state);
                }

                // We now add it.
                if (value != null)
                {
                    LinkStyle(value, state);
                    states.Add(state, value);
                }
            }
        }

        /// <summary>
        /// Adds a style, throws if it already exists (use this[] for no throw).
        /// </summary>
        /// <param name="state"></param>
        /// <param name="style"></param>
        public void AddStyle(StyleState state, IStateStyle style)
        {
            PreChanged();

            states.Add(state, style);
            LinkStyle(style, state);
        }

        /// <summary>
        /// Is style provided by theme.
        /// </summary>
        public bool IsThemeProvided
        {
            get
            {
                return themeProvided;
            }
        }

        /// <summary>
        /// Obtains a typed style.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public T GetStyle<T>(StyleState state)
        {
            if (!Common.IsTypeSameOrDerived(typeof(T), styleType))
            {
                throw new ArgumentException("Style not compatible with generic type.");
            }

            return (T)this[state];
        }

        #endregion

        #region IEnumerable<KeyValuePair<StyleState,IStateStyle>> Members

        public IEnumerator<KeyValuePair<StyleState, IStateStyle>> GetEnumerator()
        {
            return states.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return states.GetEnumerator();
        }

        #endregion

        #region IChangeableEvent Members

        public event Action<IPreChangeNotifier> OnChange
        {
            add
            {
                lock (states)
                {
                    onChange += value;
                }
            }
            remove
            {
                lock (states)
                {
                    onChange -= value;
                }
            }
        }

        #endregion
    }
}
