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

namespace SharpMedia.Math.Functions.Windows
{
    /// <summary>
    /// A bartlett window function.
    /// </summary>
    public class BartlettFunction : IWindowFunction
    {
        #region IWindowFunction Members

        public Intervald NonZeroRange
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region IFunction Members

        public Functiond Functiond
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Functionf Functionf
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public double Eval(double x)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
