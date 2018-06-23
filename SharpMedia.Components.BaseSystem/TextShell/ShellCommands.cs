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
using SharpMedia.Database;
using SharpMedia.Components.Applications;
using System.Reflection;

namespace SharpMedia.Components.BaseSystem.TextShell
{

    /// <summary>
    /// Common shell commands class.
    /// </summary>
    public static class CommonShellCommands
    {
        public static object[] EmptyParameters = new object[0];

        /// <summary>
        /// A Ls command.
        /// </summary>
        [ShellCommand(CaseSensitive=false)]
        public static int Ls(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;
            Node<object> dir = context.ShellApp.WorkingDirectory;

            // We search the directory.
            if (values.Length == 1)
            {
                dir = dir.Find(values[0].ToString());
            }
            else if(values.Length > 1)
            {
                context.Error.WriteLine("Invalid syntax: ls [directory]");
                return -1;
            }

            if (dir == null)
            {
                context.Output.WriteLine("Could not find {0} in directory {1}", values[0], dir);
                return 1;
            }

            // We print them all (and also return parameters).
            List<string> parameters = new List<string>();
            foreach (string name in dir.ChildNames)
            {
                parameters.Add(name);
                context.Output.WriteLine(name);
            }

            inlineParameters = parameters.ToArray();

            return 1;
        }

        [ShellCommand(CaseSensitive = false)]
        public static int SetReportLevel(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            string info = "notrace";
            if (values.Length == 1)
            {
                info = values[0].ToString().ToLowerInvariant();
            }

            if (values.Length > 1 || (info != "notrace" && info != "full" && info != "minimal"))
            {
                context.Error.WriteLine("Invalid syntax: ReportLevel [level], where level is 'Full', 'NoTrace', 'Minimal'");
                return -1;
            }

            // We now set.
            switch (info)
            {
                case "notrace":
                    context.ShellApp.ReportLevel = ReportLevel.NoTrace;
                    break;
                case "full":
                    context.ShellApp.ReportLevel = ReportLevel.Full;
                    break;
                case "minimal":
                    context.ShellApp.ReportLevel = ReportLevel.Minimal;
                    break;
            }

            return 1;

            

        }

        /// <summary>
        /// Change directory command.
        /// </summary>
        [ShellCommand(CaseSensitive=false)]
        public static int Cd(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            Node<object> dir = context.ShellApp.WorkingDirectory;
            if (values.Length == 1)
            {
                dir = dir.Find(values[0].ToString());
            }
            else
            {
                context.Error.WriteLine("Invalid syntax: cd NewDirectory");
                return -1;
            }

            if (dir == null)
            {
                context.Output.WriteLine("Could not find '{0}' in directory {1}", values[0].ToString(), dir);
                return -1;
            }

            context.ShellApp.WorkingDirectory = dir;
            inlineParameters = new object[] { context.ShellApp.WorkingDirectory.ToString() };
            return 1;
        }

        /// <summary>
        /// Prints all processes.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="values"></param>
        [ShellCommand(CaseSensitive = false, CanParallel=true)]
        public static int Ps(ExecutionContext context, object[] values, out object[] inlineParameters)
        {

            inlineParameters = EmptyParameters;

            if (values.Length != 0)
            {
                context.Error.WriteLine("Invalid syntax: ps accepts no parameters.");
                return -1;
            }

            List<object> result = new List<object>();

            foreach (IApplicationInstance app2 in context.ShellApp.ApplicationInstances.Instances)
            {
                result.Add(app2.InstanceId.ToString());
                context.Output.WriteLine("{0}: {1} (#{2})", app2.InstanceId, app2.ApplicationInfo.Name, app2.InstanceNumber);
            }

            inlineParameters = result.ToArray();

            return 1;
        }

        /// <summary>
        /// Infos a directory.
        /// </summary>
        [ShellCommand(CaseSensitive = false)]
        public static int Info(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            Node<object> directory = context.ShellApp.WorkingDirectory;
            if (values.Length == 1)
            {
                directory = directory.Find(values[0].ToString());
            } else if(values.Length > 1)
            {
                context.Error.WriteLine("Syntax error: info [directory]");
                return -1;
            }

            // If directory not found.
            if (directory == null)
            {
                context.Output.WriteLine("Directory '{0}' does not exist in '{1}'", 
                    values[0].ToString(), context.ShellApp.WorkingDirectory);
                return 1;
            }

            // We now print information.
            context.Output.WriteLine("Info: \"{0}\", Version={1} DefaultType={2}", 
                directory.Name, directory.Version, directory.DefaultType);
            foreach (Type type in directory.TypedStreams)
            {
                context.Output.WriteLine("\t{0}", type);
            }

            return 1;

        }

