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
using SharpMedia.Math;

namespace SharpMedia.Graphics.Shaders.Operations
{

    /// <summary>
    /// Vertex input operation allows input transfer. It can retrieve pins at the beginning.
    /// </summary>
    [Serializable]
    public class InputOperation : IOperation
    {
        #region Private Members
        PinComponent components = PinComponent.None;
        SortedList<PinComponent, Pin> outputs = new SortedList<PinComponent,Pin>();

        string comment = string.Empty;
        ShaderCode code;
        PinsDescriptor inputDesc = new PinsDescriptor();
        #endregion

        #region Private Methods

        void UpdateDesc()
        {
            // We create output desc.
            List<PinDescriptor> descs = new List<PinDescriptor>();
            foreach (KeyValuePair<PinComponent, Pin> pair in outputs)
            {
                descs.Add(new PinDescriptor(pair.Value.Format, string.Empty, pair.Key));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// This is a read-only property that allows setting input components. These are
        /// or-ed pin components that are bound for input.
        /// </summary>
        public PinComponent InputComponents
        {
            get
            {
                return components;
            }
        }


        #endregion

        #region Methods

        /// <summary>
        /// Obtains pin matching component.
        /// </summary>
        /// <param name="c">The component.</param>
        /// <returns>The actual pin.</returns>
        public Pin PinAsOutput(PinComponent c)
        {
            Pin value;
            if (outputs.TryGetValue(c, out value))
            {
                return value;
            }
            return null;
        }

        /// <summary>
        /// Adds pins as output.
        /// </summary>
        /// <param name="c">The component link.</param>
        /// <param name="format">The pin format.</param>
        public void AddInput(PinComponent c, PinFormat format)
        {
            if (PinAsOutput(c) != null)
            {
                throw new ArgumentException("The component is already a part of input.");
            }

            Pin pin = new Pin(format, Pin.NotArray, this);
            outputs.Add(c, pin);
            UpdateDesc();
        }

        /// <summary>
        /// Removes pin as input.
        /// </summary>
        /// <param name="c">The component.</param>
        /// <remarks>Pin must not be used by anyone (it is tagged as invalid).</remarks>
        public void RemoveInput(PinComponent c)
        {
            Pin p = PinAsOutput(c);

            if (p != null)
            {
                // TODO: Must unlink it.

                components &= ~c;
                outputs.Remove(c);
                UpdateDesc();
            }
            
        }

        #endregion

        #region Constuctors

        /// <summary>
        /// A construction with components that input will use.
        /// </summary>
        /// <param name="components">The components.</param>
        internal InputOperation(ShaderCode scope)
        {
            this.code = scope;
        }

        #endregion

        #region IBaseOperation Members

        public ShaderCompiler.Operand[] Compile(ShaderCompiler compiler, ShaderCompiler.Operand[] operands, FixedShaderParameters parameters, ref DualShareContext shareContext)
        {
            ShaderCompiler.Operand[] results = new ShaderCompiler.Operand[outputs.Count];

            // We simply emit descriptors.
            int i = 0;
            foreach (KeyValuePair<PinComponent, Pin> pair in outputs)
            {
                results[i++] = compiler.CreateInput(pair.Value.Format, pair.Key);
            }

            return results;
        }

        #endregion

        #region IOperation Members

        public PinsDescriptor InputDescriptor
        {
            get 
            {
                return inputDesc;
            }
        }

        public void BindInputs(params Pin[] pins)
        {
            if (pins.Length != 0)
            {
                throw new ArgumentException("Invalid number of input pins, none expected.");
            }
        }

        public Pin[] Inputs
        {
            get { return new Pin[] { }; }
        }

        public Pin[] Outputs
        {
            get 
            {
                // We create output array.
                Pin[] result = new Pin[outputs.Count];
                for (int i = 0; i < outputs.Count; i++)
                {
                    result[i] = outputs.Values[i];
                }
                return result;
            }
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

        #region IEquatable<IOperation> Members

        public bool Equals(IOperation other)
        {
            return object.ReferenceEquals(this, other);
        }

        #endregion
    }
}
