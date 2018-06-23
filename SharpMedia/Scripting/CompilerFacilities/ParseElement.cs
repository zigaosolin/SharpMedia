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

namespace SharpMedia.Scripting.CompilerFacilities
{

    /// <summary>
    /// Base parse element.
    /// </summary>
    public interface IParseElement
    {
    }

    /// <summary>
    /// A group element.
    /// </summary>
    public sealed class Group : IParseElement
    {
        #region Private Members
        GroupInfo info;
        IParseElement internalElement;
        #endregion

        #region Public Members

        /// <summary>
        /// Constructor.
        /// </summary>
        public Group(GroupInfo info, IParseElement internalElement)
        {
            this.info = info;
            this.internalElement = internalElement;
        }

        /// <summary>
        /// Group info.
        /// </summary>
        public GroupInfo GroupInfo
        {
            get { return info; }
        }

        /// <summary>
        /// Internal element.
        /// </summary>
        public IParseElement InternalElement
        {
            get { return internalElement; }
        }

        #endregion
    }

    /// <summary>
    /// Token on left, with expression on right.
    /// </summary>
    public sealed class LeftToken : IParseElement
    {
        #region Private Members
        TokenInfo info;
        IParseElement internalElement;
        #endregion

        #region Public Members

        /// <summary>
        /// Constructor.
        /// </summary>
        public LeftToken(TokenInfo info, IParseElement internalElement)
        {
            this.info = info;
            this.internalElement = internalElement;
        }

        /// <summary>
        /// Token info.
        /// </summary>
        public TokenInfo TokenInfo
        {
            get { return info; }
        }

        /// <summary>
        /// Internal element.
        /// </summary>
        public IParseElement InternalElement
        {
            get { return internalElement; }
        }

        #endregion
    }

    /// <summary>
    /// A split token, as binary expression.
    /// </summary>
    public sealed class SplitToken : IParseElement
    {
        #region Private Members
        TokenInfo info;
        IParseElement leftElement;
        IParseElement rightElement;
        #endregion

        #region Public Members

        /// <summary>
        /// Constructor.
        /// </summary>
        public SplitToken(TokenInfo info, IParseElement leftElement, IParseElement rightElement)
        {
            this.info = info;
            this.leftElement  = leftElement;
            this.rightElement = rightElement;
        }

        /// <summary>
        /// Token info.
        /// </summary>
        public TokenInfo TokenInfo
        {
            get { return info; }
        }

        /// <summary>
        /// Internal left element.
        /// </summary>
        public IParseElement LeftElement
        {
            get { return leftElement; }
        }

        /// <summary>
        /// Internal right element.
        /// </summary>
        public IParseElement RightElement
        {
            get { return rightElement; }
        }

        #endregion
    }

    /// <summary>
    /// Token on right.
    /// </summary>
    public sealed class RightToken : IParseElement
    {
        #region Private Members
        TokenInfo info;
        IParseElement internalElement;
        #endregion

        #region Public Members

        /// <summary>
        /// Constructor.
        /// </summary>
        public RightToken(TokenInfo info, IParseElement internalElement)
        {
            this.info = info;
            this.internalElement = internalElement;
        }

        /// <summary>
        /// Token info.
        /// </summary>
        public TokenInfo TokenInfo
        {
            get { return info; }
        }

        /// <summary>
        /// Internal element.
        /// </summary>
        public IParseElement InternalElement
        {
            get { return internalElement; }
        }

        #endregion
    }

    /// <summary>
    /// An identifier.
    /// </summary>
    public sealed class Identifier : IParseElement
    {
        #region Private Members
        Token token;
        #endregion

        #region Public Members

        /// <summary>
        /// Identifier.
        /// </summary>
        /// <param name="token"></param>
        public Identifier(Token token)
        {
            this.token = token;
        }

        /// <summary>
        /// A token.
        /// </summary>
        public Token Token
        {
            get { return token; }
        }

        #endregion
    }

    /// <summary>
    /// An element sequence.
    /// </summary>
    public sealed class ElementSequence : IParseElement
    {
        #region Private Members
        IParseElement[] elements;
        #endregion

        #region Public Members

        /// <summary>
        /// Element sequence.
        /// </summary>
        public ElementSequence(params IParseElement[] elements)
        {
            this.elements = elements;
        }

        /// <summary>
        /// The token.
        /// </summary>
        public IParseElement[] Elements
        {
            get { return elements; }
        }

        #endregion
    }
}
