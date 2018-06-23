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
using System.IO;

namespace SharpMedia.AI.Neural
{
    /// <summary>
    /// A neural procedure.
    /// </summary>
    /// <param name="inputs"></param>
    /// <param name="outputs"></param>
    public delegate void NeuralProcedure(double[] inputs, double[] outputs);

    /// <summary>
    /// A neural net.
    /// </summary>
    public interface INeuralNet
    {
        #region Simulations

        /// <summary>
        /// Obtains a neural procedure.
        /// </summary>
        /// <param name="emitJIT">Should we JIT the procedure.</param>
        /// <returns></returns>
        NeuralProcedure GetProcedure(bool emitJIT);

        /// <summary>
        /// Emits a C# code directly.
        /// </summary>
        /// <param name="writer"></param>
        void EmitCSharpCode(StreamWriter writer);

        #endregion

        #region Learning

        /// <summary>
        /// Learn the net to handle inputs/outputs.
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="ouputs"></param>
        /// <returns>Fitting error in range [0,1]</returns>
        double Learn(double[] inputs, double[] ouputs);



        #endregion

    }
}