        /// <summary>
        /// Deletes a node.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="values"></param>
        /// <param name="inlineParameters"></param>
        /// <returns></returns>
        [ShellCommand(CaseSensitive = false, CanParallel=true)]
        public static int Delete(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            Node<object> dir = context.ShellApp.WorkingDirectory;

            if (values.Length != 1)
            {
                context.Error.WriteLine("Invalid syntax: Delete [node].");
                return -1;
            }

            dir = dir.Find(values[0].ToString());
            if (dir == null)
            {
                context.Output.WriteLine("Directory '{0}' does not exist in '{1}'", 
                    values[0].ToString(), context.ShellApp.WorkingDirectory);
                return 1;
            }

            context.ShellApp.WorkingDirectory.Delete(values[0].ToString());

            return 1;
        }

        [ShellCommand(CaseSensitive = false, CanParallel=true)]
        public static int Create(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            Node<object> dir = context.ShellApp.WorkingDirectory;

            Type type = typeof(object);
            StreamOptions options = StreamOptions.None;

            if (values.Length == 0 || values.Length > 3)
            {
                context.Error.WriteLine("Invalid syntax: Create 'path' ['type' ['flags']]");
                return -1;
            }

            if (values.Length > 1)
            {
                type = Type.GetType(values[1].ToString(), true);
            }
            if (values.Length > 2)
            {
                // FIXME:
                throw new NotImplementedException();
            }

            // We create directory.
            dir.Create(values[0].ToString(), type, options);

            return 1;
        }

        [ShellCommand(CaseSensitive = false)]
        public static int Alias(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            if (values.Length != 2)
            {
                context.Error.WriteLine("Invalid syntax: alias 'alias-value' 'real-value'");
                return -1;
            }

            context.ShellApp.Aliases[values[0].ToString()] = values[1].ToString();

            return 1;
        }

        [ShellCommand(CaseSensitive = false)]
        public static int GetAlias(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            if (values.Length != 1)
            {
                context.Error.WriteLine("Invalid syntax: getalias 'alias-value'");
                return -1;
            }

            string val;
            if (!context.ShellApp.Aliases.TryGetValue(values[0].ToString(), out val))
            {
                context.Error.WriteLine("Alias '{0}' does not exist", values[0].ToString());
                return -1;
            }

            inlineParameters = new object[] { val };
            context.Output.WriteLine("Alias for '{0}'", val);

            return 1;
        }

        [ShellCommand(CaseSensitive = false)]
        public static int RemoveAlias(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            if (values.Length != 1)
            {
                context.Error.WriteLine("Invalid syntax: RemoveAlias 'alias-value'");
                return -1;
            }

            if (!context.ShellApp.Aliases.Remove(values[0].ToString()))
            {
                context.Error.WriteLine("Alias '{0}' did not exist, could not remove", values[0].ToString());
            }

            return 1;
        }

        [ShellCommand(CaseSensitive = false)]
        public static int PushEnv(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            // TODO:

            return 1;
        }

        [ShellCommand(CaseSensitive = false)]
        public static int PopEnv(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            // TODO:

            return 1;
        }

        [ShellCommand(CaseSensitive = false)]
        public static int SetEnv(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            // TODO:

            return 1;
        }

        [ShellCommand(CaseSensitive = false)]
        public static int ParseAsObject(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            if (values.Length != 2)
            {
                context.Error.WriteLine("Invalid syntax: ParseAsObject String Type");
            }

            Type type = Type.GetType(values[1].ToString());
            string text = values[0].ToString();

            if (type == null)
            {
                context.Error.WriteLine("Could nto resolve type {0}", values[1].ToString());
                return -1;
            }

            MethodInfo info = type.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, 
                null, new Type[] { typeof(string) }, null);
            if (info == null)
            {
                context.Error.WriteLine("The type {0} does not contain a Parse method, cannot convert", type);
                return -1;
            }

            inlineParameters = new object[] { info.Invoke(null, new object[] { text }) };

            return 1;
        }

        [ShellCommand(CaseSensitive = false)]
        public static int CreateTypedStream(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            if (values.Length > 3 || values.Length < 2)
            {
                context.Error.WriteLine("Invalid syntax: CreateTypedStream NodePath TypedStream [Flags]");
                return -1;
            }

            string path = values[0].ToString();
            Type type = type = Type.GetType(values[1].ToString());
            StreamOptions options = StreamOptions.None;

            if (type == null)
            {
                context.Error.WriteLine("Type '{0}' not resolved.", values[1].ToString());
                return -1;
            }

            if (values.Length > 2)
            {
                options = (StreamOptions)Enum.Parse(typeof(StreamOptions), values[2].ToString());
            }


            Node<object> node = context.ShellApp.WorkingDirectory.Find(path);
            if (node == null)
            {
                context.Error.WriteLine("Path '{0}' does not exist", path);
                return -1;
            }

            node.AddTypedStream(type, options);
            return 1;
        }

