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
using SharpMedia.Math;
using System.Threading;
using SharpMedia.Graphics.GUI.Metrics;
using System.Collections.ObjectModel;
using SharpMedia.Input;
using SharpMedia.Graphics.Vector;

namespace SharpMedia.Graphics.GUI.Widgets.Containers
{

    /// <summary>
    /// A sheet allows each child to be positioned as specified. 
    /// </summary>
    [Serializable]
    public class Container : Area, IContainer
    {
        #region Private Members
        protected IWidget focused = null;
        protected List<object> children = new List<object>();
        #endregion

        #region Events

        /// <summary>
        /// Container events.
        /// </summary>
        public class ContainerEvents : AreaEvents
        {
            #region Protected Members
            internal protected ContainerEvents(Container c)
                : base(c)
            {
            }
            #endregion

            #region Events

            #endregion
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Creates the sheet.
        /// </summary>
        public Container()
            : base((object)null)
        {
            eventHolder = new ContainerEvents(this);

            InitializeInternal();
        }

        /// <summary>
        /// Dummy constructor.
        /// </summary>
        /// <param name="dummy"></param>
        protected Container(object dummy)
            : base(dummy)
        {
        }

        /// <summary>
        /// Creates events.
        /// </summary>
        public new ContainerEvents Events
        {
            get { return eventHolder as ContainerEvents; }
        }

        public override IDisplayObject[] SubDisplayObjects
        {
            get
            {
                lock (syncRoot)
                {
                    List<IDisplayObject> cDisplays = new List<IDisplayObject>(children.Count);
                    for (int i = 0; i < children.Count; i++)
                    {
                        if (children[i] is IDisplayObject)
                        {
                            cDisplays.Add(children[i] as IDisplayObject);
                        }
                    }

                    return cDisplays.ToArray();
                }
            }
        }

        protected override void ApplyBoundingRectInternal(ICanvas canvas, Vector2f minPosition, Vector2f maxPosition)
        {
            base.ApplyBoundingRectInternal(canvas, minPosition, maxPosition);

            // We have to apply them to all children.
            for (int i = 0; i < children.Count; i++)
            {
                // For now ignore others.
                if (!(children[i] is IWidget)) continue;

                IWidget widget = children[i] as IWidget;

                // We extract position in canvas coordinates.
                Vector2f leftBottom, rightTop;
                if (!widget.GetPreferredRect(manager.Canvas, this, out leftBottom, out rightTop))
                {
                    // Not visible.
                    widget.ApplyBoundingRect(canvas, Vector2f.Zero, Vector2f.Zero);
                }
                else
                {
                    widget.ApplyBoundingRect(canvas, leftBottom, rightTop);
                }
            }
        }

        protected override void OnKeyPressInternal(IPointer cursor, SharpMedia.Input.KeyCodes code, 
            SharpMedia.Input.KeyboardModifiers modifiers, InputEventModifier eventModifiers)
        {
            base.OnKeyPressInternal(cursor, code, modifiers, eventModifiers);

            // We get child with focus.
            IWidget childWithFocus = InputRouting.GetChildWithFocus(this, manager);
            if (childWithFocus != null)
            {
                childWithFocus.OnKeyPress(cursor, code, modifiers, eventModifiers);
            }
        }

        protected override void OnKeyReleaseInternal(IPointer cursor, SharpMedia.Input.KeyCodes code, SharpMedia.Input.KeyboardModifiers modifiers)
        {
            base.OnKeyReleaseInternal(cursor, code, modifiers);

            // We get child with focus.
            IWidget childWithFocus = InputRouting.GetChildWithFocus(this, manager);
            if (childWithFocus != null)
            {
                childWithFocus.OnKeyRelease(cursor, code, modifiers);
            }
        }

        protected override void OnPointerMoveInternal(IPointer cursor, Vector2f deltaMove)
        {
            base.OnPointerMoveInternal(cursor, deltaMove);

            // We check two position to see if enter/leave event should be fired.
            IWidget child1 = InputRouting.GetChildAtPosition(this, manager, cursor.CanvasPosition - deltaMove);
            IWidget child2 = InputRouting.GetChildAtPosition(this, manager, cursor.CanvasPosition);

            // If child was not changed.
            if (child1 == child2)
            {
                if (child1 != null)
                {
                    child1.OnPointerMove(cursor, deltaMove);
                }
            }
            else
            {
                // TODO: maybe should adjust deltaMove to the move inside the child

                if (child1 != null)
                {
                    // We first fire move event.
                    child1.OnPointerMove(cursor, deltaMove);
                    child1.OnPointerLeave(cursor);
                }

                if (child2 != null)
                {
                    child2.OnPointerEnter(cursor);
                    child2.OnPointerMove(cursor, deltaMove);
 
                }
            }
        }

        protected override void OnPointerPressInternal(IPointer cursor, uint button, 
            InputEventModifier modifiers)
        {
            base.OnPointerPressInternal(cursor, button, modifiers);

            IWidget child = InputRouting.GetChildAtPosition(this, manager, cursor.CanvasPosition);
            if (child != null)
            {
                child.OnPointerPress(cursor, button, modifiers);
            }
        }

        protected override void OnPointerReleaseInternal(IPointer cursor, uint button)
        {
            base.OnPointerReleaseInternal(cursor, button);

            IWidget child = InputRouting.GetChildAtPosition(this, manager, cursor.CanvasPosition);
            if (child != null)
            {
                child.OnPointerRelease(cursor, button);
            }
        }

        protected override void OnWheelInternal(IPointer cursor, float deltaWheel)
        {
            base.OnWheelInternal(cursor, deltaWheel);

            // We get child with focus.
            IWidget childWithFocus = InputRouting.GetChildWithFocus(this, manager);
            if (childWithFocus != null)
            {
                childWithFocus.OnWheel(cursor, deltaWheel);
            }
        }

        #endregion

        #region Children Manipulation

        public virtual void AddChild(object child)
        {
            // TODO: Disallow cylic links

            // Ignores if child is already there.
            if (children.Contains(child)) return;

            children.Add(child);

            // We link it if widget.
            if (child is IWidget)
            {
                (child as IWidget).SetParent(this, manager);
            }
        }

        #endregion

        #region IContainerChangeContext Members

        public virtual void RemoveChild(object child)
        {
            children.RemoveAll(delegate(object obj)
            {
                if (child == obj)
                {
                    if (child is IWidget)
                    {
                        (child as IWidget).SetParent(null, null);
                    }
                    return true;
                }
                return false;
            });
        }

        #endregion

        #region IContainer Members

        public System.Collections.ObjectModel.ReadOnlyCollection<object> Children
        {
            get 
            {
                return new ReadOnlyCollection<object>(children);
            }
        }

        public ContainerMode Mode
        {
            get { return ContainerMode.None; }
            set { }
        }

        public IWidget Focused
        {
            get { return focused; }
            set 
            {
                if (focused == value) return;

                if (focused != null)
                {
                    focused.FocusChange(false);
                }

                focused = value;

                if (focused != null)
                {
                    focused.FocusChange(true);
                }
            }
        }

        #endregion
    }
}
