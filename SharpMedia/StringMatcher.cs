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
using SharpMedia.AspectOriented;
using System.Collections.ObjectModel;
using SharpMedia.Testing;
using SharpMedia.Math;

namespace SharpMedia
{
    /// <summary>
    /// A string matching rules. Can be user defined.
    /// </summary>
    [Serializable]
    public class StringMatchingRules
    {
        /// <summary>
        /// The initial score at the beginning of matching.
        /// </summary>
        public float InitialScore = 1.0f;

        /// <summary>
        /// All results below this score are discarded.
        /// </summary>
        public float MinScoreFilter = 0.0f;

        /// <summary>
        /// The character case mismatch penalty - e.g. placing 'a' instead of 'A' or vice versa.
        /// </summary>
        public float CharCaseMismatchPenalty = 0.1f;
        
        /// <summary>
        /// Character omit penalty - if the matching string has too many characters - e.g. "wayzs" matching
        /// to "ways" (if z is ignored).
        /// </summary>
        public float CharIgnorePenalty = 1.0f;

        /// <summary>
        /// Character missing penalty - for example writing cpy instead of copy results in 
        /// one such penalty at 'o' (no 'o' in matching string).
        /// </summary>
        public float CharMissingPenalty = 1.0f;

        /// <summary>
        /// Special case missing character at the end penalty. Those characters are usually easier to be omitted.
        /// </summary>
        public float CharMissingPenaltyAtEnd = 0.1f;

    }

    /// <summary>
    /// A node in string matcher tree, it is forward and backward iteratable.
    /// </summary>
    [Serializable]
    public class StringTreeNode
    {
        #region Private Members
        internal protected char character = '\0';
        internal protected StringTreeNode parent = null;
        internal protected List<StringTreeNode> children = new List<StringTreeNode>();
        internal protected string internalString = string.Empty;
        #endregion

        #region Protected Members

        /// <summary>
        /// Adds to list.
        /// </summary>
        /// <param name="list">The list.</param>
        internal protected void AddToListInternal(List<string> list)
        {
            if (internalString != string.Empty) list.Add(internalString);

            for (int i = 0; i < children.Count; i++)
            {
                children[i].AddToListInternal(list);
            }
        }

        /// <summary>
        /// Adds the internal.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="position">The position.</param>
        internal protected void AddInternal(string str, int position)
        {
            if (position == str.Length)
            {
                internalString = str;
                return;
            }

            // We check if it exists.
            for (int i = 0; i < children.Count; i++)
            {
                if (str[position] == children[i].character)
                {
                    children[i].AddInternal(str, position + 1);
                    return;
                }
            }

            // We have to create it.
            StringTreeNode node = new StringTreeNode();
            node.parent = this;
            node.character = str[position];

            // We do the recursion.
            node.AddInternal(str, position + 1);

            // Add to children.
            children.Add(node);
        }

        /// <summary>
        /// Removes the internal.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        internal protected bool RemoveInternal(string str, int position)
        {
            // We remove it if last.
            if (str.Length == position)
            {
                if (internalString != string.Empty)
                {
                    internalString = string.Empty;
                    return true;
                }
                return false;
            }

            // We do the recursion.
            for (int i = 0; i < children.Count; i++)
            {
                if (str[position] == children[i].character)
                {
                    children[i].RemoveInternal(str, position + 1);
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified STR contains internal.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="position">The position.</param>
        /// <returns>
        /// 	<c>true</c> if the specified STR contains internal; otherwise, <c>false</c>.
        /// </returns>
        internal protected bool ContainsInternal(string str, int position)
        {
            if (str.Length == position)
            {
                return true;
            }

            for (int i = 0; i < children.Count; i++)
            {
                if (str[position] == children[i].character)
                {
                    children[i].ContainsInternal(str, position + 1);
                }
            }

            return false;
        }

        /// <summary>
        /// Auto completes the string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="position">The position.</param>
        /// <param name="score">The score.</param>
        /// <param name="rules">The rules.</param>
        /// <param name="result">The result.</param>
        internal protected void AutoComplete(string str, int position, float score, 
            StringMatchingRules rules, List<KeyValuePair<float, StringTreeNode>> result)
        {
            // If cannot procceed, return.
            if (score < rules.MinScoreFilter) return;

            // Check if last character.
            if (position + 1 == str.Length)
            {
                result.Add(new KeyValuePair<float, StringTreeNode>(score, this));
                return;
            }

            // We first check if matched.
            if (str[position] == character)
            {
                // We check if we can match against internal string.
                if (internalString != string.Empty)
                {
                    float xscore = score - rules.CharIgnorePenalty * (str.Length - position - 1);
                    if (xscore >= rules.MinScoreFilter)
                    {
                        result.Add(new KeyValuePair<float, StringTreeNode>(score, this));
                    }
                }

                // We proceed matching
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].AutoComplete(str, position + 1, score, rules, result);
                }

            }
            else if (char.ToLowerInvariant(str[position]) == char.ToLowerInvariant(character))
            {
                float xscore = score + rules.CharCaseMismatchPenalty;

                // We check if we have internal string.
                if (internalString != string.Empty)
                {
                    float xxscore = score - rules.CharIgnorePenalty * (str.Length - position - 1);
                    if (xxscore >= rules.MinScoreFilter)
                    {
                        result.Add(new KeyValuePair<float, StringTreeNode>(score, this));
                    }
                }

                // We proceed matching.
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].AutoComplete(str, position + 1, xscore, rules, result);
                }
            }
            else
            {

                // Ignore character case.
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].AutoComplete(str, position, score - rules.CharIgnorePenalty, rules, result);
                }


