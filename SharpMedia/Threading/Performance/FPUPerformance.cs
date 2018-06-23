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

namespace SharpMedia.Threading.Performance
{
    /// <summary>
    /// Performance of a Floating Point Unit
    /// </summary>
    public sealed class FPUPerformance : IPerformance
    {
        private int matrixMultiplies4;
        private int vectorCrosses4;
        private int matrixMultiplies4dbl;
        private int vectorCrosses4dbl;

        /// <summary>
        /// 4D Matrix multiplies per second 
        /// </summary>
        public int MatrixMultiplies4D
        {
            get { return matrixMultiplies4; }
        }

        /// <summary>
        /// 4D Vector cross products per second
        /// </summary>
        public int VectorCrosses4D
        {
            get { return vectorCrosses4; }
        }

        /// <summary>
        /// 4D Matrix multiplies per second (double precision)
        /// </summary>
        public int DoubleMatrixMultiplies4D
        {
            get { return matrixMultiplies4dbl; }
        }

        /// <summary>
        /// 4D Vector cross products per second (double precision)
        /// </summary>
        public int DoubleVectorCrosses4D
        {
            get { return vectorCrosses4dbl; }
        }

        /// <param name="mm4"><see cref="MatrixMultiplies4D"/></param>
        /// <param name="vc4"><see cref="VectorCrosses4D"/></param>
        /// <param name="mm4d"><see cref="DoubleMatrixMultiplies4D"/></param>
        /// <param name="vc4d"><see cref="DoubleVectorCrosses4D"/></param>
        public FPUPerformance(int mm4, int vc4, int mm4d, int vc4d)
        {
            this.matrixMultiplies4 = mm4;
            this.matrixMultiplies4dbl = mm4d;
            this.vectorCrosses4 = vc4;
            this.vectorCrosses4dbl = vc4d;
        }
    }
}
