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

namespace SharpMedia.Math.Functions
{
    /// <summary>
    /// This is an interface, defined by functions.
    /// </summary>
    public interface IFunction
    {
        /// <summary>
        /// Generates a function, possibly optimizing it using expressions (dynamic code emission).
        /// </summary>
        /// <returns>The function returned.</returns>
        Functiond Functiond { get; }

        /// <summary>
        /// Generates a function, possibly optimizing it using expressions (dynamic code emission).
        /// All doubles are converted to floats (if code is emitted, conversions are necessary only
        /// at compile time).
        /// </summary>
        Functionf Functionf { get; }

        /// <summary>
        /// Evaluates at certain x.
        /// </summary>
        /// <remarks>Use this only if small number of samples are made, the delegate
        /// returning can be better suited because it can optimize code. On the other hand,
        /// delegate creates another indirection so eval may be faster for some functions.</remarks>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        double Eval(double x);
    }


}
