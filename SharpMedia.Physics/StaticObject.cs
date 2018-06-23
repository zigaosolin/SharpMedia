using System;
using System.Collections.Generic;
using System.Text;
using SharpMedia.Math;
using SharpMedia.Math.Shapes;
using SharpMedia.Math.Matrix;

namespace SharpMedia.Physics
{

    /// <summary>
    /// A static object, this is working as collider only.
    /// </summary>
    public sealed class StaticObject : IPhysicsBase
    {
        #region Private Members
        PhysicsScene scene;
        Material material;
        object[] shapes;
        #endregion

        #region Constructors

        /// <summary>
        /// A static object constructor.
        /// </summary>
        /// <param name="scene">The scene.</param>
        /// <param name="material">The material of object.</param>
        /// <param name="shapes">Shapes, in global coordinates. 
        /// The driver lists the supported shapes. This shapes will be modified.</param>
        public StaticObject(PhysicsScene scene, Material material, params object[] shapes)
        {
            this.scene = scene;
            this.material = material;
            this.shapes = shapes;
        }

        /// <summary>
        /// A static object constructor.
        /// </summary>
        /// <param name="scene">The scene this object belongs to.</param>
        /// <param name="material">The material of object.</param>
        /// <param name="position">The position of object.</param>
        /// <param name="orientation">The orientation of object.</param>
        /// <param name="shapes">Shapes, in local coordinates. 
        /// The driver list the supported shapes. This shapes will be modified.</param>
        public StaticObject(PhysicsScene scene, Material material, Vector3d position,
            Quaterniond orientation, params object[] shapes)
            : this(scene, material, Matrix4x4d.From(position, orientation), shapes)
        {
        }

        public StaticObject(PhysicsScene scene, Material material, Matrix4x4d pose, params object[] shapes)
        {
            for (int i = 0; i < shapes.Length; i++)
            {
                if (!(shapes[i] is ITransformable3d))
                {
                    throw new InvalidOperationException("One of shapes does not allow " +
                        "transforms, invalid shape type.");
                }

                (shapes[i] as ITransformable3d).Transform(pose);
            }

            this.scene = scene;
            this.material = material;
            this.shapes = shapes;

        }

        #endregion

        #region IPhysicsBase Members

        public PhysicsScene PhysicsScene
        {
            get { return scene; }
        }

        public bool TrySync()
        {
        }

        public void Sync()
        {
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
