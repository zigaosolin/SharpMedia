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
    /// This is descriptor for grouping.
    /// </summary>
    public sealed class GroupInfo
    {
        #region Private Members
        TokenId startToken;
        TokenId endToken;
        #endregion

        #region Public Members

        /// <summary>
        /// Constructor.
        /// </summary>
        public GroupInfo(TokenId startToken, TokenId endToken)
        {
            this.startToken = startToken;
            this.endToken = endToken;
        }

        /// <summary>
        /// Start token for group.
        /// </summary>
        public TokenId StartToken
        {
            get { return startToken; }
        }

        /// <summary>
        /// End token for group.
        /// </summary>
        public TokenId EndToken
        {
            get { return endToken; }
        }

        #endregion

    }

    /// <summary>
    /// Token position type.
    /// </summary>
    public enum TokenPosition
    {
        Split,
        Left,
        Right
    }

    /// <summary>
    /// This is descriptor for token information.
    /// </summary>
    public sealed class TokenInfo
    {
        #region Private Members
        TokenId tokenId;
        TokenPosition tokenPosition;
        uint priority;
        #endregion

        #region Properties

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="position"></param>
        /// <param name="priority"></param>
        public TokenInfo(TokenId id, TokenPosition position, uint priority)
        {
            this.tokenId = id;
            this.tokenPosition = position;
            this.priority = priority;
        }

        /// <summary>
        /// Token kind.
        /// </summary>
        public TokenPosition Position
        {
            get { return tokenPosition; }
        }

        /// <summary>
        /// Token type.
        /// </summary>
        public TokenId Token
        {
            get { return tokenId; }
        }

        /// <summary>
        /// Priority of token.
        /// </summary>
        public uint Priority
        {
            get { return priority; }
        }

        #endregion
    }
}