        [ShellCommand(CaseSensitive = false)]
        public static int DeleteTypedStream(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            if (values.Length != 2)
            {
                context.Error.WriteLine("Invalid syntax: DeleteTypedStream NodePath TypedStream");
                return -1;
            }

            string path = values[0].ToString();
            Type type = type = Type.GetType(values[1].ToString());

            if (type == null)
            {
                context.Error.WriteLine("Type '{0}' not resolved.", values[1].ToString());
                return -1;
            }



            Node<object> node = context.ShellApp.WorkingDirectory.Find(path);
            if (node == null)
            {
                context.Error.WriteLine("Path '{0}' does not exist", path);
                return -1;
            }

            node.RemoveTypedStream(type);
            return 1;
        }

        [ShellCommand(CaseSensitive = false)]
        public static int WriteObject(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            if (values.Length < 2 || values.Length > 5)
            {
                context.Error.WriteLine("Invalid syntax: WriteObject @object NodePath [TypedStream [Index]]");
                return -1;
            }

            object obj = values[0];
            string path = values[1].ToString();
            Type type = null;
            uint index = 0;

            // We first get type.
            if (values.Length > 2)
            {
                type = Type.GetType(values[2].ToString());
                if (type == null)
                {
                    context.Error.WriteLine("Type '{0}' not resolved.", values[2].ToString());
                    return -1;
                }
            }

            if (values.Length > 3)
            {
                if (values[3] is uint)
                {
                    index = (uint)values[3];
                }
                else if (values[3] is int)
                {
                    index = (uint)((int)values[3]);
                }
                else if (!uint.TryParse(values[3].ToString(), out index))
                {
                    context.Error.WriteLine("The index is not a number: {0}", values[3].ToString());
                    return -1;
                }
            }


            Node<object> node = context.ShellApp.WorkingDirectory.Find(path);
            if (node == null)
            {
                context.Error.WriteLine("Path '{0}' does not exist", path);
                return -1;
            }

            TypedStream<object> typedStream = null;

            try
            {
                if (type == null)
                {
                    typedStream = node.OpenForWriting();
                }
                else
                {
                    typedStream = node.Open(type, OpenMode.Write);
                }

                if (typedStream == null)
                {
                    context.Error.WriteLine("Typed stream '{0}' does not exist on path '{1}'", type, path);
                    return -1;
                }

                // We write it.
                typedStream.Write(index, obj);

            }
            finally
            {
                if (typedStream != null)
                {
                    typedStream.Dispose();
                }
            }
            return 1;
        }

        [ShellCommand(CaseSensitive = false)]
        public static int ReadObject(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            if (values.Length < 1 || values.Length > 4)
            {
                context.Error.WriteLine("Invalid syntax: ReadObject NodePath [TypedStream [Index]]");
                return -1;
            }

            string path = values[0].ToString();
            Type type = null;
            uint index = 0;

            // We first get type.
            if (values.Length > 1)
            {
                type = Type.GetType(values[1].ToString());
                if (type == null)
                {
                    context.Error.WriteLine("Type '{0}' not resolved.", values[1].ToString());
                    return -1;
                }
            }

            if (values.Length > 2)
            {
                if (values[2] is uint)
                {
                    index = (uint)values[2];
                }
                else if (values[3] is int)
                {
                    index = (uint)((int)values[2]);
                }
                else if (!uint.TryParse(values[2].ToString(), out index))
                {
                    context.Error.WriteLine("The index is not a number: {0}", values[2].ToString());
                    return -1;
                }
            }


            Node<object> node = context.ShellApp.WorkingDirectory.Find(path);
            if (node == null)
            {
                context.Error.WriteLine("Path '{0}' does not exist", path);
                return -1;
            }

            TypedStream<object> typedStream = null;

            try
            {
                if (type == null)
                {
                    typedStream = node.OpenForReading();
                }
                else
                {
                    typedStream = node.Open(type, OpenMode.Read);
                }

                if (typedStream == null)
                {
                    context.Error.WriteLine("Typed stream '{0}' does not exist on path '{1}'", type, path);
                    return -1;
                }

                // We write it.
                inlineParameters = new object[] { typedStream.Read(index) };

            }
            finally
            {
                if (typedStream != null)
                {
                    typedStream.Dispose();
                }
            }
            return 1;
        }

