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

namespace SharpMedia.Graphics.Images.Compositing.Effects
{

    /// <summary>
    /// A Box blur.
    /// </summary>
    public sealed class BoxBlur : Effect, ICompositeInterface
    {
        #region Private Members
        uint boxSize;
        #endregion

        #region Constructors

        #endregion

        #region Public Members


        public override ICompositeInterface Interface
        {
            get { return this; }
        }

        #endregion

        #region ICompositeInterface Members

        public object[] ParameterValues
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public SharpMedia.Graphics.Shaders.ShaderCompiler.Operand GetPixel(SharpMedia.Graphics.Shaders.ShaderCompiler compiler, SharpMedia.Graphics.Shaders.ShaderCompiler.Operand absolutePosition, Dictionary<ICompositeInterface, SharpMedia.Graphics.Shaders.ShaderCompiler.Operand[]> constants)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IInterface Members

        public Type TargetOperationType
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public SharpMedia.Graphics.Shaders.ParameterDescription[] AdditionalParameters
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion
    }
}
