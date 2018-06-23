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
using SharpMedia.Graphics.Shaders.Operations.Implementation;

namespace SharpMedia.Graphics.Shaders.Operations
{

    /// <summary>
    /// A while operation is looping operation. It is scoping since operations execute within it.
    /// While operation will execute as long as condition holds.
    /// </summary>
    [Serializable]
    public class WhileOperation : IDualOperation
    {
        #region Private Members
        internal WhileInputOperation inputOperation;
        internal WhileOutputOperation outputOperation;
        #endregion

        #region Static Members
        internal static readonly PinsDescriptor inputDesc = new PinsDescriptor(
            new PinDescriptor(PinFormatHelper.AllStandardFormats, true, "Loop value 1."),
            new PinDescriptor(PinFormatHelper.AllStandardFormats, true, "Loop value 2."),
            new PinDescriptor(PinFormatHelper.AllStandardFormats, true, "Loop value 3."));

        internal static readonly PinsDescriptor outputDesc = new PinsDescriptor(
            new PinDescriptor(PinFormat.Bool, "Continue loop."),
            new PinDescriptor(PinFormatHelper.AllStandardFormats, true, "Loop value 1."),
            new PinDescriptor(PinFormatHelper.AllStandardFormats, true, "Loop value 2."),
            new PinDescriptor(PinFormatHelper.AllStandardFormats, true, "Loop value 3."));

        #endregion

        #region Constructors

        /// <summary>
        /// Loop operation constructor.
        /// </summary>
        public WhileOperation()
        {
            inputOperation = new WhileInputOperation(this);
            outputOperation = new WhileOutputOperation(this);
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
