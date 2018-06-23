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

namespace SharpMedia.Database.Managed
{

    /// <summary>
    /// A path in database helper.
    /// </summary>
    internal static class PathHelper
    {
        /// <summary>
        /// A slash string.
        /// </summary>
        public const string Slash = "/";

        /// <summary>
        /// A slash seperator list.
        /// </summary>
        public static readonly char[] SlashSeperator = new char[] { '/' };

        /// <summary>
        /// A link to parent.
        /// </summary>
        public const string ParentLink = "..";

        /// <summary>
        /// A this link string.
        /// </summary>
        public const string ThisLink = ".";

        /// <summary>
        /// Validates if path is valid.
        /// </summary>
        /// <param name="path">The path in question. May not have special keys.</param>
        /// <returns></returns>
        public static bool ValidatePath(ref string path)
        {
            // We strip away the '/' at the p1.
            if (path.EndsWith(Slash)) path = path.Substring(0, path.Length - 1);

            return true;
        }

        /// <summary>
        /// Seperates a path on first '/' token.
        /// </summary>
        /// <param name="path">The in path.</param>
        /// <param name="part">The other part, can be null.</param>
        /// <returns></returns>
        public static string SeperatePath(string path, out string part)
        {
            string[] array = path.Split(SlashSeperator, 2);
            if (array.Length == 1)
            {
                part = null;
                return path;
            }
            else
            {
                part = array[1];
                return array[0];
            }
        }

        /// <summary>
        /// Validates the name, e.g. one part of path.
        /// </summary>
        /// <param name="name">The name of one part.</param>
        /// <returns></returns>
        public static bool ValidateName(string name)
        {
            return true;
        }

        /// <summary>
        /// Is the address complex, e.g. we must search.
        /// </summary>
        /// <param name="path">The address name</param>
        public static bool IsComplex(string path)
        {
            return path.Contains(Slash);
        }

    }
}
