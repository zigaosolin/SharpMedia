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
using System.Text.RegularExpressions;

namespace SharpMedia.Graphics.GUI.Validation
{
    /// <summary>
    /// A regex validator.
    /// </summary>
    public sealed class RegexValidator : IValidator<string>
    {
        #region Private Members
        Regex expression;
        string errorMessage;
        #endregion

        #region Public Members

        /// <summary>
        /// Regex validator with standard error message (not recomended).
        /// </summary>
        /// <param name="expression"></param>
        public RegexValidator(Regex expression)
        {
            this.expression = expression;
            this.errorMessage = string.Format("Not matching expression '{0}'", expression.ToString());
        }

        /// <summary>
        /// Regex validator with standard error message (not recomended).
        /// </summary>
        /// <param name="expression"></param>
        public RegexValidator(Regex expression, string errorMessage)
        {
            this.expression = expression;
            this.errorMessage = errorMessage;
        }

        #endregion

        #region IValidator<string> Members

        public ValidationResult IsValid(string obj, out string error)
        {
            if (expression.IsMatch(obj))
            {
                error = null;
                return ValidationResult.Valid;
            }
            else
            {
                error = errorMessage;
                return ValidationResult.NotValid;
            }
        }

        #endregion

        #region IValidator Members

        public bool AllowPartialValidation
        {
            get { return false; }
        }

        public ValidationResult IsValid(object obj, out string error)
        {
            return IsValid(obj as string, out error);
        }

        #endregion
    }
}
