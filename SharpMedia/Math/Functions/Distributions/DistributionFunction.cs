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

namespace SharpMedia.Math.Functions.Distributions
{

    /// <summary>
    /// A distibution function.
    /// </summary>
    public interface IDistributionFunction : IFunction
    {
        /// <summary>
        /// Comulative version of probability function. The version can be optimized or
        /// even sampled, but probability function must provide an aproximate.
        /// </summary>
        Functiond Comulative
        {
            get;
        }

        /// <summary>
        /// The mean value, value with most probability.
        /// </summary>
        double Mean
        {
            get;
        }

        /// <summary>
        /// Number dividing higher half from lover half (integral -inf -> median is the same as median -> inf).
        /// </summary>
        double Median
        {
            get;
        }

        /// <summary>
        /// The mode of probaility function.
        /// </summary>
        double Mode
        {
            get;
        }

        /// <summary>
        /// Variance of function.
        /// </summary>
        double Variance
        {
            get;
        }
    }

}