                // Insert character case.
                for (int j = 0; j < children.Count; j++)
                {
                    children[j].AutoComplete(str, position + 1, score - rules.CharMissingPenalty, rules, result);
                }
            }

        }

        /// <summary>
        /// Matches the specified STR.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="position">The position.</param>
        /// <param name="score">The score.</param>
        /// <param name="rules">The rules.</param>
        /// <param name="result">The result.</param>
        internal protected void Match(string str, int position, float score, StringMatchingRules rules, List<KeyValuePair<float, string>> result)
        {
            // If cannot procceed, return.
            if (score < rules.MinScoreFilter) return;
            
            // This is special case if appending.
            if (str.Length == position)
            {
                // We decrement the score.
                score -= rules.CharMissingPenaltyAtEnd;
                if (score < rules.MinScoreFilter) return;
                
                // We do the search.
                if (internalString != string.Empty)
                {
                    result.Add(new KeyValuePair<float, string>(score, internalString));
                }

                // We now expand to all children.
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].Match(str, position, score, rules, result);
                }

                return;
            }

            // We first check if matched.
            if (str[position] == character)
            {
                // We check if we can match against internal string.
                if (internalString != string.Empty)
                {
                    float xscore = score - rules.CharIgnorePenalty * (str.Length - position - 1);
                    if (xscore >= rules.MinScoreFilter)
                    {
                        result.Add(new KeyValuePair<float,string>(score, internalString));
                    }
                }

                // We proceed matching
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].Match(str, position + 1, score, rules, result);
                }

            }
            else if (char.ToLowerInvariant(str[position]) == char.ToLowerInvariant(character))
            {
                float xscore = score + rules.CharCaseMismatchPenalty;

                // We check if we have internal string.
                if (internalString != string.Empty)
                {
                    float xxscore = score - rules.CharIgnorePenalty * (str.Length - position - 1);
                    if (xxscore >= rules.MinScoreFilter)
                    {
                        result.Add(new KeyValuePair<float, string>(score, internalString));
                    }
                }

                // We proceed matching.
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].Match(str, position + 1, xscore, rules, result);
                }
            }
            else
            {

                // Ignore character case.
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].Match(str, position, score - rules.CharIgnorePenalty, rules, result);
                }

                // Insert character case.
                for (int j = 0; j < children.Count; j++)
                {
                    children[j].Match(str, position + 1, score - rules.CharMissingPenalty, rules, result);
                }
            }
            
        }

        #endregion

        #region Properties

        /// <summary>
        /// The node is endnode if it contains the string we matched against.
        /// </summary>
        public bool IsEndNode
        {
            get
            {
                return internalString != string.Empty;
            }
        }

        /// <summary>
        /// The matching string.
        /// </summary>
        public string MatchString
        {
            get
            {
                return internalString;
            }
        }

        /// <summary>
        /// Gets the character this node represents.
        /// </summary>
        /// <value>The character.</value>
        public char Character
        {
            get
            {
                return character;
            }
        }

        /// <summary>
        /// The root node has no parent.
        /// </summary>
        public bool IsRoot
        {
            get
            {
                return parent == null;
            }
        }

        /// <summary>
        /// Obtains the parent node.
        /// </summary>
        public StringTreeNode Parent
        {
            get
            {
                return parent;
            }
        }

        /// <summary>
        /// Obtains a read-only collection of children.
        /// </summary>
        public ReadOnlyCollection<StringTreeNode> Children
        {
            get
            {
                return new ReadOnlyCollection<StringTreeNode>(children);
            }
        }

        /// <summary>
        /// Returns all strings from this node on.
        /// </summary>
        public List<string> AllStrings
        {
            get
            {
                // We add self.
                List<string> all = new List<string>();
                if (internalString != string.Empty) all.Add(internalString);

                // And all children.
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].AddToListInternal(all);
                }

                return all;
            }
        }

        /// <summary>
        /// Returns first string among all strings.
        /// </summary>
        public string FirstString
        {
            get
            {
                if (internalString != string.Empty) return internalString;

                for (int i = 0; i < children.Count; i++)
                {
                    string str = children[i].FirstString;
                    if (str != string.Empty) return str;
                }

                return string.Empty;
            }
        }

        #endregion

    }

    /// <summary>
    /// A string matcher class implements "heuristic" matching of strings. Given
    /// an set of possible strings and an incoming string, it returns best matching
    /// string(s).
    /// </summary>
    /// <remarks>Class is thread safe if operations are done from StringMatcher and not iterators.</remarks>
    [Serializable]
    public sealed class StringMatcher 
    {
        #region Private Members
        static StringMatchingRules defaultRules = new StringMatchingRules();
        StringTreeNode root = new StringTreeNode();
        #endregion

        #region Public Members

        /// <summary>
        /// Initializes a new instance of the <see cref="StringMatcher"/> class.
        /// </summary>
        public StringMatcher()
        {
        }

        /// <summary>
        /// Adds a string. If it already exists, nothing happens.
        /// </summary>
        /// <param name="str"></param>
        public void Add([NotEmpty] string str)
        {
            root.AddInternal(str, 0);

        }

        /// <summary>
        /// Checks if string is contained.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public bool Contains([NotEmpty] string str)
        {
            return root.ContainsInternal(str, 0);
        }

        /// <summary>
        /// Removes the string.
        /// </summary>
        /// <param name="str">The string to remove.</param>
        /// <remarks>Was the string removed.</remarks>
        public bool Remove([NotEmpty] string str)
        {
            return root.RemoveInternal(str, 0);
        }

        /// <summary>
        /// Searches the specified matching.
        /// </summary>
        /// <param name="matching">The matching.</param>
        /// <returns></returns>
        public List<KeyValuePair<float, string>> Search([NotEmpty] string matching)
        {
            return Search(matching, defaultRules);
        }

        /// <summary>
        /// We do a search using supplied rules.
        /// </summary>
        /// <param name="matching"></param>
        /// <param name="rules"></param>
        public List<KeyValuePair<float, string>> Search([NotEmpty] string matching, [NotNull] StringMatchingRules rules)
        {
            // We start the progress.
            List<KeyValuePair<float, string>> result = new List<KeyValuePair<float, string>>();

            for (int i = 0; i < root.children.Count; i++)
            {
                root.children[i].Match(matching, 0, rules.InitialScore, rules, result);
            }

            // We return the result.
            return result;
        }

        /// <summary>
        /// Auto completes the matching sequence. Instead of strings, it returns StringTreeNode at the end of matching sequence.
        /// </summary>
        /// <param name="matching">The matching.</param>
        /// <returns></returns>
        public List<KeyValuePair<float, StringTreeNode>> AutoComplete([NotEmpty] string matching)
        {
            return AutoComplete(matching, defaultRules);
        }

        /// <summary>
        /// Performs string auto completing. Instead of strings it returns node at the en of matching sequence.
        /// </summary>
        /// <param name="matching">The matching.</param>
        /// <param name="rules">The rules.</param>
        /// <returns></returns>
        public List<KeyValuePair<float, StringTreeNode>> AutoComplete([NotEmpty] string matching, [NotNull] StringMatchingRules rules)
        {
            List<KeyValuePair<float, StringTreeNode>> result = new List<KeyValuePair<float, StringTreeNode>>();

            for (int i = 0; i < root.children.Count; i++)
            {
                root.children[i].AutoComplete(matching, 0, rules.InitialScore, rules, result);
            }

            return result;
        }

        #endregion

    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class StringMatcherTest
    {
        [CorrectnessTest]
        public void SimpleSearch()
        {
            StringMatcher matcher = new StringMatcher();
            matcher.Add("MyName");
            matcher.Add("MyDog");

            List<KeyValuePair<float, string>> result = matcher.Search("MyName");
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1.0f, result[0].Key);

            result = matcher.Search("MyNa");
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(MathHelper.NearEqual(0.8f, result[0].Key, 0.05f));

            result = matcher.Search("My");
            Assert.AreEqual(2, result.Count);
        }

        [CorrectnessTest]
        public void AutoComplete()
        {
            StringMatcher matcher = new StringMatcher();
            matcher.Add("MyDog");
            matcher.Add("MyName2");
            matcher.Add("MyName1");
            matcher.Add("MyName3");

            List<KeyValuePair<float, StringTreeNode>> result = matcher.AutoComplete("MyName");
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual('e', result[0].Value.Character);
        }

    }
#endif
}
