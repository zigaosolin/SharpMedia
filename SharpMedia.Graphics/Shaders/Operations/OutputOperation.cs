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
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics.Shaders.Operations
{

    /// <summary>
    /// An output operation. Output operation has only input pins and because of this allows
    /// the bindings to be specified per components and not all at once.
    /// </summary>
    [Serializable]
    public class OutputOperation : IOperation
    {
        #region Private Members
        PinComponent components = PinComponent.None;
        SortedList<PinComponent, Pin> inputs = new SortedList<PinComponent, Pin>();

        string comment = string.Empty;
        ShaderCode code;
        PinsDescriptor inputDesc = new PinsDescriptor();
        #endregion

        #region Private Methods

        void UpdateDesc()
        {
            // We create output desc.
            List<PinDescriptor> descs = new List<PinDescriptor>();
            foreach (KeyValuePair<PinComponent, Pin> pair in inputs)
            {
                descs.Add(new PinDescriptor(pair.Value.Format, string.Empty, pair.Key));
            }

            inputDesc = new PinsDescriptor(descs.ToArray());
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the components.
        /// </summary>
        public PinComponent Components
        {
            get
            {
                return components;
            }
        }

        /// <summary>
        /// Adds a component.
        /// </summary>
        /// <param name="c"></param>
        public void AddComponent(PinComponent c, PinFormat format)
        {
            if (HasComponent(c))
            {
                throw new InvalidOperationException("The component " + c.ToString() + " already exists.");
            }

            components |= c;
            inputs.Add(c, new Pin(format, Pin.NotArray, null));

            UpdateDesc();
        }

        /// <summary>
        /// Adds component and links.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="pin"></param>
        public void AddComponentAndLink(PinComponent c, [NotNull] Pin pin)
        {
            if (HasComponent(c))
            {
                throw new InvalidOperationException("The component " + c.ToString() + " already exists.");
            }

            components |= c;
            inputs.Add(c, pin);

            UpdateDesc();
        }

        /// <summary>
        /// Unlinks the component.
        /// </summary>
        /// <param name="c"></param>
        public void UnlinkComponent(PinComponent c)
        {
            if (!HasComponent(c))
            {
                throw new InvalidOperationException("The component " + c.ToString() + " does not exist.");
            }

            components &= ~c;
            inputs.Remove(c);

            UpdateDesc();

        }

        /// <summary>
        /// Is component available.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool HasComponent(PinComponent c)
        {
            return (c & components) != 0; 
        }

        #endregion

        #region IOperation Members

        public PinsDescriptor InputDescriptor
        {
            get { return inputDesc; }
        }

        public void BindInputs(params Pin[] pins)
        {
            // We validate.
            InputDescriptor.ValidateThrow(pins);

            // We bind them.
            inputs = new SortedList<PinComponent, Pin>();
            for (uint i = 0; i < InputDescriptor.Count; i++)
            {
                PinDescriptor desc = InputDescriptor[i];
                inputs.Add(desc.Component, pins[i]);
            }
            
        }

        public Pin[] Inputs
        {
            get 
            {
                Pin[] r = new Pin[inputs.Count];
                for (int i = 0; i < inputs.Count; i++)
                {
                    r[i] = inputs.Values[i];
                }

                return r;
            }
        }

            public Pin[] Outputs
        {
            get { return new Pin[] { }; }
        }

        public IScope Scope
        {
            get { return code; }
        }

        #endregion

        #region ICommentable Members

        public string Comment
        {
            get
            {
                return comment;
            }
            set
            {
                comment = value;
            }
        }

        #endregion

        #region IBaseOperation Members

        public ShaderCompiler.Operand[] Compile(ShaderCompiler compiler, ShaderCompiler.Operand[] operands, FixedShaderParameters parameters, ref DualShareContext shareContext)
        {
            // We output all.
            int i = 0;
            foreach (KeyValuePair<PinComponent, Pin> p in inputs)
            {
                compiler.Output(operands[i++], p.Key);
            }

            return new ShaderCompiler.Operand[] { };
        }

        #endregion

        #region IEquatable<IOperation> Members

        public bool Equals(IOperation other)
        {
            return object.ReferenceEquals(this, other);
        }

        #endregion

        #region Internal Methods

        internal OutputOperation(ShaderCode code)
        {
            this.code = code;
        }

        #endregion
    }

    /// <summary>
    /// Pixel output is special because it only allows render target component outputs and depth.
    /// </summary>
    [Serializable]
    public class PixelOutputOperation : OutputOperation
    {
        #region Internal Methods

        internal PixelOutputOperation(ShaderCode code) 
            : base(code)
        {

        }

        #endregion
    }
}
