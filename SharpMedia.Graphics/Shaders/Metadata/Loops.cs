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
using SharpMedia.Graphics.Shaders.Operations;

namespace SharpMedia.Graphics.Shaders.Metadata
{
    /// <summary>
    /// A general loop version.
    /// </summary>
    public class Loop
    {
        #region Private Members
        PinBinder[] binders;
        LoopOperation operation = new LoopOperation();
        bool ended = false;

        /// <summary>
        /// We create the loop.
        /// </summary>
        /// <param name="data"></param>
        internal Loop(UIntegerx1 n, params PinBinder[] data)
        {
            Pin[] input = new Pin[data.Length+1];
            input[0] = n.Pin;
            for(int i = 1; i < data.Length; i++)
            {
                input[i] = data[i-1].Pin;
            }

            // We bind inputs.
            operation.InputOperation.BindInputs(input);

            // We create binders for output.
            Pin[] outputs = operation.InputOperation.Outputs;
            binders = new PinBinder[outputs.Length];
            for (int i = 0; i < outputs.Length; i++)
            {
                binders[i] = n.Generator.CreateFrom(outputs[i]);
            }
        }

        internal void EndLoop()
        {
            if (ended == true) throw new InvalidOperationException("Cannot end the same loop twice.");
            ended = true;

            // We write binders (that were changed)
            Pin[] inputs = new Pin[binders.Length - 1];
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i] = binders[i + 1].Pin;
            }

            // We bind them.
            operation.OutputOperation.BindInputs(inputs);

        }

        #endregion

        #region Internal Members

        /// <summary>
        /// The iteration index.
        /// </summary>
        public UIntegerx1 IterationIndex
        {
            get
            {
                return (UIntegerx1)binders[0];
            }
        }

        /// <summary>
        /// Gets the pin binder or sets it.
        /// </summary>
        /// <remarks>It can be retrieved at the beginning of the loop and written after that. After loop is
        /// ended, only reads are available.</remarks>
        public PinBinder this[uint index]
        {
            get
            {
                return binders[(int)index+1];
            }
            set
            {
                if (ended) throw new InvalidOperationException("Loop already ended, cannot assign.");

                binders[(int)index+1] = value;
            }
        }

        /// <summary>
        /// Gets the pin binder or sets it.
        /// </summary>
        /// <remarks>It can be retrieved at the beginning of the loop and written after that. After loop is
        /// ended, only reads are available.</remarks>
        public PinBinder this[int index]
        {
            get
            {
                return binders[index+1];
            }
            set
            {
                if (ended) throw new InvalidOperationException("Loop already ended, cannot assign.");

                binders[index+1] = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// Loop with one argument.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Loop<T> : Loop
        where T : PinBinder
    {
        #region Protected Members

        internal Loop(UIntegerx1 n, T data)
            : base(n, data)
        {
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Value access.
        /// </summary>
        public T Value
        {
            get
            {
                return (T)this[0];
            }
            set
            {
                this[0] = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// Loop with one argument.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Loop<T, T2> : Loop
        where T : PinBinder 
        where T2 : PinBinder
    {
        #region Protected Members

        internal Loop(UIntegerx1 n, T data, T2 data2)
            : base(n, data, data2)
        {
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Value access.
        /// </summary>
        public T Value1
        {
            get
            {
                return (T)this[0];
            }
            set
            {
                this[0] = value;
            }
        }

        /// <summary>
        /// Value access.
        /// </summary>
        public T Value2
        {
            get
            {
                return (T)this[1];
            }
            set
            {
                this[1] = value;
            }
        }

        #endregion
    }
}
