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
using SharpMedia.Graphics;
using SharpMedia.Math.Shapes;

namespace SharpMedia.Physics
{
    /// <summary>
    /// Data for particle.
    /// </summary>
    public struct ParticleData
    {
        /// <summary>
        /// Position of particle.
        /// </summary>
        public Vector3d Position;

        /// <summary>
        /// Velocity of particle.
        /// </summary>
        public Vector3d Velocity;

        /// <summary>
        /// Lifetime of particle.
        /// </summary>
        public double LifeTime;

        /// <summary>
        /// Density of particle.
        /// </summary>
        public double Density;
    }

    /// <summary>
    /// A particle emitter.
    /// </summary>
    public sealed class Emitter
    {

    }

    /// <summary>
    /// Physically accurate fluid as particle system.
    /// </summary>
    /// <remarks>
    /// Fluid can deliver data in ParticleData[] array form or in Graphics TypelessBuffer
    /// form. The later is usually used since data needs to be transfered to GPU and Particle System
    /// can optimize that transfer (if data is already there, e.g. if simulation is on GPU).
    /// </remarks>
    public sealed class Fluid : IPhysicsObject, IMapable<ParticleData[]>
    {

        #region Constructors

        /// <summary>
        /// Creates particle system.
        /// </summary>
        /// <param name="scene">Physics scene.</param>
        /// <param name="maxParticles">Maximum number of particles allowed.</param>
        /// <param name="emitters">Particle emitters, can also be null (manual emit or startup emit).</param>
        /// <param name="sinks">Particle system sinks.</param>
        public Fluid(PhysicsScene scene, ulong maxParticles, Emitter[] emitters, IShaped[] sinks,
                     double stiffness, double viscosity, double density, double particlesPerUnitVolume)
        {

        }

        /// <summary>
        /// Creates particle system.
        /// </summary>
        /// <remarks>
        /// Particle system will create a graphics buffer and fill vertex data in it. Recognised
        /// components are position (P), velocity (U0), lifetime (U1) and density (U2).
        /// </remarks>
        /// <param name="scene">Physics scene.</param>
        /// <param name="maxParticles">Maximum number of particles allowed.</param>
        /// <param name="emitters">Particle emitters, can also be null (manual emit or startup emit).</param>
        /// <param name="sinks">Particle system sinks.</param>
        /// <param name="format"></param>
        public Fluid(PhysicsScene scene, ulong maxParticles, Emitter[] emitters, IShaped[] sinks,
                     double stiffness, double viscosity, double density, double particlesPerUnitVolume,
                    GraphicsDevice device,  VertexFormat format, BufferUsage bufferUsage)
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// Number of active particles.
        /// </summary>
        public ulong ParticleCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Maximum number of particles.
        /// </summary>
        public ulong MaxParticleCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Sink shapes, when iteraction occurs, particle is deleted.
        /// </summary>
        public IShaped[] Sinks
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Particle emitters.
        /// </summary>
        public Emitter[] Emitters
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Typeless buffer of current simulation.
        /// </summary>
        /// <remarks>
        /// Only read access of buffer is allowed since it is synhronized
        /// </remarks>
        public TypelessBuffer Buffer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Typed buffer for current simulation.
        /// </summary>
        /// <remarks>
        /// Always prefer this method over Buffer property since this one can cache views.
        /// </remarks>
        public VertexBufferView VertexBuffer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Higher values result in honey-like behaviour.
        /// </summary>
        public double Viscosity
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Stiffness of fluid.
        /// </summary>
        public double Stiffness
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {

            }
        }

        /// <summary>
        /// Damping of fluid.
        /// </summary>
        public double Damping
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {

            }
        }

        /// <summary>
        /// Density of fluid when it is resting.
        /// </summary>
        public double Density
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {

            }
        }

        /// <summary>
        /// Particles per unit volume (1x1x1) when the fluid is resting.
        /// </summary>
        public double ParticlesPerUnitVolume
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {

            }
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Writes data to array.
        /// </summary>
        public void CopyDataTo(List<ParticleData> particles, ulong offset, ulong count)
        {

        }

        /// <summary>
        /// Adds particles (manual emit).
        /// </summary>
        /// <param name="particles"></param>
        /// <returns>Number of particles sucessfully added.</returns>
        public ulong AddParticles(params ParticleData[] particles)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets initial data (or resets).
        /// </summary>
        /// <param name="particles"></param>
        public void SetData(params ParticleData[] particles)
        {

        }

        #endregion

        #region IPhysicsObject Members

        public void AddForce(SharpMedia.Math.Vector3d force)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void AddForceAtPosition(SharpMedia.Math.Vector3d force, SharpMedia.Math.Vector3d position)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ulong CollisionMask
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public bool IsSleeping
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion

        #region IPhysicsBase Members

        public PhysicsScene PhysicsScene
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool TrySync()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Sync()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IMapable<ParticleData> Members

        public ParticleData[] Map(MapOptions op)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UnMap()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

    }
}
