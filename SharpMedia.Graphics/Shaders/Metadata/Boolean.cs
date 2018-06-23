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
using SharpMedia.Graphics.Shaders.Operations;

namespace SharpMedia.Graphics.Shaders.Metadata
{
    /// <summary>
    /// A boolean.
    /// </summary>
    public sealed class Boolx1 : PinBinder
    {
        /// <summary>
        /// Creates boolean.
        /// </summary>
        internal Boolx1(Pin pin, CodeGenerator generator)
            : base(generator, pin)
        {
        }


        #region Operators

        /// <summary>
        /// Or operator.
        /// </summary>
        public static Boolx1 operator |([NotNull] Boolx1 b1, 
                                        [NotNull] Boolx1 b2)
        {
            return null;
        }

        /// <summary>
        /// And operator.
        /// </summary>
        public static Boolx1 operator &([NotNull] Boolx1 b1, 
                                        [NotNull] Boolx1 b2)
        {
            return null;
        }

        #endregion
    }

    /// <summary>
    /// A boolean.
    /// </summary>
    public sealed class Boolx2 : PinBinder
    {
        /// <summary>
        /// Creates boolean.
        /// </summary>
        internal Boolx2(Pin pin, CodeGenerator generator)
            : base(generator, pin)
        {
        }

        #region Properties

        /// <summary>
        /// The X swizzle.
        /// </summary>
        public Boolx1 X
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.X);
                op.BindInputs(pin);
                return new Boolx1(op.Outputs[0], Generator);
            }
        }

        /// <summary>
        /// The Y swizzle.
        /// </summary>
        public Boolx1 Y
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Y);
                op.BindInputs(pin);
                return new Boolx1(op.Outputs[0], Generator);
            }
        }


        #endregion


        #region Operators

        /// <summary>
        /// True if all are true.
        /// </summary>
        /// <returns></returns>
        public Boolx1 All()
        {
            AllOperation op = new AllOperation();
            op.BindInputs(pin);
            return new Boolx1(op.Outputs[0], Generator);
        }

        /// <summary>
        /// Any operation (if at least one is true).
        /// </summary>
        /// <returns></returns>
        public Boolx1 Any()
        {
            AnyOperation op = new AnyOperation();
            op.BindInputs(pin);
            return new Boolx1(op.Outputs[0], Generator);
        }

        /// <summary>
        /// None (all false) operation.
        /// </summary>
        /// <returns></returns>
        public Boolx1 None()
        {
            NoneOperation op = new NoneOperation();
            op.BindInputs(pin);
            return new Boolx1(op.Outputs[0], Generator);
        }

        #endregion
    }

    /// <summary>
    /// A boolean.
    /// </summary>
    public sealed class Boolx3 : PinBinder
    {
        /// <summary>
        /// Creates boolean.
        /// </summary>
        internal Boolx3(Pin pin, CodeGenerator generator)
            : base( generator, pin)
        {
        }
    }

    /// <summary>
    /// A boolean.
    /// </summary>
    public sealed class Boolx4 : PinBinder
    {
        /// <summary>
        /// Creates boolean.
        /// </summary>
        internal Boolx4(Pin pin, CodeGenerator generator)
            : base(generator, pin)
        {
        }
    }
}
