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

namespace SharpMedia.Graphics.GUI.Validation
{
    /// <summary>
    /// Validation results.
    /// </summary>
    public enum ValidationResult
    {
        NotValid,
        PartlyValid,
        Valid
    }

    /// <summary>
    /// A validator interface.
    /// </summary>
    public interface IValidator
    {

        /// <summary>
        /// Allows partial validation.
        /// </summary>
        /// <remarks>This validates data at all stages, not just on lost focus.</remarks>
        bool AllowPartialValidation {get; }

        /// <summary>
        /// A validator interface.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        ValidationResult IsValid(object obj, out string error);
    }

    /// <summary>
    /// A validator interface.
    /// </summary>
    public interface IValidator<T> : IValidator
    {

        /// <summary>
        /// A generic validator interface.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        ValidationResult IsValid(T obj, out string error);

    }
}
