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
using SharpMedia.Components.Applications;
using SharpMedia.Database;
using SharpMedia.Components.TextConsole;
using System.IO;
using SharpMedia.Components.Configuration;
using System.Globalization;

namespace SharpMedia.Math
{

    /// <summary>
    /// Can evaluate mathematical string expressions.
    /// </summary>
    /// <remarks>You can write in calculator for: "e^45*cos(12)/(12^2)*(76-12)" or in 
    /// variable form: "e^45*cos(X)/(X^2)*(76-X)" Parameters={X=12}. pi and e are defined constants, as well
    /// as standard functions.</remarks>
    public sealed class EvalApp : Application
    {
        #region Private Members
        FunctionSet functionsSet;

        // Variable values.
        Dictionary<string, double> parameters = new Dictionary<string, double>();
        
        #endregion

        #region Properties

        /// <summary>
        /// The function set used.
        /// </summary>
        public FunctionSet FunctionSet
        {
            get { return functionsSet; }
            set { functionsSet = value; }
        }

        /// <summary>
        /// Gets or sets anonymuse parameters.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public Dictionary<string,double> Parameters
        {
            set
            {
                parameters = value;
            }
            get
            {
                return parameters;
            }
        }

        #endregion

        #region Overrides

        public override int Start(string verb, string[] arguments)
        {
            if (arguments.Length != 1)
            {
                throw new ArgumentException("Invalid arguments, expecting only one argument.");
            }

            
            Expression expression = Expression.Parse(arguments[0]);

            // We set parameters.
            Expression.FunctionParams p = expression.GetParams(false);

            // We apply some defined.
            p["PI"] = MathHelper.PI;
            p["pi"] = MathHelper.PI;
            p["E"] = MathHelper.E;
            p["e"] = MathHelper.E;
            
            foreach (KeyValuePair<string, double> element in parameters)
            {
                p[element.Key] = element.Value;
            }

            // We evaluate function.
            console.Out.WriteLine(expression.Evald(p, functionsSet, 
                typeof(EvalApp).Module).ToString(CultureInfo.InvariantCulture.NumberFormat));

            return 0;
        }

        #endregion
    }
}
