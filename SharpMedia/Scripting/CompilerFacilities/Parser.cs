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
    /// A parser class.
    /// </summary>
    public sealed class Parser
    {
        #region Private Members
        List<Token> tokens;
        TokenInfo[] tokenInfo;
        GroupInfo[] groupInfo;
        #endregion

        #region Private Methods

        Parser()
        {
        }

        /// <summary>
        /// Identifies group.
        /// </summary>
        GroupInfo GetGroup(int start, int end)
        {
            for (int i = 0; i < groupInfo.Length; i++)
            {
                GroupInfo info = groupInfo[i];

                if (tokens[start].TokenId != info.StartToken || 
                    tokens[end].TokenId != info.EndToken) continue;

                return info;
            }

            return null;
        }

        TokenInfo GetToken(int start, int end, out int tokenPosition)
        {
            // We go through all tokens.
            int inGroup = 0;
            uint maxScore = 0;
            int currentBest = 0;
            tokenPosition = 0;

            // We first check beginning.
            for (int j = 0; j < tokenInfo.Length; j++)
            {
                if (tokenInfo[j].Position == TokenPosition.Left &&
                   tokenInfo[j].Priority > maxScore && 
                   tokenInfo[j].Token == tokens[start].TokenId)
                {
                    maxScore = tokenInfo[j].Priority;
                    currentBest = j;
                    tokenPosition = start;
                }
            }

            // First check groups.
            for (int j = 0; j < groupInfo.Length; j++)
            {
                if (groupInfo[j].StartToken == tokens[start].TokenId) inGroup++;
                if (groupInfo[j].EndToken == tokens[start].TokenId) inGroup--;
            }


            for (int i = start + 1; i < end; i++)
            {
                TokenId id = tokens[i].TokenId;

                // First check groups.
                for (int j = 0; j < groupInfo.Length; j++)
                {
                    if (groupInfo[j].StartToken == id) inGroup++;
                    if (groupInfo[j].EndToken == id) inGroup--;
                }

                if (inGroup < 0) throw new Exception("Group ended too many times");

                // We find token.
                for (int j = 0; j < tokenInfo.Length; j++)
                {
                    if (tokenInfo[j].Position == TokenPosition.Split &&
                       tokenInfo[j].Priority > maxScore && tokenInfo[j].Token == tokens[i].TokenId)
                    {
                        maxScore = tokenInfo[j].Priority;
                        currentBest = j;
                        tokenPosition = i;
                    }
                }
            }

            for (int j = 0; j < groupInfo.Length; j++)
            {
                if (groupInfo[j].StartToken == tokens[end].TokenId) inGroup++;
                if (groupInfo[j].EndToken == tokens[end].TokenId) inGroup--;
            }

            // We check the last.
            for (int j = 0; j < tokenInfo.Length; j++)
            {
                if (tokenInfo[j].Position == TokenPosition.Right &&
                   tokenInfo[j].Priority > maxScore && tokenInfo[j].Token == tokens[end].TokenId)
                {
                    maxScore = tokenInfo[j].Priority;
                    currentBest = j;
                    tokenPosition = end;
                }
            }

            if (inGroup != 0) throw new Exception("Asymetric groupings.");
            if (maxScore == 0) return null;
            return tokenInfo[currentBest];
        }

        IParseElement[] GetSequence(int start, int end)
        {
            List<IParseElement> elements = new List<IParseElement>();

            uint inGroup = 0;
            for (int i = start; i <= end; i++)
            {
                TokenId id = tokens[i].TokenId;

                // First check groups.
                for (int j = 0; j < groupInfo.Length; j++)
                {
                    if (groupInfo[j].StartToken == id) inGroup++;
                    if (groupInfo[j].EndToken == id) inGroup--;
                }

                if (inGroup == 0)
                {
                    elements.Add(Parse(start, i));
                    start = i + 1;
                }
            }

            return elements.ToArray();
        }

        IParseElement Parse(int start, int end)
        {
            // If start equals end, we insert a "blank" constant.
            if (start > end) return new Identifier(new Token(string.Empty, 0));
            if (start == end) return new Identifier(tokens[start]);

            // We first check if it is a group.
            GroupInfo group = GetGroup(start, end);
            if (group != null)
            {
                return new Group(group, Parse(start + 1, end - 1));
            }

            // We try to find split token.
            int tokenPosition;
            TokenInfo token = GetToken(start, end, out tokenPosition);
            if (token != null)
            {
                if (token.Position == TokenPosition.Left)
                {
                    return new LeftToken(token, Parse(start + 1, end));
                }
                else if (token.Position == TokenPosition.Split)
                {
                    return new SplitToken(token, Parse(start, tokenPosition - 1), Parse(tokenPosition + 1, end));
                }
                else
                {
                    return new RightToken(token, Parse(start, end - 1));
                }
            }

            // We have a sequence.
            return new ElementSequence(GetSequence(start, end));
        }


        #endregion


        #region Public Members

        /// <summary>
        /// Parses the tokens.
        /// </summary>
        public static IParseElement Parse(List<Token> tokens, TokenInfo[] tokenInfo, GroupInfo[] groupInfo)
        {
            return Parse(tokens, 0, (uint)tokens.Count, tokenInfo, groupInfo);
        }

        /// <summary>
        /// Parses the tokens.
        /// </summary>
        public static IParseElement Parse(List<Token> tokens, uint start, uint end,
                                            TokenInfo[] tokenInfo, GroupInfo[] groupInfo)
        {
            if (tokens[(int)end].TokenId == TokenId.Terminate) end--;
            if(end < start) throw new ArgumentException("start,end");

            Parser parser = new Parser();
            parser.tokens = tokens;
            parser.tokenInfo = tokenInfo;
            parser.groupInfo = groupInfo;

            // We find split.
            return parser.Parse((int)start, (int)end);

        }

        #endregion

    }
}
