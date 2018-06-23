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
using SharpMedia.Math.Shapes.Volumes;
using System.Collections.ObjectModel;

namespace SharpMedia.Physics
{



    /// <summary>
    /// A physics scene object.
    /// </summary>
    public sealed class PhysicsScene : IDisposable
    {
        #region Private Members
        List<AutomaticEffector> effectors = new List<AutomaticEffector>();
        #endregion

        #region Public Classes

        /// <summary>
        /// An effector that is automatically applied to scene.
        /// </summary>
        public sealed class AutomaticEffector
        {
            /// <summary>
            /// The effector that is used.
            /// </summary>
            public Effectors.IEffector Effector;

            /// <summary>
            /// The mask to which effector is applied.
            /// </summary>
            public ulong Mask = ulong.MaxValue;

        }

        #endregion

        #region Automatic Effectors

        /// <summary>
        /// Registers an automatic effector.
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="effector"></param>
        public void RegisterAutomaticEffector(ulong mask, Effectors.IEffector effector)
        {
            AutomaticEffector ef = new AutomaticEffector();
            ef.Mask = mask;
            ef.Effector = effector;

            effectors.Add(ef);
        }


        /// <summary>
        /// Unregisters an autoamtic effector.
        /// </summary>
        /// <param name="effector"></param>
        public void UnRegisterAutomaticEffector(Effectors.IEffector effector)
        {
            effectors.RemoveAll(delegate(AutomaticEffector ef) { return ef.Effector == effectors; });
        }


        /// <summary>
        /// Gets all automatic effectors.
        /// </summary>
        public ReadOnlyCollection<AutomaticEffector> AutomaticEffectors
        {
            get
            {
                return effectors;
            }
        }


        #endregion

        #region Public Members

        /// <summary>
        /// Obtains all physics objects that contain bits from mask.
        /// </summary>
        /// <param name="mask">The mask.</param>
        /// <returns>The object list.</returns>
        public IPhysicsObject[] GetObjects(ulong mask)
        {
            return null;
        }

        /// <summary>
        /// Obtains all objects.
        /// </summary>
        /// <returns>The object list.</returns>
        public IPhysicsObject[] GetAllObjects()
        {
            return null;
        }

        /// <summary>
        /// Obtains all objects that are contained in bounding box.
        /// </summary>
        /// <param name="boundingBox">The buondung box.</param>
        /// <returns></returns>
        public IPhysicsObject[] GetObjects(AABoxd boundingBox)
        {
            return null;
        }

        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
