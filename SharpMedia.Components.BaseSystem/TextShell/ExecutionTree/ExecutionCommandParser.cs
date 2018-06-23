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
using SharpMedia.Scripting.CompilerFacilities;

namespace SharpMedia.Components.BaseSystem.TextShell.ExecutionTree
{

    /// <summary>
    /// Execution command helper class.
    /// </summary>
    internal static class ExecutionCommandParser
    {
        #region Private Members

        private static AllowTokenResult AllowTokensDelegate(TokenId id)
        {
            switch (id)
            {
                case TokenId.Identifier:
                case TokenId.Dot:
                case TokenId.Comma:
                    return AllowTokenResult.AsIdentifier;
                case TokenId.LeftBracket:
                case TokenId.RightBracket:
                    return AllowTokenResult.Allow;
                case TokenId.LeftSquareBracket:
                case TokenId.RightSquareBracket:
                case TokenId.LeftCurlyBracket:
                case TokenId.RightCurlyBracket: 
                case TokenId.Number:
                case TokenId.Plus:
                case TokenId.Minus:
                case TokenId.Percent:
                    return AllowTokenResult.AsIdentifier;
                case TokenId.Grave:
                    return AllowTokenResult.Allow;
                case TokenId.Colon:
                    return AllowTokenResult.AsIdentifier;
                case TokenId.Or:
                case TokenId.And:
                    return AllowTokenResult.Allow;
                case TokenId.Less:
                case TokenId.More:     
                case TokenId.Assign:
                case TokenId.Not:
                    return AllowTokenResult.AsIdentifier;
                case TokenId.Quote:
                    return AllowTokenResult.Allow;
                case TokenId.Backslash:
                case TokenId.Slash:
                    return AllowTokenResult.AsIdentifier;
                case TokenId.Space:
                case TokenId.Tab:
                case TokenId.Newline:
                    return AllowTokenResult.Ignore;
                case TokenId.LogicalOr:
                case TokenId.LogicalAnd:
                    return AllowTokenResult.Allow;
                case TokenId.NotEqual:
                case TokenId.Equal:
                case TokenId.Arrow:
                    return AllowTokenResult.AsIdentifier;
                case TokenId.String:
                    return AllowTokenResult.Allow;
                case TokenId.Comment:
                    return AllowTokenResult.AsIdentifier;
                case TokenId.Terminate:
                    return AllowTokenResult.Allow;
                case TokenId.Unknown:
                default:
                    return AllowTokenResult.AsIdentifier;
            }
        }

        #region Parsing Specifier

        static GroupInfo internalExpression = new GroupInfo(TokenId.LeftCurlyBracket, TokenId.RightCurlyBracket);
        static GroupInfo groupExpression = new GroupInfo(TokenId.LeftBracket, TokenId.RightBracket);
        static TokenInfo logicalAnd = new TokenInfo(TokenId.LogicalAnd, TokenPosition.Split, 5);
        static TokenInfo logicalOr = new TokenInfo(TokenId.LogicalOr, TokenPosition.Split, 5);
        static TokenInfo parallel = new TokenInfo(TokenId.And, TokenPosition.Split, 4);
        static TokenInfo pipe = new TokenInfo(TokenId.Or, TokenPosition.Split, 4);

        static GroupInfo[] groupInfo = new GroupInfo[]{
            internalExpression,
            groupExpression
        };

        static TokenInfo[] tokenInfo = new TokenInfo[]{
            logicalAnd,
            logicalOr,
            parallel,
            pipe
        };

        #endregion


        static IExecutionCommand Parse(bool executable, IParseElement element)
        {
            bool dummy;
            return Parse(executable, element, out dummy);
        }

        static IExecutionCommand Parse(bool executable, IParseElement element, out bool isGroup)
        {
            isGroup = false;

            if (element is Group)
            {
                Group group = (Group)element;

                if (group.GroupInfo == internalExpression)
                {
                    isGroup = true;
                    return Parse(true, group.InternalElement);
                }
                else if (group.GroupInfo == groupExpression)
                {
                    isGroup = true;
                    // We skip group expression.
                    return Parse(true, group.InternalElement);
                }
            }
            else if(element is Identifier)
            {
                Identifier id = (Identifier) element;

                if (!executable)
                {
                    return new ConstantCommand(id.Token.TokenValue);
                }
                else
                {
                    return new MethodCallCommand(new ConstantCommand(id.Token.TokenValue), new IExecutionCommand[0]);
                }
            }
            else if (element is SplitToken)
            {
                SplitToken token = (SplitToken)element;

                if (token.TokenInfo == logicalAnd)
                {
                    return new AndCommand(Parse(true, token.LeftElement), Parse(true, token.RightElement));
                }
                else if (token.TokenInfo == logicalOr)
                {
                    return new OrCommand(Parse(true, token.LeftElement), Parse(true, token.RightElement));
                }
                else if (token.TokenInfo == parallel)
                {
                    return new ParallelCommand(Parse(true, token.LeftElement), Parse(true, token.RightElement));
                }
                else if (token.TokenInfo == pipe)
                {
                    return new PipeCommand(Parse(true, token.LeftElement), Parse(true, token.RightElement));
                }
            }
            else if (element is ElementSequence)
            {
                ElementSequence token = (ElementSequence)element;

                // We have a method call.
                IExecutionCommand methodName = Parse(false, token.Elements[0]);
                List<IExecutionCommand> parameters = new List<IExecutionCommand>();
                for (int i = 1; i < token.Elements.Length; i++)
                {
                    parameters.Add(Parse(false, token.Elements[i]));
                }

                return new MethodCallCommand(methodName, parameters.ToArray());
            }

            throw new Exception("Could not parse command.");
        }

        private static IExecutionCommand Parse(List<Token> tokens, uint start, uint end)
        {
            IParseElement element = Parser.Parse(tokens, start, end, tokenInfo, groupInfo);

            bool isGroup;
            IExecutionCommand command = Parse(true, element, out isGroup);
            if (isGroup) return new MethodCallCommand(command, new IExecutionCommand[0]);
            return command;
        }

        #endregion

        #region Public Members


        /// <summary>
        /// Parses commands into execution tree.
        /// </summary>
        public static IExecutionCommand Parse(string line)
        {
            List<Token> tokens = Token.Tokenize(line, AllowTokensDelegate);

            // We first partition into subgroups.
            return Parse(tokens, 0, (uint)tokens.Count - 1);

        }


        #endregion
    }
}
