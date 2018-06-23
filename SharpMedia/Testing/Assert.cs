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

namespace SharpMedia.Testing
{

    /// <summary>
    /// Assert exception
    /// </summary>
    [Serializable]
    public class AssertException : Exception
    {
        public AssertException() : base("Assertion failed") { }
        public AssertException(string message) : base(message) { }
        public AssertException(string message, Exception inner) : base(message, inner) { }
        protected AssertException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// An assertion class.
    /// </summary>
    public static class Assert
    {
        /// <summary>
        /// Checks if true.
        /// </summary>
        /// <param name="value"></param>
        public static void IsTrue(bool value)
        {
            if (!value) throw new AssertException();
        }

        /// <summary>
        /// Asserts that object must be null.
        /// </summary>
        /// <param name="obj"></param>
        public static void IsNull(object obj)
        {
            if (obj != null) throw new AssertException();
        }

        /// <summary>
        /// Asserts that object is not null.
        /// </summary>
        /// <param name="obj"></param>
        public static void IsNotNull(object obj)
        {
            if (obj == null) throw new AssertException();
        }

        /// <summary>
        /// Checks if false.
        /// </summary>
        /// <param name="value"></param>
        public static void IsFalse(bool value)
        {
            if (value) throw new AssertException();
        }

        /// <summary>
        /// Checks if objects are equal.
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        public static void AreEqual(object obj1, object obj2)
        {
            if (object.ReferenceEquals(obj1, obj2)) return;

            if (obj1 == null || !obj1.Equals(obj2)) throw new AssertException();
        }

        /// <summary>
        /// Checks if not equal.
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        public static void NotEqual(object obj1, object obj2)
        {
            if (object.ReferenceEquals(obj1, obj2)) throw new AssertException();

            if (obj1 != null && obj1.Equals(obj2)) throw new AssertException();
        }



    }
}
