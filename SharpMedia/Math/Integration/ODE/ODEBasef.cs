// This file was generated by TemplateEngine from template source 'ODEBase'
// using template 'ODEBasef. Do not modify this file directly, modify it from template source.

using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMedia.Math.Integration.ODE
{

    /// <summary>
    /// An ODE output listener.
    /// </summary>
    public interface IODEListenerf
    {
        /// <summary>
        /// Dense listener (all valid points).
        /// </summary>
        bool IsDense { get; }

        /// <summary>
        /// Reports a point.
        /// </summary>
        /// <param name="vector"></param>
        void Output(Vectorf vector);
    }

    /// <summary>
    /// A ordinary differention equation solver with initial conditions.
    /// </summary>
    public interface IODESolverf
    {
        /// <summary>
        /// Integrates the fonction, taking 
        /// aproximatelly number of steps (const * evaluations).
        /// </summary>
        bool Integrate(uint steps, IODEListenerf listener);

        /// <summary>
        /// The error estimate of integration.
        /// </summary>
        double ErrorEstimate { get; }

        /// <summary>
        /// End condition, null if not integrated until there.
        /// </summary>
        Vectorf EndCondition { get; }

    }
}