        [ShellCommand(CaseSensitive = false)]
        public static int GetEnv(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            // TODO:

            return 1;
        }

        [ShellCommand(CaseSensitive = false, CanParallel=true)]
        public static int Search(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            Node<object> root = context.ShellApp.WorkingDirectory;

            if(values.Length != 1)
            {
                context.Error.WriteLine("Invalid syntax: Search 'path'");
                return -1;
            }

            Node<object>[] r = root.Search(values[0].ToString());
            inlineParameters = new object[r.Length];
            for (int i = 0; i < r.Length; i++)
            {
                inlineParameters[i] = r[i].Path;
                context.Output.WriteLine("\t{0}", r[i].Path);
            }

            return 1;
        }

        [ShellCommand(CaseSensitive = false)]
        public static int ListAliases(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            if (values.Length != 0)
            {
                context.Error.WriteLine("Syntex error: no parameters excpected.");
                return -1;
            }

            context.Output.WriteLine("Listing aliases:");
            foreach(KeyValuePair<string,string> aliases in context.ShellApp.Aliases)
            {
                context.Output.WriteLine("\t{0} => {1}", aliases.Key, aliases.Value);
            }

            return 1;
        }


        [ShellCommand(CaseSensitive = false, CanParallel=true)]
        public static int Kill(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            
            return 1;
        }

        [ShellCommand(CaseSensitive = false)]
        public static int Help(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;
            if (values.Length != 0)
            {
                context.Error.WriteLine("Invalid syntax: no parameters expected for help.");
                return -1;
            }

            int splitWidth = context.ShellApp.Console.Width - 40;

            System.IO.TextWriter output = context.Output;

            output.WriteLine("SharpMedia Console Help");

            output.WriteLine("Search paths:");
            foreach (string str in context.ShellApp.ExecutableSearchPaths)
            {
                output.WriteLine("\t{0}", str);

            }
            output.Write("Registered Commands:");

            int width = 0;
            foreach(Type type in context.ShellApp.ShellCommandTypes)
            {
                foreach(MethodInfo info in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
                {
                    if(info.GetCustomAttributes(typeof(ShellCommandAttribute), true).Length == 0) continue;

                    width += info.Name.Length;

                    if (width > splitWidth)
                    {
                        output.Write("{0}\t", output.NewLine);
                        width = 0;
                    }
                    output.Write("{0}, ", info.Name);

                }
            }

            

            return 1;
        }

        [ShellCommand(CaseSensitive = false)]
        public static int Echo(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            for (int i = 0; i < values.Length; i++)
            {
                context.Output.WriteLine(values[i].ToString());
            }

            return 1;
        }

        [ShellCommand(CaseSensitive = false)]
        public static int EchoStream(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            // Stream is simply echo-ed.
            inlineParameters = EmptyParameters;

            context.Output.Write(context.Input.ReadToEnd());

            return 1;
        }

        [ShellCommand(CaseSensitive = false)]
        public static int Install(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            

            return 1;
        }


        [ShellCommand(CaseSensitive = false)]
        public static int WriteProperty(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            if (values.Length != 3)
            {
                context.Error.WriteLine("Invalid syntax: WriteProperty Object PropertyName Value");
                return -1;
            }

            object obj = values[0];
            string propertyName = values[1].ToString();
            object value = values[2];

            if (obj == null)
            {
                context.Error.WriteLine("Object is null.");
                return -1;
            }

            PropertyInfo info = obj.GetType().GetProperty(propertyName);
            if (info == null || !info.CanWrite)
            {
                context.Error.WriteLine("Property {0} on object is non-existant or not writable", propertyName);
                return -1;
            }

            info.GetSetMethod(true).Invoke(obj, new object[] { value });

            return 1;
        }

        [ShellCommand(CaseSensitive = false)]
        public static int ReadProperty(ExecutionContext context, object[] values, out object[] inlineParameters)
        {
            inlineParameters = EmptyParameters;

            if (values.Length != 2)
            {
                context.Error.WriteLine("Invalid syntax: ReadProperty Object PropertyName");
                return -1;
            }

            object obj = values[0];
            string propertyName = values[1].ToString();

            if (obj == null)
            {
                context.Error.WriteLine("Object is null.");
                return -1;
            }

            PropertyInfo info = obj.GetType().GetProperty(propertyName);
            if (info == null || !info.CanRead)
            {
                context.Error.WriteLine("Property {0} on object is non-existant or not readable", propertyName);
                return -1;
            }

            inlineParameters = new object[] { info.GetGetMethod(true).Invoke(obj, new object[] { }) };

            return 1;
        }

    }
}
