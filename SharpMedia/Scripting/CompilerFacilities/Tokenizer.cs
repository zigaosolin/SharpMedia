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
using SharpMedia.Testing;

namespace SharpMedia.Scripting.CompilerFacilities
{
    /// <summary>
    /// A token id.
    /// </summary>
    public enum TokenId
    {
        // Identifier.
        Identifier,

        // Single character.
        Dot,
        Comma,
        LeftBracket,
        RightBracket,
        LeftSquareBracket,
        RightSquareBracket,
        LeftCurlyBracket,
        RightCurlyBracket,
        Number,
        Plus,
        Minus,
        Percent,
        Grave,
        Colon,
        Or,
        And,
        Less,
        More,
        Assign,
        Not,
        Quote,
        Backslash,
        Slash,
        Space,
        Tab,
        Newline,


        // Multicharacter.
        LogicalOr,
        LogicalAnd,
        NotEqual,
        Equal,
        Arrow,

        // Special
        String,
        Comment,

        // Termination
        Terminate,
        Unknown
    }


    /// <summary>
    /// Allow token result.
    /// </summary>
    public enum AllowTokenResult
    {
        Allow,
        Ignore,
        AsIdentifier
    }

    /// <summary>
    /// Delegate that allows token.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public delegate AllowTokenResult AllowToken(TokenId token);

    /// <summary>
    /// A token.
    /// </summary>
    public sealed class Token
    {
        #region Private Members
        TokenId tokenId;
        string identifier;

        // Error information.
        uint lineNumber;
        #endregion

        #region Tokenization Data

        struct TokenSpec
        {
            public char Token;
            public TokenId TokenId;

            public TokenSpec(char token, TokenId tokenId)
            {
                TokenId = tokenId;
                Token = token;
            }
        }

        static TokenSpec[] tokenSpecs = new TokenSpec[] {
            new TokenSpec('(', TokenId.LeftBracket),
            new TokenSpec(')', TokenId.RightBracket),
            new TokenSpec('[', TokenId.LeftSquareBracket),
            new TokenSpec(']', TokenId.RightSquareBracket),
            new TokenSpec('{', TokenId.LeftCurlyBracket),
            new TokenSpec('}', TokenId.RightCurlyBracket),
            new TokenSpec('+', TokenId.Plus),
            new TokenSpec('-', TokenId.Minus),
            new TokenSpec('.', TokenId.Dot),
            new TokenSpec(',', TokenId.Comma),
            new TokenSpec(':', TokenId.Colon),
            new TokenSpec('`', TokenId.Grave),
            new TokenSpec('%', TokenId.Percent),
            new TokenSpec('!', TokenId.Not),
            new TokenSpec('=', TokenId.Assign),
            new TokenSpec('<', TokenId.Less),
            new TokenSpec('>', TokenId.More),
            new TokenSpec('|', TokenId.Or),
            new TokenSpec('&', TokenId.And),
            new TokenSpec('"', TokenId.Quote),
            new TokenSpec('\\', TokenId.Backslash),
            new TokenSpec('/', TokenId.Slash),
            new TokenSpec(' ', TokenId.Space),
            new TokenSpec('\n', TokenId.Newline),
            new TokenSpec('\t', TokenId.Tab)
        };

        #endregion

        #region Static Members

