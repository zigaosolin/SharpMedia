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
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics.Shaders.Metadata
{

    /// <summary>
    /// A pin array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class PinArray<T> : PinBinder 
        where T : PinBinder
    {
        #region Internal Members

        /// <summary>
        /// Creates an array.
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="gen"></param>
        internal PinArray(Pin pin, CodeGenerator gen)
            : base(gen, pin)
        {
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Gets or sets at index.
        /// </summary>
        public T this[uint index]
        {
            get
            {
                IndexInArrayOperation op = new IndexInArrayOperation();
                op.BindInputs(this.pin, Generator.Fixed(index).Pin);
                return (T)Generator.CreateFrom(op.Outputs[0]);
            }
            set
            {
                WriteToIndexInArrayOperation op = new WriteToIndexInArrayOperation();
                op.BindInputs(this.pin, Generator.Fixed(index).Pin, value.Pin);

                // New array is created, we copy to our pin.
                this.pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// Gets or sets at index.
        /// </summary>
        public T this[[NotNull] UIntegerx1 index]
        {
            get
            {
                if (index.Generator != this.Generator) throw new ArgumentException("Different generators combined.");

                IndexInArrayOperation op = new IndexInArrayOperation();
                op.BindInputs(this.pin, index.Pin);
                return (T)Generator.CreateFrom(op.Outputs[0]);
            }
            set
            {
                if (index.Generator != this.Generator) throw new ArgumentException("Different generators combined.");

                WriteToIndexInArrayOperation op = new WriteToIndexInArrayOperation();
                op.BindInputs(this.pin, index.Pin, value.Pin);

                // New array is created, we copy to our pin.
                this.pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// Gets or sets at index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[Integerx1 index]
        {
            get
            {
                if (index.Generator != this.Generator) throw new ArgumentException("Different generators combined.");

                IndexInArrayOperation op = new IndexInArrayOperation();
                op.BindInputs(this.pin, index.Pin);
                return (T)Generator.CreateFrom(op.Outputs[0]);
            }
            set
            {
                if (index.Generator != this.Generator) throw new ArgumentException("Different generators combined.");

                WriteToIndexInArrayOperation op = new WriteToIndexInArrayOperation();
                op.BindInputs(this.pin, index.Pin, value.Pin);

                // New array is created, we copy to our pin.
                this.pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The size of array.
        /// </summary>
        public UIntegerx1 Size
        {
            get
            {
                ArraySizeOperation op = new ArraySizeOperation();
                op.BindInputs(pin);
                return new UIntegerx1(op.Outputs[0], Generator);
            }
        }

        #endregion

    }
}
