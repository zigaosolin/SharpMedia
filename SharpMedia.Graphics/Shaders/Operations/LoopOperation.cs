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
using SharpMedia.Graphics.States;
using SharpMedia.Graphics.Shaders.Operations.Implementation;

namespace SharpMedia.Graphics.Shaders.Operations
{

    /// <summary>
    /// A loop operation iterates through same procedure a few times.
    /// </summary>
    /// <remarks>Allows up to 3 loop values, which can be changed during loop. Their
    /// values are transfered to next iteration (e.g. their are not actually immutable).</remarks>
    [Serializable]
    public class LoopOperation : IDualOperation
    {
        #region Private Members
        internal LoopInputOperation inputOperation;
        internal LoopOutputOperation outputOperation;
        #endregion

        #region Static Members
        internal static readonly PinsDescriptor inputDesc = new PinsDescriptor(
            new PinDescriptor(PinFormat.UInteger, "Number of iterations."),
            new PinDescriptor(PinFormatHelper.AllStandardFormats, true, "Loop value 1."),
            new PinDescriptor(PinFormatHelper.AllStandardFormats, true, "Loop value 2."),
            new PinDescriptor(PinFormatHelper.AllStandardFormats, true, "Loop value 3."));

        internal static readonly PinsDescriptor outputDesc = new PinsDescriptor(
            new PinDescriptor(PinFormat.UInteger, "Number of iterations."),
            new PinDescriptor(PinFormatHelper.AllStandardFormats, true, "Loop value 1."),
            new PinDescriptor(PinFormatHelper.AllStandardFormats, true, "Loop value 2."),
            new PinDescriptor(PinFormatHelper.AllStandardFormats, true, "Loop value 3."));

        #endregion

        #region Constructors

        /// <summary>
        /// Loop operation constructor.
        /// </summary>
        public LoopOperation()
        {
            inputOperation = new LoopInputOperation(this);
            outputOperation = new LoopOutputOperation(this);
        }

        #endregion

        #region IDualOperation Members

        public IOperation InputOperation
        {
            get { return inputOperation; }
        }

        public IOperation OutputOperation
        {
            get { return outputOperation; }
        }

        #endregion
    }
}