        public static string TokenRepresentation(TokenId id)
        {
            switch (id)
            {
                case TokenId.Dot:
                    return ".";
                case TokenId.Comma:
                    return ",";
                case TokenId.LeftBracket:
                    return "(";
                case TokenId.RightBracket:
                    return ")";
                case TokenId.LeftSquareBracket:
                    return "[";
                case TokenId.RightSquareBracket:
                    return "]";
                case TokenId.LeftCurlyBracket:
                    return "{";
                case TokenId.RightCurlyBracket:
                    return "}";
                case TokenId.Colon:
                    return ".";
                case TokenId.Plus:
                    return "+";
                case TokenId.Grave:
                    return "`";
                case TokenId.Percent:
                    return "%";
                case TokenId.Minus:
                    return "-";
                case TokenId.And:
                    return "&";
                case TokenId.Or:
                    return "|";
                case TokenId.NotEqual:
                    return "!=";
                case TokenId.Equal:
                    return "==";
                case TokenId.Arrow:
                    return "->";
                case TokenId.Assign:
                    return "=";
                case TokenId.Backslash:
                    return "\\";
                case TokenId.Less:
                    return "<";
                case TokenId.More:
                    return ">";
                case TokenId.Newline:
                    return "\n";
                case TokenId.Not:
                    return "!";
                case TokenId.Quote:
                    return "\"";
                case TokenId.Slash:
                    return "/";
                case TokenId.Space:
                    return " ";
                case TokenId.Tab:
                    return "\t";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Splits identifiers such as "10px" to 10 and "px"
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="specifier"></param>
        /// <returns></returns>
        static int GetNumberWithSpecifier(string identifier, out string specifier)
        {
            int i;
            for (i = 0; i < identifier.Length; i++)
            {
                if (!char.IsNumber(identifier[i])) break;
            }

            // We now break it.
            specifier = identifier.Substring(i);

            int r;
            if (!int.TryParse(identifier.Substring(0, i), out r))
            {
                return int.MinValue;
            }
            return r;
        }

        #endregion

        #region Tokenization

        /// <summary>
        /// Tokenizes with all tokens allowed.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<Token> Tokenize(string str)
        {
            return Tokenize(str, StandardAllow);
        }

        /// <summary>
        /// Tokenizes the string.
        /// </summary>
        public static List<Token> Tokenize(string str, AllowToken allowToken)
        {
            int startIndex = 0;
            int currentIndex = 0;
            uint lineNumber = 0;

            List<Token> tokens = new List<Token>();

            // We tokenize.
            for (; currentIndex < str.Length; currentIndex++)
            {
                TokenId id = IsToken(str[currentIndex], allowToken);
                if (id != TokenId.Unknown)
                {
                    if (startIndex != currentIndex)
                    {
                        tokens.Add(new Token(str.Substring(startIndex, currentIndex - startIndex), lineNumber));
                    }

                    tokens.Add(new Token(id, lineNumber));

                    startIndex = currentIndex + 1;
                }
                
                if(str[currentIndex] == '\n') lineNumber++;
            }

            // We add last token.
            if (startIndex != currentIndex)
            {
                tokens.Add(new Token(str.Substring(startIndex, currentIndex - startIndex), lineNumber));
            }


            // We make some final adjustment, such as grouping numbers.
            for (int i = 0; i < tokens.Count; i++)
            {

                // 1) Numbers are "enlarged", so we support "1.0" to be parsed as one number, or
                // "1.0d" to be parsed as number and identifier.
                if (allowToken(TokenId.Number) == AllowTokenResult.Allow &&
                    tokens[i].TokenId == TokenId.Number && 
                    (i + 2) < tokens.Count && tokens[i + 1].TokenId == TokenId.Dot)
                {
                    if (tokens[i + 2].TokenId == TokenId.Number)
                    {
                        tokens[i].identifier = tokens[i].Identifier + "," + tokens[i + 2].Identifier;
                        tokens.RemoveRange(i + 1, 2);
                    }
                    else if (tokens[i + 2].TokenId == TokenId.Identifier)
                    {
                        string specifier;
                        int r = GetNumberWithSpecifier(tokens[i + 2].Identifier, out specifier);
                        if (r == int.MinValue) continue;

                        // Otherwise, we rearangle.
                        tokens[i].identifier = tokens[i].Identifier + "," + r.ToString();
                        tokens[i + 2].identifier = specifier;
                        tokens.RemoveAt(i + 1);
                    }


                // 2) Numbers with single specifier, "5d"
                }
                else if (allowToken(TokenId.Number) == AllowTokenResult.Allow && 
                         tokens[i].TokenId == TokenId.Identifier)
                {
                    string specifier;
                    int r = GetNumberWithSpecifier(tokens[i].Identifier, out specifier);
                    if (r == int.MinValue) continue;

                    // Otherwise we rearangle.
                    tokens[i].identifier = r.ToString();
                    tokens[i].tokenId = TokenId.Number;

                    tokens.Insert(i + 1, new Token(specifier, tokens[i].lineNumber));
                }
                // 4) String grouping.
                else if (tokens[i].TokenId == TokenId.Quote &&
               allowToken(TokenId.String) == AllowTokenResult.Allow)
                {
                    if (i > 0 && tokens[i - 1].TokenId == TokenId.Backslash) continue;
                    StringBuilder builder = new StringBuilder();

                    bool found = false;
                    int j = i;
                    while (++j < tokens.Count)
                    {

                        if (tokens[j].TokenId == TokenId.Quote)
                        {
                            found = true;
                            break;
                        }

                        builder.Append(tokens[j].TokenValue);
                    }

                    // If not found, we do nothing.
                    if (!found)
                    {
                        continue;
                    }

                    // We create range.
                    tokens[i] = new Token(TokenId.String, builder.ToString(), tokens[i].LineNumber);
                    tokens.RemoveRange(i + 1, j - i);
                }
                // 4) Comments
                
                // 5) We support grouppings
                else if (i > 0)
                {
                    // a) &&
                    if (allowToken(TokenId.LogicalAnd) == AllowTokenResult.Allow &&
                       tokens[i - 1].TokenId == TokenId.And &&
                       tokens[i].TokenId == TokenId.And)
                    {
                        tokens.RemoveAt(i);
                        tokens[i - 1] = new Token(TokenId.LogicalAnd, tokens[i-1].LineNumber);
                        i--;
                    }
                    // b) ||
                    else if (
                           allowToken(TokenId.LogicalOr) == AllowTokenResult.Allow &&
                            tokens[i - 1].TokenId == TokenId.Or &&
                           tokens[i].TokenId == TokenId.Or)
                    {
                        tokens.RemoveAt(i);
                        tokens[i - 1] = new Token(TokenId.LogicalOr, tokens[i-1].LineNumber);
                        i--;
                    }
                    // c) ->
                    else if (
                        allowToken(TokenId.Arrow) == AllowTokenResult.Allow &&
                        tokens[i - 1].TokenId == TokenId.Minus &&
                        tokens[i].TokenId == TokenId.Less)
                    {
                        tokens.RemoveAt(i);
                        tokens[i - 1] = new Token(TokenId.Arrow, tokens[i-1].LineNumber);
                        i--;
                    }
                    // d) ==
                    else if (
                       allowToken(TokenId.Equal) == AllowTokenResult.Allow &&
                            tokens[i - 1].TokenId == TokenId.Assign &&
                            tokens[i].TokenId == TokenId.Assign)
                    {
                        tokens.RemoveAt(i);
                        tokens[i - 1]= new Token(TokenId.Equal, tokens[i-1].LineNumber);
                        i--;
                    }
                    // d) !=
                    else if (
                       allowToken(TokenId.NotEqual) == AllowTokenResult.Allow &&
                       tokens[i - 1].TokenId == TokenId.Not &&
                       tokens[i].TokenId == TokenId.Assign)
                    {
                        tokens.RemoveAt(i);
                        tokens[i - 1] = new Token(TokenId.NotEqual, tokens[i-1].LineNumber);
                        i--;
                    }
                }
               
               
            }

            tokens.Add(new Token(TokenId.Terminate, 0));


            // We now filter all values.
            uint identifierLine = 0;
            StringBuilder identifierBuild = new StringBuilder();
            List<Token> results = new List<Token>(tokens.Count/2);
            for (int i = 0; i < tokens.Count; i++)
            {
                AllowTokenResult result = allowToken(tokens[i].TokenId);

                // We handle identifiers.
                if (result == AllowTokenResult.AsIdentifier)
                {
                    identifierLine = tokens[i].LineNumber;
                    identifierBuild.Append(tokens[i].TokenValue);
                    continue;
                }
                
                // We now handle non-ignores.
                if (identifierBuild.Length != 0)
                {
                    results.Add(new Token(identifierBuild.ToString(), identifierLine));
                    identifierBuild.Length = 0;
                }

                if (result != AllowTokenResult.Ignore)
                {
                    results.Add(tokens[i]);
                }
            }

            // We should push them.
            return results;
        }

        /// <summary>
        /// Converts to stack form of tokenization.
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public static Stack<Token> StackForm(List<Token> tokens)
        {
            Stack<Token> stack = new Stack<Token>(tokens.Count);
            for (int i = tokens.Count - 1; i >= 0; i--) stack.Push(tokens[i]);
            return stack;
        }

        /// <summary>
        /// Is token getter.
        /// </summary>
        static TokenId IsToken(char c, AllowToken allowToken)
        {
            for (int i = 0; i < tokenSpecs.Length; i++)
            {
                if (c == tokenSpecs[i].Token)
                {
                    if (allowToken(tokenSpecs[i].TokenId) != AllowTokenResult.AsIdentifier)
                    {
                        return tokenSpecs[i].TokenId;
                    }
                }
            }
            return TokenId.Unknown;
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Creates a token with id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lineNumber"></param>
        public Token(TokenId id, uint lineNumber)
        {
            this.tokenId = id;
            this.lineNumber = lineNumber;
        }

        /// <summary>
        /// Creates an identifier token.
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="lineNumber"></param>
        public Token(string identifier, uint lineNumber)
        {
            float dummy;
            if (float.TryParse(identifier, out dummy))
            {
                this.tokenId = TokenId.Number;
            }
            else
            {
                this.tokenId = TokenId.Identifier;
            }

            this.identifier = identifier;
            this.lineNumber = lineNumber;
        }

        /// <summary>
        /// Creates a token with specifier.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="identifier"></param>
        /// <param name="lineNumber"></param>
        public Token(TokenId id, string identifier, uint lineNumber)
        {
            this.tokenId = id;
            this.identifier = identifier;
            this.lineNumber = lineNumber;
        }

        #endregion

        #region Properties

        /// <summary>
        /// A token id.
        /// </summary>
        public TokenId TokenId
        {
            get
            {
                return tokenId;
            }
        }

        /// <summary>
        /// An identifier, in case toke is TokenId.Identifier.
        /// </summary>
        public string Identifier
        {
            get
            {
                return identifier;
            }
        }

        /// <summary>
        /// A value - either identifier or token converted to string.
        /// </summary>
        public string TokenValue
        {
            get
            {
                if (identifier != null) return identifier;
                return TokenRepresentation(tokenId);

            }
        }

        /// <summary>
        /// A line number.
        /// </summary>
        public uint LineNumber
        {
            get
            {
                return lineNumber;
            }
        }


        #endregion

        #region Tokenization Delegates

        /// <summary>
        /// A standard tokenization configuration.
        /// </summary>
        public static AllowTokenResult StandardAllow(TokenId id)
        {
            switch (id)
            {
                case TokenId.Identifier:
                case TokenId.Dot:
                case TokenId.Comma:
                case TokenId.LeftBracket:
                case TokenId.RightBracket:
                case TokenId.LeftSquareBracket:
                case TokenId.RightSquareBracket:
                case TokenId.LeftCurlyBracket:
                case TokenId.RightCurlyBracket:
                case TokenId.Number:
                case TokenId.Plus:
                case TokenId.Minus:
                case TokenId.Percent:
                case TokenId.Grave:
                case TokenId.Colon:
                case TokenId.Or:
                case TokenId.And:
                case TokenId.Less:
                case TokenId.More:
                case TokenId.Assign:
                case TokenId.Not:
                case TokenId.Quote:
                case TokenId.Backslash:
                case TokenId.Slash:
                    return AllowTokenResult.Allow;
                case TokenId.Space:
                case TokenId.Tab:
                case TokenId.Newline:
                    return AllowTokenResult.Ignore;
                case TokenId.LogicalOr:
                case TokenId.LogicalAnd:
                case TokenId.NotEqual:
                case TokenId.Equal:
                    return AllowTokenResult.Allow;
                case TokenId.Arrow:
                    return AllowTokenResult.AsIdentifier;
                case TokenId.String:
                case TokenId.Comment:
                case TokenId.Terminate:
                    return AllowTokenResult.Allow;
                case TokenId.Unknown:
                default:
                    return AllowTokenResult.AsIdentifier;
            }
        }

        #endregion
    }


#if SHARPMEDIA_TESTSUITE

    [TestSuite]
    class TokenStreamTest
    {
        [CorrectnessTest]
        public void Tokenize()
        {
            //List<Token> tokens1 = Token.Tokenize("10.0d+ thirteen%-");
            List<Token> tokens = Token.Tokenize("|| %% &&\"some string && || | I need to preserve\"",
                delegate(TokenId id) { return id == TokenId.Space ? AllowTokenResult.Ignore : AllowTokenResult.Allow; });

            Assert.AreEqual(6, tokens.Count);
            
        }

        [CorrectnessTest]
        public void Parse()
        {
            
        }
    }

#endif

}
