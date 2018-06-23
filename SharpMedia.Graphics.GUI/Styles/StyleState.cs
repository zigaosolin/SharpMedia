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

namespace SharpMedia.Graphics.GUI.Styles
{

    /// <summary>
    /// A style state descriptor. Common styles available through CommonStateStyles.
    /// </summary>
    /// <remarks>It is immutable.</remarks>
    public sealed class StyleState : IComparable<StyleState>, IEquatable<StyleState>
    {
        #region Private Members
        string name;
        string description;
        StyleState redirect;
        #endregion

        #region Properties

        /// <summary>
        /// Name of style state.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Description if style state.
        /// </summary>
        public string Description
        {
            get { return description; }
        }

        /// <summary>
        /// The style state it redirects to if not available.
        /// </summary>
        public StyleState Redirect
        {
            get { return redirect; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructors.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="desc"></param>
        /// <param name="redirect"></param>
        public StyleState(string name, string desc, StyleState redirect)
        {
            this.name = name;
            this.description = desc;
            this.redirect = redirect;
        }

        #endregion

        #region IComparable<StyleState> Members

        public int CompareTo(StyleState other)
        {
            return this.Name.CompareTo(other.Name);
        }

        #endregion

        #region IEquatable<StyleState> Members

        public bool Equals(StyleState other)
        {
            return this.Name == other.Name;
        }

        #endregion
    }
}
