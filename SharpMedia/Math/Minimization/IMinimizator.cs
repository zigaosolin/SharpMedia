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

namespace SharpMedia.Math.Minimization
{

    /// <summary>
    /// Performs minimization of function (maximization can be performed using -f).
    /// </summary>
    public interface IMinimizator
    {

        /// <summary>
        /// Refines the minimization of function
        /// </summary>
        /// <returns>Returns false if number of steps is not sufficient.</returns>
        bool Refine(uint maxSteps);

        /// <summary>
        /// Current minimum (best).
        /// </summary>
        Vectord CurrentMinimum { get; }

        /// <summary>
        /// Error estimate, more like a tolerance.
        /// </summary>
        double ErrorEstimate { get; }

    }
}
