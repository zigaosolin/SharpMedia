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
    /// Binds a texture.
    /// </summary>
    public abstract class TextureBinder : PinBinder
    {

        protected TextureBinder(CodeGenerator gen, Pin pin)
            : base(gen, pin)
        {
        }
    }

    /// <summary>
    /// Binds a texture.
    /// </summary>
    public sealed class Texture2DBinder<T> : TextureBinder 
        where T : PinBinder
    {
        internal Texture2DBinder(Pin pin, CodeGenerator gen)
            : base(gen, pin)
        {
        }

        #region Loading methods

        /// <summary>
        /// Loads at specific address with mipmap and offset.
        /// </summary>
        public T Load([NotNull] Integerx3 position, Integerx2 offset)
        {
            if (position.Generator != this.Generator ||
                ((object)offset != null && offset.Generator != this.Generator))
            {
                throw new ArgumentException("Mixing generators not allowed.");
            }

            LoadOperation op = new LoadOperation();
            if ((object)offset != null)
            {
                op.BindInputs(pin, position.Pin, offset.Pin);
            }
            else
            {
                op.BindInputs(pin, position.Pin);
            }
            return (T)this.Generator.CreateFrom(op.Outputs[0]);
        }

        /// <summary>
        /// Loads at specific address with mipmap.
        /// </summary>
        public T Load([NotNull] Integerx3 position)
        {
            return Load(position, null);
        }

        /// <summary>
        /// Loads at specific address at mipmap 0.
        /// </summary>
        public T Load([NotNull] Integerx2 position)
        {
            Integerx3 addr = position.Generator.Expand<Integerx3>(position, ExpandType.AddZeros);
            return Load(addr);
        }

        #endregion

        #region Sampling Methods

        /// <summary>
        /// Sample at specific address.
        /// </summary>
        public T Sample([NotNull] SamplerBinder sampler, [NotNull] Floatx2 address, Integerx2 offset)
        {
            if (address.Generator != this.Generator ||
                ((object)offset != null && offset.Generator != this.Generator) ||
                sampler.Generator != this.Generator)
            {
                throw new ArgumentException("Mixing generators not allowed.");
            }

            SampleOperation op = new SampleOperation();
            if ((object)offset != null)
            {
                op.BindInputs(sampler.Pin, pin, address.Pin, offset.Pin);
            }
            else
            {
                op.BindInputs(sampler.Pin, pin, address.Pin);
            }
            return (T)this.Generator.CreateFrom(op.Outputs[0]);
        }

        /// <summary>
        /// Samples at specific address.
        /// </summary>
        public T Sample([NotNull] SamplerBinder sampler, [NotNull] Floatx2 address)
        {
            return Sample(sampler, address, null);
        }

        #endregion
    }

    /// <summary>
    /// Binds a texture.
    /// </summary>
    public sealed class Texture2DArrayBinder<T> : TextureBinder
        where T : PinBinder
    {
        internal Texture2DArrayBinder(Pin pin, CodeGenerator gen)
            : base(gen, pin)
        {
        }

        #region Loading methods

        /// <summary>
        /// Loads at specific address with mipmap and offset.
        /// </summary>
        public T Load([NotNull] Integerx4 position, Integerx2 offset)
        {
            if (position.Generator != this.Generator ||
                ((object)offset != null && offset.Generator != this.Generator))
            {
                throw new ArgumentException("Mixing generators not allowed.");
            }

            LoadOperation op = new LoadOperation();
            if ((object)offset != null)
            {
                op.BindInputs(pin, position.Pin, offset.Pin);
            }
            else
            {
                op.BindInputs(pin, position.Pin);
            }
            return (T)this.Generator.CreateFrom(op.Outputs[0]);
        }

        /// <summary>
        /// Loads at specific address with mipmap.
        /// </summary>
        public T Load([NotNull] Integerx4 position)
        {
            return Load(position, null);
        }

        /// <summary>
        /// Loads at specific address at mipmap 0.
        /// </summary>
        public T Load([NotNull] Integerx3 position)
        {
            Integerx4 addr = position.Generator.Expand<Integerx4>(position, ExpandType.AddZeros);
            return Load(addr);
        }

        #endregion

        #region Sampling Methods

        /// <summary>
        /// Sample at specific address.
        /// </summary>
        public T Sample([NotNull] SamplerBinder sampler, [NotNull] Floatx3 address, Integerx3 offset)
        {
            if (address.Generator != this.Generator ||
                ((object)offset != null && offset.Generator != this.Generator) ||
                sampler.Generator != this.Generator)
            {
                throw new ArgumentException("Mixing generators not allowed.");
            }

            SampleOperation op = new SampleOperation();
            if ((object)offset != null)
            {
                op.BindInputs(sampler.Pin, pin, address.Pin, offset.Pin);
            }
            else
            {
                op.BindInputs(sampler.Pin, pin, address.Pin);
            }
            return (T)this.Generator.CreateFrom(op.Outputs[0]);
        }

        /// <summary>
        /// Samples at specific address.
        /// </summary>
        public T Sample([NotNull] SamplerBinder sampler, [NotNull] Floatx3 address)
        {
            return Sample(sampler, address, null);
        }

        #endregion
    }

    /// <summary>
    /// Binds a texture.
    /// </summary>
    public sealed class Texture1DArrayBinder<T> : TextureBinder
        where T : PinBinder
    {
        internal Texture1DArrayBinder(Pin pin, CodeGenerator gen)
            : base(gen, pin)
        {
        }

        #region Loading methods

        /// <summary>
        /// Loads at specific address with mipmap and offset.
        /// </summary>
        public T Load([NotNull] Integerx3 position, Integerx2 offset)
        {
            if (position.Generator != this.Generator ||
                ((object)offset != null && offset.Generator != this.Generator))
            {
                throw new ArgumentException("Mixing generators not allowed.");
            }

            LoadOperation op = new LoadOperation();
            if ((object)offset != null)
            {
                op.BindInputs(pin, position.Pin, offset.Pin);
            }
            else
            {
                op.BindInputs(pin, position.Pin);
            }
            return (T)this.Generator.CreateFrom(op.Outputs[0]);
        }

        /// <summary>
        /// Loads at specific address with mipmap.
        /// </summary>
        public T Load([NotNull] Integerx3 position)
        {
            return Load(position, null);
        }

        /// <summary>
        /// Loads at specific address at mipmap 0.
        /// </summary>
        public T Load([NotNull] Integerx2 position)
        {
            Integerx3 addr = position.Generator.Expand<Integerx3>(position, ExpandType.AddZeros);
            return Load(addr);
        }

        #endregion

        #region Sampling Methods

        /// <summary>
        /// Sample at specific address.
        /// </summary>
        public T Sample([NotNull] SamplerBinder sampler, [NotNull] Floatx2 address, Integerx2 offset)
        {
            if (address.Generator != this.Generator ||
                ((object)offset != null && offset.Generator != this.Generator) ||
                sampler.Generator != this.Generator)
            {
                throw new ArgumentException("Mixing generators not allowed.");
            }

            SampleOperation op = new SampleOperation();
            if ((object)offset != null)
            {
                op.BindInputs(sampler.Pin, pin, address.Pin, offset.Pin);
            }
            else
            {
                op.BindInputs(sampler.Pin, pin, address.Pin);
            }
            return (T)this.Generator.CreateFrom(op.Outputs[0]);
        }

        /// <summary>
        /// Samples at specific address.
        /// </summary>
        public T Sample([NotNull] SamplerBinder sampler, [NotNull] Floatx2 address)
        {
            return Sample(sampler, address, null);
        }

        #endregion
    }

    /// <summary>
    /// Binds a texture.
    /// </summary>
    public sealed class Texture1DBinder<T> : TextureBinder
        where T : PinBinder
    {
        internal Texture1DBinder(Pin pin, CodeGenerator gen)
            : base(gen, pin)
        {
        }

        #region Loading methods

        /// <summary>
        /// Loads at specific address with mipmap and offset.
        /// </summary>
        public T Load([NotNull] Integerx2 position, Integerx1 offset)
        {
            if (position.Generator != this.Generator ||
                ((object)offset != null && offset.Generator != this.Generator))
            {
                throw new ArgumentException("Mixing generators not allowed.");
            }

            LoadOperation op = new LoadOperation();
            if ((object)offset != null)
            {
                op.BindInputs(pin, position.Pin, offset.Pin);
            }
            else
            {
                op.BindInputs(pin, position.Pin);
            }
            return (T)this.Generator.CreateFrom(op.Outputs[0]);
        }

        /// <summary>
        /// Loads at specific address with mipmap.
        /// </summary>
        public T Load([NotNull] Integerx2 position)
        {
            return Load(position, null);
        }

        /// <summary>
        /// Loads at specific address at mipmap 0.
        /// </summary>
        public T Load([NotNull] Integerx1 position)
        {
            Integerx2 addr = position.Generator.Expand<Integerx2>(position, ExpandType.AddZeros);
            return Load(addr);
        }

        #endregion

        #region Sampling Methods

        /// <summary>
        /// Sample at specific address.
        /// </summary>
        public T Sample([NotNull] SamplerBinder sampler, [NotNull] Floatx1 address, Integerx1 offset)
        {
            if (address.Generator != this.Generator ||
                ((object)offset != null && offset.Generator != this.Generator) ||
                sampler.Generator != this.Generator)
            {
                throw new ArgumentException("Mixing generators not allowed.");
            }

            SampleOperation op = new SampleOperation();
            if ((object)offset != null)
            {
                op.BindInputs(sampler.Pin, pin, address.Pin, offset.Pin);
            }
            else
            {
                op.BindInputs(sampler.Pin, pin, address.Pin);
            }
            return (T)this.Generator.CreateFrom(op.Outputs[0]);
        }

        /// <summary>
        /// Samples at specific address.
        /// </summary>
        public T Sample([NotNull] SamplerBinder sampler, [NotNull] Floatx1 address)
        {
            return Sample(sampler, address, null);
        }

        #endregion
    }

    /// <summary>
    /// A buffer texture binder.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class BufferTextureBinder<T> : TextureBinder 
        where T : PinBinder
    {
        internal BufferTextureBinder(Pin pin, CodeGenerator gen)
            : base(gen, pin)
        {
        }

        #region Loading methods

        /// <summary>
        /// Loads at specific address with mipmap and offset.
        /// </summary>
        public T Load([NotNull] Integerx1 position, Integerx1 offset)
        {
            if (position.Generator != this.Generator ||
                ((object)offset != null && offset.Generator != this.Generator))
            {
                throw new ArgumentException("Mixing generators not allowed.");
            }

            LoadOperation op = new LoadOperation();
            if ((object)offset != null)
            {
                op.BindInputs(pin, position.Pin, offset.Pin);
            }
            else
            {
                op.BindInputs(pin, position.Pin);
            }
            return (T)this.Generator.CreateFrom(op.Outputs[0]);
        }

        /// <summary>
        /// Loads at specific address with mipmap.
        /// </summary>
        public T Load([NotNull] Integerx1 position)
        {
            return Load(position, null);
        }

        #endregion
    }

    /// <summary>
    /// Binds a texture.
    /// </summary>
    public sealed class TextureCubeBinder<T> : TextureBinder
        where T : PinBinder
    {
        internal TextureCubeBinder(Pin pin, CodeGenerator gen)
            : base(gen, pin)
        {
        }


        #region Sampling Methods

        /// <summary>
        /// Sample at specific address.
        /// </summary>
        public T Sample([NotNull] SamplerBinder sampler, [NotNull] Floatx3 address)
        {
            if (address.Generator != this.Generator ||
                sampler.Generator != this.Generator)
            {
                throw new ArgumentException("Mixing generators not allowed.");
            }

            SampleOperation op = new SampleOperation();
            op.BindInputs(sampler.Pin, pin, address.Pin);
            
            return (T)this.Generator.CreateFrom(op.Outputs[0]);
        }

        #endregion
    }

    /// <summary>
    /// Binds a texture.
    /// </summary>
    public sealed class Texture3DBinder<T> : TextureBinder
        where T : PinBinder
    {
        internal Texture3DBinder(Pin pin, CodeGenerator gen)
            : base(gen, pin)
        {
        }

        #region Loading methods

        /// <summary>
        /// Loads at specific address with mipmap and offset.
        /// </summary>
        public T Load([NotNull] Integerx4 position, Integerx3 offset)
        {
            if (position.Generator != this.Generator ||
                ((object)offset != null && offset.Generator != this.Generator))
            {
                throw new ArgumentException("Mixing generators not allowed.");
            }

            LoadOperation op = new LoadOperation();
            if ((object)offset != null)
            {
                op.BindInputs(pin, position.Pin, offset.Pin);
            }
            else
            {
                op.BindInputs(pin, position.Pin);
            }
            return (T)this.Generator.CreateFrom(op.Outputs[0]);
        }

        /// <summary>
        /// Loads at specific address with mipmap.
        /// </summary>
        public T Load([NotNull] Integerx4 position)
        {
            return Load(position, null);
        }

        /// <summary>
        /// Loads at specific address at mipmap 0.
        /// </summary>
        public T Load([NotNull] Integerx3 position)
        {
            Integerx4 addr = position.Generator.Expand<Integerx4>(position, ExpandType.AddZeros);
            return Load(addr);
        }

        #endregion

        #region Sampling Methods

        /// <summary>
        /// Sample at specific address.
        /// </summary>
        public T Sample([NotNull] SamplerBinder sampler, [NotNull] Floatx3 address, Integerx3 offset)
        {
            if (address.Generator != this.Generator ||
                ((object)offset != null && offset.Generator != this.Generator) ||
                sampler.Generator != this.Generator)
            {
                throw new ArgumentException("Mixing generators not allowed.");
            }

            SampleOperation op = new SampleOperation();
            if ((object)offset != null)
            {
                op.BindInputs(sampler.Pin, pin, address.Pin, offset.Pin);
            }
            else
            {
                op.BindInputs(sampler.Pin, pin, address.Pin);
            }
            return (T)this.Generator.CreateFrom(op.Outputs[0]);
        }

        /// <summary>
        /// Samples at specific address.
        /// </summary>
        public T Sample([NotNull] SamplerBinder sampler, [NotNull] Floatx3 address)
        {
            return Sample(sampler, address, null);
        }

        #endregion
    }
}
