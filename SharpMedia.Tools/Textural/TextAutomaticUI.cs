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
using SharpMedia.Tools.Parameters;
using SharpMedia.Components.Configuration;
using SharpMedia.Components.TextConsole;
using SharpMedia.Scripting.CompilerFacilities;

namespace SharpMedia.Tools.Textural
{

    /// <summary>
    /// A Textural Shell based "application" for UI providing.
    /// </summary>
    public class TextAutomaticUI : IAutomaticUI
    {
        #region Private Members
        ITextConsole console;
        #endregion

        #region Components

        /// <summary>
        /// The console.
        /// </summary>
        [Required]
        public ITextConsole Console
        {
            get { return console; }
            set { console = value; }
        }

        #endregion

        #region Private Members

        string GenDesc(IToolParameter param)
        {
            // We generate description.
            StringBuilder b = new StringBuilder();
            b.Append(param.Name);

            b.Append(" [");
            bool comma = false;
            if (param.Attribute.IsOptional)
            {
                b.Append("Optional");
                comma = true;
            }

            if (param.AcceptsOnlyHintedValues)
            {
                if (comma) b.Append(", ");
                b.Append("AcceptsOnlyHinted");
            }

            if (param.IsSet)
            {
                b.Append("IsSet");
            }

            b.Append("]");

            return b.ToString();
        }

        void List(IToolParameter[] toolParameters)
        {
            // We now output tool parameters.
            for (int i = 0; i < toolParameters.Length; i++)
            {
                IToolParameter parameter = toolParameters[i];

                console.WriteLine("\t{0} - {1}", i, GenDesc(parameter));
            }
        }

        void Help(IToolParameter parameter)
        {
            console.WriteLine("Info in parameter '{0}' with friendly name '{1}'", parameter.Name, parameter.Attribute.FriendlyName);
            console.WriteLine("Description: {0}", parameter.Attribute.Description);
            console.WriteLine("Optional: {0}", parameter.Attribute.IsOptional ? "true" : "false");
            console.WriteLine("UI Group: {0}", parameter.Attribute.UIGroup);
        }

        void Help()
        {
            console.WriteLine("Set the parameter you wish to write/change by typeing name," +
                              "friendly name or index and then set its value. The format is 'param value'");
        }


        AllowTokenResult AllowTokensDelegate(TokenId id)
        {
            switch (id)
            {
                case TokenId.Space:
                case TokenId.Tab:
                case TokenId.Newline:
                    return AllowTokenResult.Ignore;
                case TokenId.Number:
                case TokenId.String:
                case TokenId.Identifier:
                    return AllowTokenResult.Allow;
                default:
                    return AllowTokenResult.AsIdentifier;
            }
        }

        int Find(string name, IToolParameter[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if (name == parameters[i].Name)
                {
                    return i;
                    break;
                }

                if (name == parameters[i].Attribute.FriendlyName)
                {
                    return i;
                    break;
                }
            }

            return -1;
        }


        
        #endregion


        #region IAutomaticUI Members

        public bool Run(Type toolName, IToolParameter[] toolParameters)
        {
            console.WriteLine("Welcome to text based tool configuration for tool '{0}'", toolName.Name);
            console.WriteLine("Use 'help [parameter index]' for help, 'list' to list parameters," +
                    "'accept' to end configuration and 'exit' to terminate configuration.");

            while (true)
            {
                try
                {

                    console.Write("Configure {0}>", toolName.Name);

                    string line = console.ReadLine().Trim();

                    // We parse it.
                    List<Token> tokens = Token.Tokenize(line, AllowTokensDelegate);

                    // We precheck.
                    if (tokens.Count == 0) continue;
                    if (tokens.Count > 2)
                    {
                        console.WriteLine("Invalid command.");
                        continue;
                    }

                    int index = -1;

                    // We have a number.
                    if (tokens[0].TokenId == TokenId.Number)
                    {
                        index = int.Parse(tokens[0].Identifier);
                    }
                    else if (tokens[0].TokenId == TokenId.Identifier || tokens[0].TokenId == TokenId.String)
                    {
                        // We try to extract.
                        string name = tokens[0].Identifier;

                        // We check special options.
                        if (name.ToLower() == "exit") return false;
                        if (name.ToLower() == "accept") return true;
                        if (name.ToLower() == "list")
                        {
                            List(toolParameters);
                            continue;
                        }
                        if (name.ToLower() == "help")
                        {
                            if (tokens.Count == 2)
                            {
                                int idx = -1;
                                if (tokens[1].TokenId == TokenId.Number)
                                {
                                    idx = int.Parse(tokens[1].Identifier);
                                } else {
                                    idx = Find(tokens[1].Identifier, toolParameters);
                                }


                                if (idx < 0 || idx >= toolParameters.Length)
                                {
                                    console.WriteLine("Invalid parameter index/name.");
                                    continue;
                                }
                                else
                                {
                                    Help(toolParameters[idx]);
                                    continue;
                                }   
                            }
                            else
                            {
                                Help();
                            }
                            continue;
                        }

                        // We have parameter setter.
                        index = Find(tokens[0].Identifier, toolParameters);
                    }


                    // We check params.
                    if (index < 0 || index >= toolParameters.Length)
                    {
                        console.WriteLine("Invalid parameter index/name.");
                        continue;
                    }

                    if (tokens.Count != 2)
                    {
                        console.WriteLine("The format is 'param value', two arguments expected.");
                        continue;
                    }

                    toolParameters[index].Parse(tokens[1].Identifier);
                }
                catch (Exception ex)
                {
                    console.WriteLine(ex.ToString());
                }
            }
        }

        #endregion
    }
}
