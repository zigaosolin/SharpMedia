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

namespace SharpMedia.Math.Functions.Complex
{

    /// <summary>
    /// A complex function.
    /// </summary>
    public interface IComplexFunction
    {
        /// <summary>
        /// Returns a complex functon.
        /// </summary>
        /// <param name="optimize"></param>
        /// <returns></returns>
        ComplexFunctiond ComplexFunctiond { get; }


        /// <summary>
        /// Returns a complex functon.
        /// </summary>
        /// <param name="optimize"></param>
        /// <returns></returns>
        ComplexFunctionf ComplexFunctionf { get; }


        /// <summary>
        /// Evals the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        Complexd Eval(Complexd x);
    }
}
