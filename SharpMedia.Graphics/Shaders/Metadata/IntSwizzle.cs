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

    public sealed partial class Integerx2
    {
        /// <summary>
        /// The X swizzle.
        /// </summary>
        public Integerx1 X
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("X"));
                op.BindInputs(pin);
                return new Integerx1(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("X"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The Y swizzle.
        /// </summary>
        public Integerx1 Y
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("Y"));
                op.BindInputs(pin);
                return new Integerx1(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("Y"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XX swizzle.
        /// </summary>
        public Integerx2 XX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XX"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XY swizzle.
        /// </summary>
        public Integerx2 XY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XY"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YX swizzle.
        /// </summary>
        public Integerx2 YX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YX"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YY swizzle.
        /// </summary>
        public Integerx2 YY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YY"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXX swizzle.
        /// </summary>
        public Integerx3 XXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXY swizzle.
        /// </summary>
        public Integerx3 XXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYX swizzle.
        /// </summary>
        public Integerx3 XYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYY swizzle.
        /// </summary>
        public Integerx3 XYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXX swizzle.
        /// </summary>
        public Integerx3 YXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXY swizzle.
        /// </summary>
        public Integerx3 YXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYX swizzle.
        /// </summary>
        public Integerx3 YYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYY swizzle.
        /// </summary>
        public Integerx3 YYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXXX swizzle.
        /// </summary>
        public Integerx4 XXXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXXY swizzle.
        /// </summary>
        public Integerx4 XXXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXYX swizzle.
        /// </summary>
        public Integerx4 XXYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXYY swizzle.
        /// </summary>
        public Integerx4 XXYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYXX swizzle.
        /// </summary>
        public Integerx4 XYXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYXY swizzle.
        /// </summary>
        public Integerx4 XYXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYYX swizzle.
        /// </summary>
        public Integerx4 XYYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYYY swizzle.
        /// </summary>
        public Integerx4 XYYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXXX swizzle.
        /// </summary>
        public Integerx4 YXXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXXY swizzle.
        /// </summary>
        public Integerx4 YXXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXYX swizzle.
        /// </summary>
        public Integerx4 YXYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXYY swizzle.
        /// </summary>
        public Integerx4 YXYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYXX swizzle.
        /// </summary>
        public Integerx4 YYXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYXY swizzle.
        /// </summary>
        public Integerx4 YYXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYYX swizzle.
        /// </summary>
        public Integerx4 YYYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYYY swizzle.
        /// </summary>
        public Integerx4 YYYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }


    }

    public sealed partial class Integerx3
    {
        /// <summary>
        /// The X swizzle.
        /// </summary>
        public Integerx1 X
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("X"));
                op.BindInputs(pin);
                return new Integerx1(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("X"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The Y swizzle.
        /// </summary>
        public Integerx1 Y
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("Y"));
                op.BindInputs(pin);
                return new Integerx1(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("Y"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The Z swizzle.
        /// </summary>
        public Integerx1 Z
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("Z"));
                op.BindInputs(pin);
                return new Integerx1(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("Z"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XX swizzle.
        /// </summary>
        public Integerx2 XX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XX"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XY swizzle.
        /// </summary>
        public Integerx2 XY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XY"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZ swizzle.
        /// </summary>
        public Integerx2 XZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZ"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YX swizzle.
        /// </summary>
        public Integerx2 YX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YX"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YY swizzle.
        /// </summary>
        public Integerx2 YY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YY"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZ swizzle.
        /// </summary>
        public Integerx2 YZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZ"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZX swizzle.
        /// </summary>
        public Integerx2 ZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZX"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZY swizzle.
        /// </summary>
        public Integerx2 ZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZY"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZ swizzle.
        /// </summary>
        public Integerx2 ZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZ"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXX swizzle.
        /// </summary>
        public Integerx3 XXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXY swizzle.
        /// </summary>
        public Integerx3 XXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXZ swizzle.
        /// </summary>
        public Integerx3 XXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYX swizzle.
        /// </summary>
        public Integerx3 XYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYY swizzle.
        /// </summary>
        public Integerx3 XYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYZ swizzle.
        /// </summary>
        public Integerx3 XYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZX swizzle.
        /// </summary>
        public Integerx3 XZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZY swizzle.
        /// </summary>
        public Integerx3 XZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZZ swizzle.
        /// </summary>
        public Integerx3 XZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXX swizzle.
        /// </summary>
        public Integerx3 YXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXY swizzle.
        /// </summary>
        public Integerx3 YXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXZ swizzle.
        /// </summary>
        public Integerx3 YXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYX swizzle.
        /// </summary>
        public Integerx3 YYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYY swizzle.
        /// </summary>
        public Integerx3 YYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYZ swizzle.
        /// </summary>
        public Integerx3 YYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZX swizzle.
        /// </summary>
        public Integerx3 YZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZY swizzle.
        /// </summary>
        public Integerx3 YZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZZ swizzle.
        /// </summary>
        public Integerx3 YZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXX swizzle.
        /// </summary>
        public Integerx3 ZXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXY swizzle.
        /// </summary>
        public Integerx3 ZXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXZ swizzle.
        /// </summary>
        public Integerx3 ZXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYX swizzle.
        /// </summary>
        public Integerx3 ZYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYY swizzle.
        /// </summary>
        public Integerx3 ZYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYZ swizzle.
        /// </summary>
        public Integerx3 ZYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZX swizzle.
        /// </summary>
        public Integerx3 ZZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZY swizzle.
        /// </summary>
        public Integerx3 ZZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZZ swizzle.
        /// </summary>
        public Integerx3 ZZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXXX swizzle.
        /// </summary>
        public Integerx4 XXXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXXY swizzle.
        /// </summary>
        public Integerx4 XXXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXXZ swizzle.
        /// </summary>
        public Integerx4 XXXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXYX swizzle.
        /// </summary>
        public Integerx4 XXYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXYY swizzle.
        /// </summary>
        public Integerx4 XXYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXYZ swizzle.
        /// </summary>
        public Integerx4 XXYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXZX swizzle.
        /// </summary>
        public Integerx4 XXZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXZY swizzle.
        /// </summary>
        public Integerx4 XXZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXZZ swizzle.
        /// </summary>
        public Integerx4 XXZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYXX swizzle.
        /// </summary>
        public Integerx4 XYXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYXY swizzle.
        /// </summary>
        public Integerx4 XYXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYXZ swizzle.
        /// </summary>
        public Integerx4 XYXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYYX swizzle.
        /// </summary>
        public Integerx4 XYYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYYY swizzle.
        /// </summary>
        public Integerx4 XYYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYYZ swizzle.
        /// </summary>
        public Integerx4 XYYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYZX swizzle.
        /// </summary>
        public Integerx4 XYZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYZY swizzle.
        /// </summary>
        public Integerx4 XYZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYZZ swizzle.
        /// </summary>
        public Integerx4 XYZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZXX swizzle.
        /// </summary>
        public Integerx4 XZXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZXY swizzle.
        /// </summary>
        public Integerx4 XZXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZXZ swizzle.
        /// </summary>
        public Integerx4 XZXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZYX swizzle.
        /// </summary>
        public Integerx4 XZYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZYY swizzle.
        /// </summary>
        public Integerx4 XZYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZYZ swizzle.
        /// </summary>
        public Integerx4 XZYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZZX swizzle.
        /// </summary>
        public Integerx4 XZZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZZY swizzle.
        /// </summary>
        public Integerx4 XZZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZZZ swizzle.
        /// </summary>
        public Integerx4 XZZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXXX swizzle.
        /// </summary>
        public Integerx4 YXXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXXY swizzle.
        /// </summary>
        public Integerx4 YXXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXXZ swizzle.
        /// </summary>
        public Integerx4 YXXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXYX swizzle.
        /// </summary>
        public Integerx4 YXYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXYY swizzle.
        /// </summary>
        public Integerx4 YXYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXYZ swizzle.
        /// </summary>
        public Integerx4 YXYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXZX swizzle.
        /// </summary>
        public Integerx4 YXZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXZY swizzle.
        /// </summary>
        public Integerx4 YXZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXZZ swizzle.
        /// </summary>
        public Integerx4 YXZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYXX swizzle.
        /// </summary>
        public Integerx4 YYXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYXY swizzle.
        /// </summary>
        public Integerx4 YYXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYXZ swizzle.
        /// </summary>
        public Integerx4 YYXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYYX swizzle.
        /// </summary>
        public Integerx4 YYYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYYY swizzle.
        /// </summary>
        public Integerx4 YYYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYYZ swizzle.
        /// </summary>
        public Integerx4 YYYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYZX swizzle.
        /// </summary>
        public Integerx4 YYZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYZY swizzle.
        /// </summary>
        public Integerx4 YYZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYZZ swizzle.
        /// </summary>
        public Integerx4 YYZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZXX swizzle.
        /// </summary>
        public Integerx4 YZXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZXY swizzle.
        /// </summary>
        public Integerx4 YZXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZXZ swizzle.
        /// </summary>
        public Integerx4 YZXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZYX swizzle.
        /// </summary>
        public Integerx4 YZYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZYY swizzle.
        /// </summary>
        public Integerx4 YZYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZYZ swizzle.
        /// </summary>
        public Integerx4 YZYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZZX swizzle.
        /// </summary>
        public Integerx4 YZZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZZY swizzle.
        /// </summary>
        public Integerx4 YZZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZZZ swizzle.
        /// </summary>
        public Integerx4 YZZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXXX swizzle.
        /// </summary>
        public Integerx4 ZXXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXXY swizzle.
        /// </summary>
        public Integerx4 ZXXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXXZ swizzle.
        /// </summary>
        public Integerx4 ZXXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXYX swizzle.
        /// </summary>
        public Integerx4 ZXYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXYY swizzle.
        /// </summary>
        public Integerx4 ZXYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXYZ swizzle.
        /// </summary>
        public Integerx4 ZXYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXZX swizzle.
        /// </summary>
        public Integerx4 ZXZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXZY swizzle.
        /// </summary>
        public Integerx4 ZXZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXZZ swizzle.
        /// </summary>
        public Integerx4 ZXZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYXX swizzle.
        /// </summary>
        public Integerx4 ZYXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYXY swizzle.
        /// </summary>
        public Integerx4 ZYXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYXZ swizzle.
        /// </summary>
        public Integerx4 ZYXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYYX swizzle.
        /// </summary>
        public Integerx4 ZYYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYYY swizzle.
        /// </summary>
        public Integerx4 ZYYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYYZ swizzle.
        /// </summary>
        public Integerx4 ZYYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYZX swizzle.
        /// </summary>
        public Integerx4 ZYZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYZY swizzle.
        /// </summary>
        public Integerx4 ZYZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYZZ swizzle.
        /// </summary>
        public Integerx4 ZYZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZXX swizzle.
        /// </summary>
        public Integerx4 ZZXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZXY swizzle.
        /// </summary>
        public Integerx4 ZZXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZXZ swizzle.
        /// </summary>
        public Integerx4 ZZXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZYX swizzle.
        /// </summary>
        public Integerx4 ZZYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZYY swizzle.
        /// </summary>
        public Integerx4 ZZYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZYZ swizzle.
        /// </summary>
        public Integerx4 ZZYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZZX swizzle.
        /// </summary>
        public Integerx4 ZZZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZZY swizzle.
        /// </summary>
        public Integerx4 ZZZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZZZ swizzle.
        /// </summary>
        public Integerx4 ZZZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }


    }

    public sealed partial class Integerx4
    {
        /// <summary>
        /// The X swizzle.
        /// </summary>
        public Integerx1 X
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("X"));
                op.BindInputs(pin);
                return new Integerx1(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("X"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The Y swizzle.
        /// </summary>
        public Integerx1 Y
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("Y"));
                op.BindInputs(pin);
                return new Integerx1(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("Y"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The Z swizzle.
        /// </summary>
        public Integerx1 Z
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("Z"));
                op.BindInputs(pin);
                return new Integerx1(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("Z"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The W swizzle.
        /// </summary>
        public Integerx1 W
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("W"));
                op.BindInputs(pin);
                return new Integerx1(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("W"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XX swizzle.
        /// </summary>
        public Integerx2 XX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XX"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XY swizzle.
        /// </summary>
        public Integerx2 XY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XY"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZ swizzle.
        /// </summary>
        public Integerx2 XZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZ"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XW swizzle.
        /// </summary>
        public Integerx2 XW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XW"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YX swizzle.
        /// </summary>
        public Integerx2 YX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YX"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YY swizzle.
        /// </summary>
        public Integerx2 YY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YY"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZ swizzle.
        /// </summary>
        public Integerx2 YZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZ"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YW swizzle.
        /// </summary>
        public Integerx2 YW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YW"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZX swizzle.
        /// </summary>
        public Integerx2 ZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZX"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZY swizzle.
        /// </summary>
        public Integerx2 ZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZY"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZ swizzle.
        /// </summary>
        public Integerx2 ZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZ"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZW swizzle.
        /// </summary>
        public Integerx2 ZW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZW"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WX swizzle.
        /// </summary>
        public Integerx2 WX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WX"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WY swizzle.
        /// </summary>
        public Integerx2 WY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WY"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WZ swizzle.
        /// </summary>
        public Integerx2 WZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WZ"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WW swizzle.
        /// </summary>
        public Integerx2 WW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WW"));
                op.BindInputs(pin);
                return new Integerx2(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXX swizzle.
        /// </summary>
        public Integerx3 XXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXY swizzle.
        /// </summary>
        public Integerx3 XXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXZ swizzle.
        /// </summary>
        public Integerx3 XXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXW swizzle.
        /// </summary>
        public Integerx3 XXW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXW"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYX swizzle.
        /// </summary>
        public Integerx3 XYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYY swizzle.
        /// </summary>
        public Integerx3 XYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYZ swizzle.
        /// </summary>
        public Integerx3 XYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYW swizzle.
        /// </summary>
        public Integerx3 XYW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYW"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZX swizzle.
        /// </summary>
        public Integerx3 XZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZY swizzle.
        /// </summary>
        public Integerx3 XZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZZ swizzle.
        /// </summary>
        public Integerx3 XZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZW swizzle.
        /// </summary>
        public Integerx3 XZW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZW"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XWX swizzle.
        /// </summary>
        public Integerx3 XWX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XWX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XWX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XWY swizzle.
        /// </summary>
        public Integerx3 XWY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XWY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XWY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XWZ swizzle.
        /// </summary>
        public Integerx3 XWZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XWZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XWZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XWW swizzle.
        /// </summary>
        public Integerx3 XWW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XWW"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XWW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXX swizzle.
        /// </summary>
        public Integerx3 YXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXY swizzle.
        /// </summary>
        public Integerx3 YXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXZ swizzle.
        /// </summary>
        public Integerx3 YXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXW swizzle.
        /// </summary>
        public Integerx3 YXW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXW"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYX swizzle.
        /// </summary>
        public Integerx3 YYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYY swizzle.
        /// </summary>
        public Integerx3 YYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYZ swizzle.
        /// </summary>
        public Integerx3 YYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYW swizzle.
        /// </summary>
        public Integerx3 YYW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYW"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZX swizzle.
        /// </summary>
        public Integerx3 YZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZY swizzle.
        /// </summary>
        public Integerx3 YZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZZ swizzle.
        /// </summary>
        public Integerx3 YZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZW swizzle.
        /// </summary>
        public Integerx3 YZW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZW"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YWX swizzle.
        /// </summary>
        public Integerx3 YWX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YWX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YWX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YWY swizzle.
        /// </summary>
        public Integerx3 YWY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YWY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YWY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YWZ swizzle.
        /// </summary>
        public Integerx3 YWZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YWZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YWZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YWW swizzle.
        /// </summary>
        public Integerx3 YWW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YWW"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YWW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXX swizzle.
        /// </summary>
        public Integerx3 ZXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXY swizzle.
        /// </summary>
        public Integerx3 ZXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXZ swizzle.
        /// </summary>
        public Integerx3 ZXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXW swizzle.
        /// </summary>
        public Integerx3 ZXW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXW"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYX swizzle.
        /// </summary>
        public Integerx3 ZYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYY swizzle.
        /// </summary>
        public Integerx3 ZYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYZ swizzle.
        /// </summary>
        public Integerx3 ZYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYW swizzle.
        /// </summary>
        public Integerx3 ZYW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYW"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZX swizzle.
        /// </summary>
        public Integerx3 ZZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZY swizzle.
        /// </summary>
        public Integerx3 ZZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZZ swizzle.
        /// </summary>
        public Integerx3 ZZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZW swizzle.
        /// </summary>
        public Integerx3 ZZW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZW"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZWX swizzle.
        /// </summary>
        public Integerx3 ZWX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZWX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZWX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZWY swizzle.
        /// </summary>
        public Integerx3 ZWY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZWY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZWY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZWZ swizzle.
        /// </summary>
        public Integerx3 ZWZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZWZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZWZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZWW swizzle.
        /// </summary>
        public Integerx3 ZWW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZWW"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZWW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WXX swizzle.
        /// </summary>
        public Integerx3 WXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WXX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WXY swizzle.
        /// </summary>
        public Integerx3 WXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WXY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WXZ swizzle.
        /// </summary>
        public Integerx3 WXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WXZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WXW swizzle.
        /// </summary>
        public Integerx3 WXW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WXW"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WXW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WYX swizzle.
        /// </summary>
        public Integerx3 WYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WYX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WYY swizzle.
        /// </summary>
        public Integerx3 WYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WYY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WYZ swizzle.
        /// </summary>
        public Integerx3 WYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WYZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WYW swizzle.
        /// </summary>
        public Integerx3 WYW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WYW"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WYW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WZX swizzle.
        /// </summary>
        public Integerx3 WZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WZX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WZY swizzle.
        /// </summary>
        public Integerx3 WZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WZY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WZZ swizzle.
        /// </summary>
        public Integerx3 WZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WZZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WZW swizzle.
        /// </summary>
        public Integerx3 WZW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WZW"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WZW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WWX swizzle.
        /// </summary>
        public Integerx3 WWX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WWX"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WWX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WWY swizzle.
        /// </summary>
        public Integerx3 WWY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WWY"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WWY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WWZ swizzle.
        /// </summary>
        public Integerx3 WWZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WWZ"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WWZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WWW swizzle.
        /// </summary>
        public Integerx3 WWW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WWW"));
                op.BindInputs(pin);
                return new Integerx3(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WWW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXXX swizzle.
        /// </summary>
        public Integerx4 XXXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXXY swizzle.
        /// </summary>
        public Integerx4 XXXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXXZ swizzle.
        /// </summary>
        public Integerx4 XXXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXXW swizzle.
        /// </summary>
        public Integerx4 XXXW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXXW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXXW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXYX swizzle.
        /// </summary>
        public Integerx4 XXYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXYY swizzle.
        /// </summary>
        public Integerx4 XXYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXYZ swizzle.
        /// </summary>
        public Integerx4 XXYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXYW swizzle.
        /// </summary>
        public Integerx4 XXYW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXYW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXYW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXZX swizzle.
        /// </summary>
        public Integerx4 XXZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXZY swizzle.
        /// </summary>
        public Integerx4 XXZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXZZ swizzle.
        /// </summary>
        public Integerx4 XXZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXZW swizzle.
        /// </summary>
        public Integerx4 XXZW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXZW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXZW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXWX swizzle.
        /// </summary>
        public Integerx4 XXWX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXWX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXWX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXWY swizzle.
        /// </summary>
        public Integerx4 XXWY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXWY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXWY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXWZ swizzle.
        /// </summary>
        public Integerx4 XXWZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXWZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXWZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XXWW swizzle.
        /// </summary>
        public Integerx4 XXWW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XXWW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XXWW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYXX swizzle.
        /// </summary>
        public Integerx4 XYXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYXY swizzle.
        /// </summary>
        public Integerx4 XYXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYXZ swizzle.
        /// </summary>
        public Integerx4 XYXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYXW swizzle.
        /// </summary>
        public Integerx4 XYXW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYXW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYXW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYYX swizzle.
        /// </summary>
        public Integerx4 XYYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYYY swizzle.
        /// </summary>
        public Integerx4 XYYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYYZ swizzle.
        /// </summary>
        public Integerx4 XYYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYYW swizzle.
        /// </summary>
        public Integerx4 XYYW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYYW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYYW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYZX swizzle.
        /// </summary>
        public Integerx4 XYZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYZY swizzle.
        /// </summary>
        public Integerx4 XYZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYZZ swizzle.
        /// </summary>
        public Integerx4 XYZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYZW swizzle.
        /// </summary>
        public Integerx4 XYZW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYZW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYZW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYWX swizzle.
        /// </summary>
        public Integerx4 XYWX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYWX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYWX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYWY swizzle.
        /// </summary>
        public Integerx4 XYWY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYWY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYWY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYWZ swizzle.
        /// </summary>
        public Integerx4 XYWZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYWZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYWZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XYWW swizzle.
        /// </summary>
        public Integerx4 XYWW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XYWW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XYWW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZXX swizzle.
        /// </summary>
        public Integerx4 XZXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZXY swizzle.
        /// </summary>
        public Integerx4 XZXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZXZ swizzle.
        /// </summary>
        public Integerx4 XZXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZXW swizzle.
        /// </summary>
        public Integerx4 XZXW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZXW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZXW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZYX swizzle.
        /// </summary>
        public Integerx4 XZYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZYY swizzle.
        /// </summary>
        public Integerx4 XZYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZYZ swizzle.
        /// </summary>
        public Integerx4 XZYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZYW swizzle.
        /// </summary>
        public Integerx4 XZYW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZYW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZYW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZZX swizzle.
        /// </summary>
        public Integerx4 XZZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZZY swizzle.
        /// </summary>
        public Integerx4 XZZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZZZ swizzle.
        /// </summary>
        public Integerx4 XZZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZZW swizzle.
        /// </summary>
        public Integerx4 XZZW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZZW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZZW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZWX swizzle.
        /// </summary>
        public Integerx4 XZWX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZWX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZWX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZWY swizzle.
        /// </summary>
        public Integerx4 XZWY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZWY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZWY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZWZ swizzle.
        /// </summary>
        public Integerx4 XZWZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZWZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZWZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XZWW swizzle.
        /// </summary>
        public Integerx4 XZWW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XZWW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XZWW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XWXX swizzle.
        /// </summary>
        public Integerx4 XWXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XWXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XWXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XWXY swizzle.
        /// </summary>
        public Integerx4 XWXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XWXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XWXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XWXZ swizzle.
        /// </summary>
        public Integerx4 XWXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XWXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XWXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XWXW swizzle.
        /// </summary>
        public Integerx4 XWXW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XWXW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XWXW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XWYX swizzle.
        /// </summary>
        public Integerx4 XWYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XWYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XWYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XWYY swizzle.
        /// </summary>
        public Integerx4 XWYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XWYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XWYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XWYZ swizzle.
        /// </summary>
        public Integerx4 XWYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XWYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XWYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XWYW swizzle.
        /// </summary>
        public Integerx4 XWYW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XWYW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XWYW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XWZX swizzle.
        /// </summary>
        public Integerx4 XWZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XWZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XWZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XWZY swizzle.
        /// </summary>
        public Integerx4 XWZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XWZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XWZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XWZZ swizzle.
        /// </summary>
        public Integerx4 XWZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XWZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XWZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XWZW swizzle.
        /// </summary>
        public Integerx4 XWZW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XWZW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XWZW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XWWX swizzle.
        /// </summary>
        public Integerx4 XWWX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XWWX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XWWX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XWWY swizzle.
        /// </summary>
        public Integerx4 XWWY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XWWY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XWWY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XWWZ swizzle.
        /// </summary>
        public Integerx4 XWWZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XWWZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XWWZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The XWWW swizzle.
        /// </summary>
        public Integerx4 XWWW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("XWWW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("XWWW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXXX swizzle.
        /// </summary>
        public Integerx4 YXXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXXY swizzle.
        /// </summary>
        public Integerx4 YXXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXXZ swizzle.
        /// </summary>
        public Integerx4 YXXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXXW swizzle.
        /// </summary>
        public Integerx4 YXXW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXXW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXXW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXYX swizzle.
        /// </summary>
        public Integerx4 YXYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXYY swizzle.
        /// </summary>
        public Integerx4 YXYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXYZ swizzle.
        /// </summary>
        public Integerx4 YXYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXYW swizzle.
        /// </summary>
        public Integerx4 YXYW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXYW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXYW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXZX swizzle.
        /// </summary>
        public Integerx4 YXZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXZY swizzle.
        /// </summary>
        public Integerx4 YXZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXZZ swizzle.
        /// </summary>
        public Integerx4 YXZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXZW swizzle.
        /// </summary>
        public Integerx4 YXZW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXZW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXZW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXWX swizzle.
        /// </summary>
        public Integerx4 YXWX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXWX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXWX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXWY swizzle.
        /// </summary>
        public Integerx4 YXWY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXWY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXWY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXWZ swizzle.
        /// </summary>
        public Integerx4 YXWZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXWZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXWZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YXWW swizzle.
        /// </summary>
        public Integerx4 YXWW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YXWW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YXWW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYXX swizzle.
        /// </summary>
        public Integerx4 YYXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYXY swizzle.
        /// </summary>
        public Integerx4 YYXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYXZ swizzle.
        /// </summary>
        public Integerx4 YYXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYXW swizzle.
        /// </summary>
        public Integerx4 YYXW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYXW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYXW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYYX swizzle.
        /// </summary>
        public Integerx4 YYYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYYY swizzle.
        /// </summary>
        public Integerx4 YYYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYYZ swizzle.
        /// </summary>
        public Integerx4 YYYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYYW swizzle.
        /// </summary>
        public Integerx4 YYYW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYYW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYYW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYZX swizzle.
        /// </summary>
        public Integerx4 YYZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYZY swizzle.
        /// </summary>
        public Integerx4 YYZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYZZ swizzle.
        /// </summary>
        public Integerx4 YYZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYZW swizzle.
        /// </summary>
        public Integerx4 YYZW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYZW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYZW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYWX swizzle.
        /// </summary>
        public Integerx4 YYWX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYWX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYWX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYWY swizzle.
        /// </summary>
        public Integerx4 YYWY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYWY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYWY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYWZ swizzle.
        /// </summary>
        public Integerx4 YYWZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYWZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYWZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YYWW swizzle.
        /// </summary>
        public Integerx4 YYWW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YYWW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YYWW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZXX swizzle.
        /// </summary>
        public Integerx4 YZXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZXY swizzle.
        /// </summary>
        public Integerx4 YZXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZXZ swizzle.
        /// </summary>
        public Integerx4 YZXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZXW swizzle.
        /// </summary>
        public Integerx4 YZXW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZXW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZXW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZYX swizzle.
        /// </summary>
        public Integerx4 YZYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZYY swizzle.
        /// </summary>
        public Integerx4 YZYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZYZ swizzle.
        /// </summary>
        public Integerx4 YZYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZYW swizzle.
        /// </summary>
        public Integerx4 YZYW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZYW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZYW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZZX swizzle.
        /// </summary>
        public Integerx4 YZZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZZY swizzle.
        /// </summary>
        public Integerx4 YZZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZZZ swizzle.
        /// </summary>
        public Integerx4 YZZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZZW swizzle.
        /// </summary>
        public Integerx4 YZZW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZZW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZZW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZWX swizzle.
        /// </summary>
        public Integerx4 YZWX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZWX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZWX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZWY swizzle.
        /// </summary>
        public Integerx4 YZWY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZWY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZWY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZWZ swizzle.
        /// </summary>
        public Integerx4 YZWZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZWZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZWZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YZWW swizzle.
        /// </summary>
        public Integerx4 YZWW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YZWW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YZWW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YWXX swizzle.
        /// </summary>
        public Integerx4 YWXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YWXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YWXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YWXY swizzle.
        /// </summary>
        public Integerx4 YWXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YWXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YWXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YWXZ swizzle.
        /// </summary>
        public Integerx4 YWXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YWXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YWXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YWXW swizzle.
        /// </summary>
        public Integerx4 YWXW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YWXW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YWXW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YWYX swizzle.
        /// </summary>
        public Integerx4 YWYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YWYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YWYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YWYY swizzle.
        /// </summary>
        public Integerx4 YWYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YWYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YWYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YWYZ swizzle.
        /// </summary>
        public Integerx4 YWYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YWYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YWYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YWYW swizzle.
        /// </summary>
        public Integerx4 YWYW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YWYW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YWYW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YWZX swizzle.
        /// </summary>
        public Integerx4 YWZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YWZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YWZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YWZY swizzle.
        /// </summary>
        public Integerx4 YWZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YWZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YWZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YWZZ swizzle.
        /// </summary>
        public Integerx4 YWZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YWZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YWZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YWZW swizzle.
        /// </summary>
        public Integerx4 YWZW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YWZW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YWZW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YWWX swizzle.
        /// </summary>
        public Integerx4 YWWX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YWWX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YWWX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YWWY swizzle.
        /// </summary>
        public Integerx4 YWWY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YWWY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YWWY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YWWZ swizzle.
        /// </summary>
        public Integerx4 YWWZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YWWZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YWWZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The YWWW swizzle.
        /// </summary>
        public Integerx4 YWWW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("YWWW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("YWWW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXXX swizzle.
        /// </summary>
        public Integerx4 ZXXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXXY swizzle.
        /// </summary>
        public Integerx4 ZXXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXXZ swizzle.
        /// </summary>
        public Integerx4 ZXXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXXW swizzle.
        /// </summary>
        public Integerx4 ZXXW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXXW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXXW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXYX swizzle.
        /// </summary>
        public Integerx4 ZXYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXYY swizzle.
        /// </summary>
        public Integerx4 ZXYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXYZ swizzle.
        /// </summary>
        public Integerx4 ZXYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXYW swizzle.
        /// </summary>
        public Integerx4 ZXYW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXYW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXYW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXZX swizzle.
        /// </summary>
        public Integerx4 ZXZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXZY swizzle.
        /// </summary>
        public Integerx4 ZXZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXZZ swizzle.
        /// </summary>
        public Integerx4 ZXZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXZW swizzle.
        /// </summary>
        public Integerx4 ZXZW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXZW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXZW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXWX swizzle.
        /// </summary>
        public Integerx4 ZXWX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXWX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXWX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXWY swizzle.
        /// </summary>
        public Integerx4 ZXWY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXWY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXWY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXWZ swizzle.
        /// </summary>
        public Integerx4 ZXWZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXWZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXWZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZXWW swizzle.
        /// </summary>
        public Integerx4 ZXWW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZXWW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZXWW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYXX swizzle.
        /// </summary>
        public Integerx4 ZYXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYXY swizzle.
        /// </summary>
        public Integerx4 ZYXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYXZ swizzle.
        /// </summary>
        public Integerx4 ZYXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYXW swizzle.
        /// </summary>
        public Integerx4 ZYXW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYXW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYXW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYYX swizzle.
        /// </summary>
        public Integerx4 ZYYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYYY swizzle.
        /// </summary>
        public Integerx4 ZYYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYYZ swizzle.
        /// </summary>
        public Integerx4 ZYYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYYW swizzle.
        /// </summary>
        public Integerx4 ZYYW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYYW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYYW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYZX swizzle.
        /// </summary>
        public Integerx4 ZYZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYZY swizzle.
        /// </summary>
        public Integerx4 ZYZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYZZ swizzle.
        /// </summary>
        public Integerx4 ZYZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYZW swizzle.
        /// </summary>
        public Integerx4 ZYZW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYZW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYZW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYWX swizzle.
        /// </summary>
        public Integerx4 ZYWX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYWX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYWX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYWY swizzle.
        /// </summary>
        public Integerx4 ZYWY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYWY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYWY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYWZ swizzle.
        /// </summary>
        public Integerx4 ZYWZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYWZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYWZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZYWW swizzle.
        /// </summary>
        public Integerx4 ZYWW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZYWW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZYWW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZXX swizzle.
        /// </summary>
        public Integerx4 ZZXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZXY swizzle.
        /// </summary>
        public Integerx4 ZZXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZXZ swizzle.
        /// </summary>
        public Integerx4 ZZXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZXW swizzle.
        /// </summary>
        public Integerx4 ZZXW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZXW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZXW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZYX swizzle.
        /// </summary>
        public Integerx4 ZZYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZYY swizzle.
        /// </summary>
        public Integerx4 ZZYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZYZ swizzle.
        /// </summary>
        public Integerx4 ZZYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZYW swizzle.
        /// </summary>
        public Integerx4 ZZYW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZYW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZYW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZZX swizzle.
        /// </summary>
        public Integerx4 ZZZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZZY swizzle.
        /// </summary>
        public Integerx4 ZZZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZZZ swizzle.
        /// </summary>
        public Integerx4 ZZZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZZW swizzle.
        /// </summary>
        public Integerx4 ZZZW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZZW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZZW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZWX swizzle.
        /// </summary>
        public Integerx4 ZZWX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZWX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZWX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZWY swizzle.
        /// </summary>
        public Integerx4 ZZWY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZWY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZWY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZWZ swizzle.
        /// </summary>
        public Integerx4 ZZWZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZWZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZWZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZZWW swizzle.
        /// </summary>
        public Integerx4 ZZWW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZZWW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZZWW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZWXX swizzle.
        /// </summary>
        public Integerx4 ZWXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZWXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZWXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZWXY swizzle.
        /// </summary>
        public Integerx4 ZWXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZWXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZWXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZWXZ swizzle.
        /// </summary>
        public Integerx4 ZWXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZWXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZWXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZWXW swizzle.
        /// </summary>
        public Integerx4 ZWXW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZWXW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZWXW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZWYX swizzle.
        /// </summary>
        public Integerx4 ZWYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZWYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZWYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZWYY swizzle.
        /// </summary>
        public Integerx4 ZWYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZWYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZWYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZWYZ swizzle.
        /// </summary>
        public Integerx4 ZWYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZWYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZWYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZWYW swizzle.
        /// </summary>
        public Integerx4 ZWYW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZWYW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZWYW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZWZX swizzle.
        /// </summary>
        public Integerx4 ZWZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZWZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZWZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZWZY swizzle.
        /// </summary>
        public Integerx4 ZWZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZWZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZWZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZWZZ swizzle.
        /// </summary>
        public Integerx4 ZWZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZWZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZWZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZWZW swizzle.
        /// </summary>
        public Integerx4 ZWZW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZWZW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZWZW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZWWX swizzle.
        /// </summary>
        public Integerx4 ZWWX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZWWX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZWWX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZWWY swizzle.
        /// </summary>
        public Integerx4 ZWWY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZWWY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZWWY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZWWZ swizzle.
        /// </summary>
        public Integerx4 ZWWZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZWWZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZWWZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The ZWWW swizzle.
        /// </summary>
        public Integerx4 ZWWW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("ZWWW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("ZWWW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WXXX swizzle.
        /// </summary>
        public Integerx4 WXXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WXXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WXXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WXXY swizzle.
        /// </summary>
        public Integerx4 WXXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WXXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WXXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WXXZ swizzle.
        /// </summary>
        public Integerx4 WXXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WXXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WXXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WXXW swizzle.
        /// </summary>
        public Integerx4 WXXW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WXXW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WXXW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WXYX swizzle.
        /// </summary>
        public Integerx4 WXYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WXYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WXYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WXYY swizzle.
        /// </summary>
        public Integerx4 WXYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WXYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WXYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WXYZ swizzle.
        /// </summary>
        public Integerx4 WXYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WXYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WXYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WXYW swizzle.
        /// </summary>
        public Integerx4 WXYW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WXYW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WXYW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WXZX swizzle.
        /// </summary>
        public Integerx4 WXZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WXZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WXZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WXZY swizzle.
        /// </summary>
        public Integerx4 WXZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WXZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WXZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WXZZ swizzle.
        /// </summary>
        public Integerx4 WXZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WXZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WXZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WXZW swizzle.
        /// </summary>
        public Integerx4 WXZW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WXZW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WXZW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WXWX swizzle.
        /// </summary>
        public Integerx4 WXWX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WXWX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WXWX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WXWY swizzle.
        /// </summary>
        public Integerx4 WXWY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WXWY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WXWY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WXWZ swizzle.
        /// </summary>
        public Integerx4 WXWZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WXWZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WXWZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WXWW swizzle.
        /// </summary>
        public Integerx4 WXWW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WXWW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WXWW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WYXX swizzle.
        /// </summary>
        public Integerx4 WYXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WYXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WYXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WYXY swizzle.
        /// </summary>
        public Integerx4 WYXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WYXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WYXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WYXZ swizzle.
        /// </summary>
        public Integerx4 WYXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WYXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WYXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WYXW swizzle.
        /// </summary>
        public Integerx4 WYXW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WYXW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WYXW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WYYX swizzle.
        /// </summary>
        public Integerx4 WYYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WYYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WYYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WYYY swizzle.
        /// </summary>
        public Integerx4 WYYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WYYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WYYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WYYZ swizzle.
        /// </summary>
        public Integerx4 WYYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WYYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WYYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WYYW swizzle.
        /// </summary>
        public Integerx4 WYYW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WYYW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WYYW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WYZX swizzle.
        /// </summary>
        public Integerx4 WYZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WYZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WYZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WYZY swizzle.
        /// </summary>
        public Integerx4 WYZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WYZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WYZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WYZZ swizzle.
        /// </summary>
        public Integerx4 WYZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WYZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WYZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WYZW swizzle.
        /// </summary>
        public Integerx4 WYZW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WYZW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WYZW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WYWX swizzle.
        /// </summary>
        public Integerx4 WYWX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WYWX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WYWX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WYWY swizzle.
        /// </summary>
        public Integerx4 WYWY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WYWY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WYWY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WYWZ swizzle.
        /// </summary>
        public Integerx4 WYWZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WYWZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WYWZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WYWW swizzle.
        /// </summary>
        public Integerx4 WYWW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WYWW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WYWW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WZXX swizzle.
        /// </summary>
        public Integerx4 WZXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WZXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WZXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WZXY swizzle.
        /// </summary>
        public Integerx4 WZXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WZXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WZXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WZXZ swizzle.
        /// </summary>
        public Integerx4 WZXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WZXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WZXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WZXW swizzle.
        /// </summary>
        public Integerx4 WZXW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WZXW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WZXW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WZYX swizzle.
        /// </summary>
        public Integerx4 WZYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WZYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WZYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WZYY swizzle.
        /// </summary>
        public Integerx4 WZYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WZYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WZYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WZYZ swizzle.
        /// </summary>
        public Integerx4 WZYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WZYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WZYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WZYW swizzle.
        /// </summary>
        public Integerx4 WZYW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WZYW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WZYW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WZZX swizzle.
        /// </summary>
        public Integerx4 WZZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WZZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WZZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WZZY swizzle.
        /// </summary>
        public Integerx4 WZZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WZZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WZZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WZZZ swizzle.
        /// </summary>
        public Integerx4 WZZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WZZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WZZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WZZW swizzle.
        /// </summary>
        public Integerx4 WZZW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WZZW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WZZW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WZWX swizzle.
        /// </summary>
        public Integerx4 WZWX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WZWX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WZWX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WZWY swizzle.
        /// </summary>
        public Integerx4 WZWY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WZWY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WZWY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WZWZ swizzle.
        /// </summary>
        public Integerx4 WZWZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WZWZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WZWZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WZWW swizzle.
        /// </summary>
        public Integerx4 WZWW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WZWW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WZWW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WWXX swizzle.
        /// </summary>
        public Integerx4 WWXX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WWXX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WWXX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WWXY swizzle.
        /// </summary>
        public Integerx4 WWXY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WWXY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WWXY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WWXZ swizzle.
        /// </summary>
        public Integerx4 WWXZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WWXZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WWXZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WWXW swizzle.
        /// </summary>
        public Integerx4 WWXW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WWXW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WWXW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WWYX swizzle.
        /// </summary>
        public Integerx4 WWYX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WWYX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WWYX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WWYY swizzle.
        /// </summary>
        public Integerx4 WWYY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WWYY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WWYY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WWYZ swizzle.
        /// </summary>
        public Integerx4 WWYZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WWYZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WWYZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WWYW swizzle.
        /// </summary>
        public Integerx4 WWYW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WWYW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WWYW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WWZX swizzle.
        /// </summary>
        public Integerx4 WWZX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WWZX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WWZX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WWZY swizzle.
        /// </summary>
        public Integerx4 WWZY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WWZY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WWZY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WWZZ swizzle.
        /// </summary>
        public Integerx4 WWZZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WWZZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WWZZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WWZW swizzle.
        /// </summary>
        public Integerx4 WWZW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WWZW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WWZW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WWWX swizzle.
        /// </summary>
        public Integerx4 WWWX
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WWWX"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WWWX"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WWWY swizzle.
        /// </summary>
        public Integerx4 WWWY
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WWWY"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WWWY"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WWWZ swizzle.
        /// </summary>
        public Integerx4 WWWZ
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WWWZ"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WWWZ"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }

        /// <summary>
        /// The WWWW swizzle.
        /// </summary>
        public Integerx4 WWWW
        {
            get
            {
                SwizzleOperation op = new SwizzleOperation(SwizzleMask.Parse("WWWW"));
                op.BindInputs(pin);
                return new Integerx4(op.Outputs[0], Generator);
            }
            [param: NotNull]
            set
            {
                WriteToSwizzledOperation op = new WriteToSwizzledOperation(SwizzleMask.Parse("WWWW"));
                op.BindInputs(pin, value.Pin);
                pin = op.Outputs[0];
            }
        }


    }


}
