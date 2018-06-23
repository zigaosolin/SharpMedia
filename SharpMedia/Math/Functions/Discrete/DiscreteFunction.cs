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

namespace SharpMedia.Math.Functions.Discrete
{
    /// <summary>
    /// A discrete function, defined only for positive (and zero) value.
    /// </summary>
    /// <param name="value">The value of delegate.</param>
    /// <returns>The value of function at value.</returns>
    public delegate double DiscreteFunctiond(uint value);

    /// <summary>
    /// A discrete function, defined only for positive (and zero) value.
    /// </summary>
    /// <param name="value">The value of delegate.</param>
    /// <returns>The value of function at value.</returns>
    public delegate float DiscreteFunctionf(uint value);

    /// <summary>
    /// A discrete function.
    /// </summary>
    public interface IDiscreteFunction
    {
        /// <summary>
        /// A float discrete fuction.
        /// </summary>
        DiscreteFunctionf Functionf { get; }

        /// <summary>
        /// A double discrete function.
        /// </summary>
        DiscreteFunctiond Functiond { get; }

        /// <summary>
        /// Evalues function.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        double Eval(uint value);
    }

}
