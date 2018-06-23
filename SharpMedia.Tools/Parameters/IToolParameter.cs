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
using SharpMedia.Components.Configuration;

namespace SharpMedia.Tools.Parameters
{
    /// <summary>
    /// A base class for tool parameter, extracted from class.
    /// </summary>
    public interface IToolParameter
    {
        /// <summary>
        /// The attribute this parameter is bound to.
        /// </summary>
        UIAttribute Attribute { get; }

        /// <summary>
        /// The name of parameter.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Sets a value as string. It is legal to throw exceptions here.
        /// </summary>
        /// <param name="sourceString">The string.</param>
        void Parse(string sourceString);

        /// <summary>Set the parameter to tool directly.</summary>
        /// <remarks>Validity checks are to be placed here, so it is perfectly right
        /// to open resources or contact servers at this point and raise exceptions
        /// if errors occur or values are incorrect</remarks>
        /// <param name="toolDirectory">The tool's directory, where the configuration will occur.</param>
        void Apply(IComponentDirectory toolDirectory);

        /// <summary>
        /// Returns an array of possible values to use as hints to the user.
        /// </summary>
        string[] PossibleValuesHint
        {
            get;
        }

        /// <summary>
        /// Only hinted values are accepted.
        /// </summary>
        bool AcceptsOnlyHintedValues
        {
            get;
        }

        /// <summary>
        /// Is the value set.
        /// </summary>
        bool IsSet
        {
            get;
        }
    }
}
