// This file was generated by TemplateEngine from template source 'UniformStepper'
// using template 'UniformStepperf. Do not modify this file directly, modify it from template source.

using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMedia.Math.Interpolation
{
    /// <summary>
    /// A uniform interpolator interface.
    /// </summary>
    public interface IUniformStepperf
    {
        /// <summary>
        /// Interpolates value.
        /// </summary>
        /// <param name="t">In range [0,1]</param>
        /// <returns>Result in range [0,1].</returns>
        float Interpolate(float t);

    }
}
