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
using System.IO;

namespace TemplateEngine
{

    /// <summary>
    /// A source document.
    /// </summary>
    public class SourceDocument : Preprocessor.HolderNode
    {
        #region Internal Classes

        enum TokenType
        {
            Identifier,
            Insert,
            Tag
        }

        class Token
        {
            #region Private Members
            TokenType type;
            string identifier;
            int line;
            #endregion

            #region Properties

            public TokenType Type
            {
                get { return type; }
            }

            public string Identifier
            {
                get { return identifier; }
            }

            public int Line
            {
                get
                {
                    return line;
                }
            }

            #endregion

            #region Constructors

            public Token(TokenType type, string id, int line)
            {
                this.type = type;
                this.identifier = id;
                this.line = line;
            }

            #endregion
        }


        #endregion

        #region Properties

        /// <summary>
        /// Elements of document.
        /// </summary>
        public IDocumentNode[] Nodes
        {
            get
            {
                return nodes.Clone() as IDocumentNode[];
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructors.
        /// </summary>
        /// <param name="nodes"></param>
        public SourceDocument(IDocumentNode[] nodes)
            : base(nodes)
        {

        }

        #endregion

        #region Static Members

        /// <summary>
        /// Parses file as document.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static SourceDocument Parse(string file)
        {
            using (StreamReader reader = new StreamReader(file))
            {
                return Parse(reader);
            }
        }

        /// <summary>
        /// Parses stream as document.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static SourceDocument Parse(StreamReader reader)
        {
            // We read the whole file (not optimal, but there is always enough storage for that).
            string str = reader.ReadToEnd();

            List<Token> tokens = new List<Token>();

            // We tokenize.
            int idx = 0;
            int line = 1;
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                
                if (c == '@')
                {
                    tokens.Add(new Token(TokenType.Identifier, str.Substring(idx, i - idx), line));
                    idx = i;
                    for (++i; i < str.Length; i++)
                    {
                        if (!char.IsLetter(str[i]) && !char.IsDigit(str[i])) break;
                    }
                    

                    tokens.Add(new Token(TokenType.Insert, str.Substring(idx+1, i-idx-1), line));
                    idx = i--;
                }
                else if (c == '/' && i + 2 < str.Length && str[i+1] == '/' && str[i+2] == '#')
                {
                    tokens.Add(new Token(TokenType.Identifier, str.Substring(idx, i - idx), line));

                    idx = i;
                    for (i += 2; i < str.Length; i++)
                    {
                        if (str[i] == '\n') break;
                    }

                    tokens.Add(new Token(TokenType.Tag, str.Substring(idx, i - idx - 1), line));
                    idx = i--;
                }


                if(c == '\n') line++;
            }

            tokens.Add(new Token(TokenType.Identifier, str.Substring(idx, str.Length - idx), line));
            

            // We now convert tokens to hierarchy.
            List<IDocumentNode> nodes = new List<IDocumentNode>();
            CreateNodes(tokens.ToArray(), delegate(string s) { return false; }, 0, nodes);
            return new SourceDocument(nodes.ToArray());
            
        }

        static string ExtractCommand(string id)
        {
            return id.Split(' ', '\t')[0].Substring(3);
        }

        static string[] ExtractAndedCommandBody(string id)
        {
            // For now ...
            string[] data = id.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            List<string> output = new List<string>();
            for (int i = 1; i < data.Length; i ++)
            {
                if (i % 2 == 0)
                {
                    if (data[i] != "&&")
                    {
                        throw new Exception("Only anding supported.");
                    }
                }
                else
                {
                    output.Add(data[i]);
                }
            }

            return output.ToArray();
        }


        static int CreateNodes(Token[] tokens, Predicate<string> terminating, 
            int from, List<IDocumentNode> nodes)
        {
            for (int i = from; i < tokens.Length; i++)
            {
                Token token = tokens[i];

                string command = string.Empty;
                if (token.Type == TokenType.Tag)
                {
                    command = ExtractCommand(token.Identifier);
                }

                if (terminating(command)) return i;

                // We process it else.
                

                switch (command)
                {
                    case "":
                        if (token.Type == TokenType.Insert)
                        {
                            nodes.Add(new Preprocessor.ReferenceNode(token.Identifier));
                        }
                        else
                        {
                            nodes.Add(new Preprocessor.StringNode(token.Identifier));
                        }
                        break;
                    case "else":
                        nodes.Add(new Preprocessor.ElseNode());
                        break;
                    case "ifdef":
                        // We process
                        List<IDocumentNode> subNodes = new List<IDocumentNode>();
                        i = CreateNodes(tokens, delegate(string cmd) { return cmd == "endif"; }, i + 1, subNodes);
                        nodes.Add(new Preprocessor.IfDefNode(subNodes.ToArray(), 
                            ExtractAndedCommandBody(token.Identifier)));
                        break;
                    case "foreach":
                        // We process
                        List<IDocumentNode> subNodes2 = new List<IDocumentNode>();
                        i = CreateNodes(tokens, delegate(string cmd) { return cmd == "endfor"; }, i + 1, subNodes2);
                        nodes.Add(new Preprocessor.ForEachNode(token.Identifier.Substring(11), subNodes2.ToArray()));
                        break;

                        break;
                    default:
                        throw new Exception(string.Format("Unrecognized or invalidly positioned command '{0}'", command));

                }

            }

            return tokens.Length;
        }

        #endregion


    }
}
