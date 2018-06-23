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

namespace SharpMedia.Graphics.GUI.Compiler.Emit
{
    /// <summary>
    /// A compile context.
    /// </summary>
    internal sealed class CompileContext
    {
        /// <summary>
        /// Compile options.
        /// </summary>
        public readonly CompileOptions Options;

        /// <summary>
        /// All widgets, registered in proceess.
        /// </summary>
        public readonly SortedList<string, ASTWidget> Widgets = new SortedList<string, ASTWidget>();

        /// <summary>
        /// All styles, registered in process.
        /// </summary>
        public readonly SortedList<string, Styles.ASTStyle> Styles = 
            new SortedList<string, Styles.ASTStyle>();

        /// <summary>
        /// All animations, registered in process.
        /// </summary>
        public readonly SortedList<string, Animations.ASTAnimation> Animations =
            new SortedList<string, Animations.ASTAnimation>();

        /// <summary>
        /// The application context type name.
        /// </summary>
        public string ApplicationContextType;

        /// <summary>
        /// Adds a widget.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public void RegisterWidget(string name, ASTWidget widget)
        {
            Widgets.Add(name, widget);
        }


        uint counter = 0;

        public string GetNextTempName()
        {
            return string.Format("temp_{0}", counter++);
        }

        /// <summary>
        /// Resolves the type.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Type ResolveType(string prefix, string name)
        {
            if (prefix == "")
            {
                return Type.GetType(name);
            }
            else if (prefix == StandardPrefixes.Gui)
            {
                return Type.GetType("SharpMedia.Graphics.GUI.Widgets." + name);
            }
            else if (prefix == StandardPrefixes.Cointainer)
            {
                return Type.GetType("SharpMedia.Graphics.GUI.Widgets.Containers." + name);
            }
            else if (prefix == StandardPrefixes.Style)
            {
                return Type.GetType("SharpMedia.Graphics.GUI.Styles." + name);
            }
            else if (prefix == StandardPrefixes.Verification)
            {
                return Type.GetType("SharpMedia.Graphics.GUI.Validation." + name);
            }
            else if (prefix == StandardPrefixes.Animation)
            {
                return Type.GetType("SharpMedia.Graphics.GUI.Animations." + name);
            }
            else if (prefix == StandardPrefixes.Fills)
            {
                return Type.GetType("SharpMedia.Graphics.Vector.Fills." + name);
            }

            if (Options.AdditionalNamespacesMapper.ContainsKey(prefix))
            {
                return Type.GetType(Options.AdditionalNamespacesMapper[prefix] + "." + name);
            }
            return null;
        }

        public CompileContext(CompileOptions op)
        {
            this.Options = op;
        }
    }
}
