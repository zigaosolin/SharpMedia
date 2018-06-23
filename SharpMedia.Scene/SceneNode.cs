using System;
using System.Collections.Generic;
using System.Text;
using SharpMedia.Math;
using SharpMedia.Math.Matrix;
using System.Collections.ObjectModel;
using SharpMedia.AspectOriented;

namespace SharpMedia.Scene
{

    /// <summary>
    /// A scene node class.
    /// </summary>
    public sealed class SceneNode
    {
        #region Private Members
        string name = string.Empty;
        Vector3d position;
        Quaterniond orientation;
        bool isStatic = false;
        bool isMutable = false;
        SceneNode parent = null;
        SceneNode prefab = null;

        List<SceneNode> children = new List<SceneNode>();
        SortedList<Type, List<object>> components = new SortedList<Type, List<object>>();
        SortedList<string, object> localVariables = new SortedList<string, object>();
        #endregion

        #region Private Members

        void AssertMutable()
        {
            if (!isMutable)
            {
                throw new InvalidOperationException("Scene node must be mutable for this operation.");
            }
        }

        void AssertImmutable()
        {
            if (isMutable)
            {
                throw new InvalidOperationException("Scene node must be mutable for this operation.");
            }
        }

        void AssertStatic()
        {
            if (!isStatic)
            {
                throw new InvalidOperationException("Scene node must be static for this operation.");
            }
        }

        void AssertDynamic()
        {
            if (isStatic)
            {
                throw new InvalidOperationException("Scene node must be dynamic for this operation.");
            }
        }

        Type GetQueryType(object obj)
        {
            // We extract the "query type".
            Type type = sceneComponent.GetType();
            object[] attributes = type.GetCustomAttributes(typeof(SceneComponentAttribute), true);
            if (attributes.Length > 0)
            {
                Type queryType = (attributes[0] as SceneComponentAttribute).QueryType;
                if (queryType != null)
                {
                    type = queryType;
                }
            }

            return type;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// A scene node constructor.
        /// </summary>
        public SceneNode()
        {
        }

        /// <summary>
        /// A scene node contructor with name.
        /// </summary>
        /// <param name="name"></param>
        public SceneNode(string name)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// The name of scene node.
        /// </summary>
        public string Name
        {
            get { return name; }
            set 
            {
                AssertMutable();
                name = value; 
            }
        }

        /// <summary>
        /// The relative to parent position.
        /// </summary>
        public Vector3d Position
        {
            get { return position; }
            set 
            {
                AssertDynamic();
                AssertMutable();
                position = value; 
            }
        }

        /// <summary>
        /// The relative to parent orientation.
        /// </summary>
        public Quaterniond Orientation
        {
            get { return orientation; }
            set 
            {
                AssertDynamic();
                AssertMutable();
                orientation = value; 
            }
        }

        // TODO: add helpers for pose, matrix setting, abosulte matrix, float based matrix when
        // camera is present etc.

        /// <summary>
        /// Obtains the absolute (world) position.
        /// </summary>
        public Vector3d WorldPosition
        {
            get
            {
                if (parent != null)
                {
                    return parent.WorldPosition + position;
                }
                return position;
            }
        }

        /// <summary>
        /// Obtains the absolute (world) orientation.
        /// </summary>
        public Quaterniond WorldOrientation
        {
            get
            {
                if (parent != null)
                {
                    return parent.WorldOrientation * orientation;
                }
                return orientation;
            }
        }

        /// <summary>
        /// Gets or sets if the scene node is static.
        /// </summary>
        public bool IsStatic
        {
            get { return isStatic; }
            set 
            {
                AssertMutable();
                isStatic = value; 
            }
        }

        /// <summary>
        /// Gets or sets if the scene node is mutable.
        /// </summary>
        public bool IsMutable
        {
            get { return isMutable; }
            set
            {
                isMutable = value;
            }
        }

        /// <summary>
        /// Obtains the parent fo scene node.
        /// </summary>
        public SceneNode Parent
        {
            get
            {
                return parent;
            }
        }

        /// <summary>
        /// Sets or gets the prefab node.
        /// </summary>
        public SceneNode Prefab
        {
            get { return prefab; }
            set
            {
                AssertMutable();
                prefab = value;
            }
        }

        /// <summary>
        /// The children of this node.
        /// </summary>
        public ReadOnlyCollection<SceneNode> Children
        {
            get { return new ReadOnlyCollection<SceneNode>(children); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a local variable.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetLocal(string name)
        {
            return localVariables[name];
        }

        /// <summary>
        /// Creates a local variable.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public void CreateLocal(string name, object var)
        {
            localVariables.Add(name, var);
        }

        /// <summary>
        /// Deletes a local variable.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public void DeleteLocal(string name)
        {
            localVariables.Remove(name);
        }

        /// <summary>
        /// Adds a child.
        /// </summary>
        /// <param name="node"></param>
        public void AddChild(SceneNode node)
        {
            AssertMutable();

            if (node.parent != null)
            {
                throw new InvalidOperationException("Scene node is already in tree.");
            }

            lock (children)
            {
                node.parent = this;
                children.Add(node);
            }
            
        }

        /// <summary>
        /// Removes a child.
        /// </summary>
        /// <param name="node"></param>
        public void RemoveChild(SceneNode node)
        {
            AssertMutable();

            if (node.parent != this)
            {
                throw new InvalidOperationException("Scene node is not a child of this node.");
            }

            lock (children)
            {
                node.parent = null;
                children.Add(node);
            }
        }

        /// <summary>
        /// Adds a scene component.
        /// </summary>
        /// <param name="sceneComponent">The scene component.</param>
        public void AddSceneComponent([NotNull] object sceneComponent)
        {
            AssertMutable();

            Type type = GetQueryType(sceneComponent);

            lock (components)
            {
                List<object> perTypeObjects;
                if (components.TryGetValue(type, out perTypeObjects))
                {
                    perTypeObjects.Add(components);
                }
                else
                {
                    perTypeObjects = new List<object>();
                    perTypeObjects.Add(sceneComponent);
                    components.Add(type, perTypeObjects);
                }
            }

        }

        /// <summary>
        /// Removes a scene component.
        /// </summary>
        /// <param name="sceneComponent"></param>
        public void RemoveSceneComponent([NotNull] object sceneComponent)
        {
            AssertMutable();

            Type type = GetQueryType(sceneComponent);

            lock (components)
            {
                List<object> perTypeObjects;
                if (!components.TryGetValue(type, out perTypeObjects) || 
                    !perTypeObjects.Remove(sceneComponent))
                {
                    throw new Exception("Scene component not found, cannot remove it.");
                }

                if (perTypeObjects.Count == 0)
                {
                    components.Remove(type);
                }
            }
        }

        #endregion

        #region Search Helpers

        public SceneNode GetChild(string name)
        {
            return null;
        }

        public object GetComponent(string name)
        {
            return null;
        }

        public object[] GetComponents(string name)
        {
            return null;
        }

        public object GetComponent(Type type)
        {
            return null;
        }

        public object[] GetComponents(Type type)
        {
            return null;
        }

        public object[] GetComponents(Type type, Predicate<object> predicate)
        {
            return null;
        }

        #endregion

    }
}
