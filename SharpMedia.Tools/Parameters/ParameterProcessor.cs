using System;
using System.Collections.Generic;
using System.Text;
using SharpMedia.Components.TextConsole;
using System.Reflection;
using SharpMedia.Database;

namespace SharpMedia.Tools.Parameters
{

    /// <summary>
    /// Extracts parameters from tool.
    /// </summary>
    public static class ParameterProcessor
    {
        /// <summary>
        /// Extracts parameters from tool.
        /// </summary>
        /// <param name="type">The tool type.</param>
        /// <returns>The tool parameters.</returns>
        public static IToolParameter[] Extract(Type type, DatabaseManager manager)
        {
            List<IToolParameter> parameters = new List<IToolParameter>();

            foreach (PropertyInfo info in type.GetProperties())
            {
                if (!info.CanWrite) continue;

                object[] attrs = info.GetCustomAttributes(typeof(UIAttribute), true);
                if (attrs.Length == 0) continue;

                // We extract attribute.
                UIAttribute attribute = attrs[0] as UIAttribute;
                Type propertyType = info.PropertyType;

                if (attribute is NodeUIAttribute)
                {
                    parameters.Add(new Node(attribute as NodeUIAttribute, 
                        propertyType, info.Name, manager));
                }
                else if (attribute is TypedStreamUIAttribute)
                {
                    parameters.Add(new TypedStream(attribute as TypedStreamUIAttribute,
                        propertyType, info.Name, manager));
                }
                else
                {
                    if (Common.IsTypeSameOrDerived(typeof(Enum), propertyType))
                    {
                        parameters.Add(new Enumerator(attribute, propertyType, info.Name));
                    }
                    else
                    {
                        parameters.Add(new Basic(attribute, propertyType, info.Name));
                    }
                }
            }

            return parameters.ToArray();
        }


        /// <summary>
        /// Extracts parameters, with already provided existing.
        /// </summary>
        /// <param name="type">The tool type.</param>
        /// <param name="existing">Existing parameters.</param>
        /// <remarks>If there are existing parameters that are not part of tool, 
        /// errors are written to stream.</remarks>
        /// <returns></returns>
        public static string[] Apply(IToolParameter[] parameters, string[] args, ITextConsole console)
        {
            List<string> unprocessed = new List<string>();

            for (int i = 0; i < args.Length; i++)
            {
                string[] data = args[i].Split('=');
                if (data.Length != 2)
                {
                    unprocessed.Add(args[i]);
                    continue;
                }

                for (int j = 0; j < parameters.Length; j++)
                {
                    if (data[0] == parameters[j].Name)
                    {
                        try
                        {
                            parameters[j].Parse(data[1]);
                            break;
                        }
                        catch (Exception ex)
                        {
                            unprocessed.Add(args[i]);
                            console.WriteLine(ex.ToString());
                        }

                        break;
                    }
                }
            }

            return args;
        }

        /// <summary>
        /// Unrolls parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string[] Unroll(IToolParameter[] parameters)
        {
            List<string> args = new List<string>();

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].IsSet)
                {
                    args.Add(parameters[i].ToString());
                }
            }

            return args.ToArray();
        }

    }
}
