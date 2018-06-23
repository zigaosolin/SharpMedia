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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Remoting;
using System.Reflection;
using System.Threading;

using SharpMedia.Testing;
using SharpMedia.AspectOriented;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SharpMedia
{

    /// <summary>
    /// A changeable event.
    /// </summary>
    public interface IPreChangeNotifier
    {
        /// <summary>
        /// Fired when data in the display object changes significantly enough to warrant a redraw.
        /// </summary>
        event Action<IPreChangeNotifier> OnChange;
    }

    /// <summary>
    /// A comparable pair, holds pair and allows sorting (first on first, then on second).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public struct ComparablePair<T, U> : IComparable<ComparablePair<T, U>> 
        where U : IComparable<U> where T : IComparable<T>
    {
        public T Key;
        public U Value;

        public ComparablePair(T key, U value)
        {
            this.Key = key;
            this.Value = value;
        }

        #region IComparable<ComparablePair<T,U>> Members

        public int CompareTo(ComparablePair<T, U> other)
        {
            int c = Key.CompareTo(other.Key);
            if (c == 0)
            {
                return Value.CompareTo(other.Value);
            }
            return c;
        }

        #endregion
    }

    /// <summary>
    /// A generic version of cloneable interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICloneable<T>
    {
        /// <summary>
        /// Clones self.
        /// </summary>
        /// <returns></returns>
        T Clone();
    }

    /// <summary>
    /// The placement of object. This is useful flag for resources, cachable
    /// objects etc.
    /// </summary>
    [Flags]
    public enum Placement : int
    {
        /// <summary>
        /// No placement.
        /// </summary>
        NoPlacement = 0,

        /// <summary>
        /// The memory placement.
        /// </summary>
        Memory = 1,

        /// <summary>
        /// DMA (direct memory access) memory.
        /// </summary>
        DMAMemory = 2,

        /// <summary>
        /// Memory of specific hardware device.
        /// </summary>
        DeviceMemory = 4,

        /// <summary>
        /// Resource resides on harddrive, it can be loaded from there.
        /// </summary>
        HardDrive = 8,

        /// <summary>
        /// Resides remotely on computer, memory is not specified (in can be system, DMA or device).
        /// </summary>
        Remote = 16,

        /// <summary>
        /// Remote harddrive memory.
        /// </summary>
        RemoteHarddrive = 32
    }

    /// <summary>
    /// Frequency of update.
    /// </summary>
    public enum UpdateFrequency
    {
        /// <summary>
        /// Per vertex update to new element.
        /// </summary>
        PerVertex,

        /// <summary>
        /// Per instance update to new element.
        /// </summary>
        PerInstance
    }

    /// <summary>
    /// The resource is placable.
    /// </summary>
    public interface IPlacable
    {
        /// <summary>
        /// A placement of placable.
        /// </summary>
        Placement Placement { get; }
    }

    /// <summary>
    /// Usage of resource.
    /// </summary>
    public enum Usage
    {
        /// <summary>
        /// Readable and writable by device.
        /// </summary>
        Default,

        /// <summary>
        /// Readable by device, writable by CPU.
        /// </summary>
        Dynamic,

        /// <summary>
        /// Readable and writable by CPU, readable by device.
        /// </summary>
        Staging,

        /// <summary>
        /// The static (immutable) resource, once specified by CPU and then never updated neither
        /// by CPU nor GPU.
        /// </summary>
        Static
    }

    /// <summary>
    /// Map options of mapable.
    /// </summary>
    public enum MapOptions
    {
        /// <summary>
        /// Only read is allowed.
        /// </summary>
        Read,

        /// <summary>
        /// Only write is allowed.
        /// </summary>
        Write,

        /// <summary>
        /// Read-write is allowed.
        /// </summary>
        ReadWrite,

    }

    /// <summary>
    /// A mappable interface. Used by resources.
    /// </summary>
    public interface IMapable<T>
    {
        /// <summary>
        /// Locks the resource, outputting data.
        /// </summary>
        /// <returns></returns>
        T Map(MapOptions op);

        /// <summary>
        /// Unmaps the resource.
        /// </summary>
        void UnMap();
    }

    /// <summary>
    /// A range lockable interface. Can map a certain range of data.
    /// </summary>
    /// <typeparam name="T">Basic data type</typeparam>
    public interface IRangeMapable<T> : IMapable<T>
    {
        /// <summary>
        /// Maps data in range.
        /// </summary>
        T Map(MapOptions usage, ulong offset, ulong count);
    }

    /// <summary>
    /// A LOD lockable interface, used by textures (mipmaps) and geometry.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILODMapable<T> : IMapable<T>
    {
        /// <summary>
        /// Can map certain level of detail (mipmap, geometry LOD ...).
        /// </summary>
        T Map(MapOptions usage, uint lod);
    }

    /// <summary>
    /// A documentable entry. Comments can be added to classes
    /// through this interface. 
    /// </summary>
    public interface ICommentable
    {
        /// <summary>
        /// Description of entry.
        /// </summary>
        string Comment { get; set; }
    }

    /// <summary>
    /// Some common routines, shared between all of SharpMedia.
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// Capitalizes a string
        /// </summary>
        /// <param name="original">The string to capitalize</param>
        /// <returns>a capitalized string</returns>
        /// <example>Will convert abcd to Abcd and camelCase to CamelCase</example>
        public static string StringCapitalize([NotNull] string original)
        {
            if(original.Length == 0) return original;
            return Char.ToUpper(original[0]) + original.Substring(1);
        }

        /// <summary>
        /// Performans a wildcards matching.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool WildcardsMatch(string pattern, string stringToMatch)
        {
            // A very naive implementation of wildcards matching
            // FIXME: optimize (non recursive, possilby with pointers).

            int j = 0;
            for (int i = 0; i < stringToMatch.Length; i++, j++)
            {
                if (j >= pattern.Length) return false;

                char ch = stringToMatch[i];

                char m = pattern[j];

                switch (m)
                {
                    case '?':
                        break;
                    case '*':
                        // We advance over *s
                        do { j++; } while (j < pattern.Length && pattern[j] == '*');

                        // If ended with *, end it now.
                        if (j >= pattern.Length) return true;

                        for (int z = i; z < stringToMatch.Length; z++)
                        {
                            if (WildcardsMatch(pattern.Substring(j), stringToMatch.Substring(z))) return true;
                        }
                        return false;
                    default:
                        if (m != ch) return false;
                        break;

                }
                

            }

            return true;
        }

        /// <summary>
        /// Obtains supported casts of a type
        /// </summary>
        /// <param name="type">Type to query supported casts for</param>
        /// <returns>Array of types with supported casts</returns>
        public static Type[] SupportedCasts(Type type, bool includingSystem)
        {
            List<Type> supportedCasts = new List<Type>();
            supportedCasts.Add(type);

            Type baset = type.BaseType;
            while (baset != null)
            {
                if (!supportedCasts.Contains(baset))
                {
                    if (!baset.FullName.StartsWith("System") || includingSystem)
                    {
                            supportedCasts.Add(baset);
                    }
                }
                else break;
                baset = baset.BaseType;
            }

            foreach (Type iface in type.GetInterfaces())
            {
                supportedCasts.Add(iface);
            }

            return supportedCasts.ToArray();
        }

        /// <summary>
        /// Checks whether a type is a direct or indirect ancestor of another type.
        /// </summary>
        /// <param name="a">The type that is the potential ancestor</param>
        /// <param name="b">The type that is checked with (potential (grand)child)</param>
        /// <returns></returns>
        public static bool IsTypeSameOrDerived(Type a, Type b)
        {
            if (a == b) return true;
            if (a == null || b == null) return false;
            if (a == typeof(object)) return true;
            if (b.IsSubclassOf(a)) return true;
            foreach (Type iface in b.GetInterfaces()) { if (iface == a) return true; }
            
            return false;
        }

        /// <summary>
        /// Converts array to string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ArrayToString<T>(IList<T> list)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            for (int i = 0; i < list.Count; i++)
            {
                if (i != 0) builder.Append(",");
                builder.Append(list[i].ToString());
            }
            return builder.Append("}").ToString();
        }

        /// <summary>
        /// Copies from src to dst.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="n"></param>
        public static unsafe void Memcpy<T>(void* src, T[] dst, ulong n)
        {
            GCHandle handle = GCHandle.Alloc(dst, GCHandleType.Pinned);

            try
            {
                void* ptr = handle.AddrOfPinnedObject().ToPointer();

                Memcpy(src, dst, n);

            }
            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// Copies n bytes from src to dst.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="bytes"></param>
        public static unsafe void Memcpy(void* src, void* dst, ulong n)
        {
            ulong* s = (ulong*)src;
            ulong* d = (ulong*)dst;

            // We use ulong for copy.
            ulong i;
            ulong n_8 = n / 8;
            for (i = 0; i < n_8; i++)
            {
                d[i] = s[i];
            }


            i *= 8;

            // The rest is copied per-byte.
            for (; i < n; i++)
            {
                ((byte*)dst)[i] = ((byte*)src)[i];
            }
        }

        /// <summary>
        /// Copies n bytes from source to dest. Source is mapped to buffer.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="n"></param>
        public static unsafe void Memcpy<T>( T[] src, void* dst, ulong n)
        {
            GCHandle handle = GCHandle.Alloc(src, GCHandleType.Pinned);

            try
            {
                void* ptr = handle.AddrOfPinnedObject().ToPointer();

                Memcpy(ptr, dst, n);

            }
            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// Find the type as requireed
        /// </summary>
        /// <param name="name">The name of the type</param>
        /// <returns>The type found</returns>
        public static Type FindType(string name)
        {
            return Type.GetType(name);
        }

        /// <summary>
        /// The version of the SharpMedia runtime
        /// </summary>
        public const string VersionString = "SharpMedia, v.1.0.5";

        public const string DistributionInfoString =
            "This distribution and all of its contents are owned in full by the SharpMedia team.\n" +
            "All rights reserved, Copyright (c) 2006-2007, based in part on work (c) 1999-2006.\n" +
            "For more information, contact via e-mail at: enquiry@sharpmedia.com.\n" +
            "Enjoy Our Release!";


        public const string LicenceInfoString =
            "This file constitutes a part of the SharpMedia project, (c) 2007 by the SharpMedia team\n" +
            "and is licensed for your use under the conditions of the NDA or other legally binding contract\n" +
            "that you or a legal entity you represent has signed with the SharpMedia team.\n" +
            "In an event that you have received or obtained this file without such legally binding contract\n" +
            "in place, you MUST destroy all files and other content to which this lincese applies and\n" +
            "contact the SharpMedia team for further instructions at the internet mail address:\n" +
            "legal@sharpmedia.com";

        public const string LicenceInfoStringAsComment =
            "// This file constitutes a part of the SharpMedia project, (c) 2007 by the SharpMedia team\n" +
            "// and is licensed for your use under the conditions of the NDA or other legally binding contract\n" +
            "// that you or a legal entity you represent has signed with the SharpMedia team.\n" +
            "// In an event that you have received or obtained this file without such legally binding contract\n" +
            "// in place, you MUST destroy all files and other content to which this lincese applies and\n" +
            "// contact the SharpMedia team for further instructions at the internet mail address:\n" +
            "//\n" +
            "//    legal@sharpmedia.com\n" +
            "//\n";

        /// <summary>
        /// The generation number
        /// </summary>
        public const int VersionGeneration = 1;

        /// <summary>
        /// The major version
        /// </summary>
        public const int VersionMajor      = 0;

        /// <summary>
        /// The minor version
        /// </summary>
        public const int VersionMinor      = 5;

        /// <summary>
        /// The file name extension for assembly files (dlls)
        /// </summary>
        public const string AssemblyFileExtension = ".dll";

        /// <summary>
        /// Serializes the object to stream.
        /// </summary>
        /// <param name="stream">The actual block write stream, created by allocation.</param>
        /// <param name="obj">The object to write.</param>
        public static void SerializeToStream([NotNull] Stream stream, object obj)
        {
            // Serialize the object; may result in exception thrown.
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
        }

        /// <summary>
        /// Serializes to object to array.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static byte[] SerializeToArray(object obj)
        {
            MemoryStream stream = new MemoryStream(1024);
            SerializeToStream(stream, obj);
            return stream.ToArray();
        }

        /// <summary>
        /// Deserializes object from stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>Object deserialized.</returns>
        public static object DeserializeFromStream([NotNull] Stream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(stream);
        }

        /// <summary>
        /// Deserializes object from bytes.
        /// </summary>
        /// <param name="bytes">The raw serialized bytes.</param>
        /// <returns>Object deserialized.</returns>
        public static object DeserializeFromArray([NotNull] byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            IFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(stream);
        }

        /// <summary>
        /// Calculates the size of serialized object. This is usually called in preprocessing.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>The size of object in bytes.</returns>
        /// <remarks>The cost of this method is near the cost of serialize to array. If you will
        /// also need the byte[] array, prefer that method.</remarks>
        public static ulong GetSerializedObjectSize(object obj)
        {
            FakeStream stream = new FakeStream();

            // Serialize the object.
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);

            // Output length.
            return (ulong)stream.Length;
        }

        /// <summary>
        /// Deserializes an object from a byte[] buffer
        /// </summary>
        /// <param name="b">The buffer to deserialize from</param>
        /// <returns>The deserialized object</returns>
        public static object DeserializeObject([NotNull] byte[] b)
        {
            IFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(new MemoryStream(b));
        }

        public static object DeserializeFromSoapStream([NotNull] Stream stream)
        {
            IFormatter formatter = new SoapFormatter();
            return formatter.Deserialize(stream);
        }

        public static void SerializeToSoapStream([NotNull] Stream stream, object p)
        {
            IFormatter formatter = new SoapFormatter();
            formatter.Serialize(stream, p);
            stream.Close();
        }


        static volatile Logging.ILoggerManager logManager;
        static Logging.ILogger nullLogger = new Logging.NullLoger();
        static Logging.ILogger GetLogger(Type type)
        {
            if(type == null) return nullLogger;
            
            Logging.ILoggerManager manager = logManager;
            if(manager != null)
            {
                Logging.ILogger logger = manager[type];
                return logger != null ? logger : nullLogger;
            }
            return nullLogger;
        }

        /// <summary>
        /// Configures logging for the root-level appdomain.
        /// </summary>
        /// <param name="manager">Parameter may be null, indicating no manager is used to log.</param>
        /// <remarks></remarks>
        public static void ConfigureLogging(Logging.ILoggerManager manager)
        {
            logManager = manager;
        }


        /// <summary>
        /// Called by a deprecated method when it is invoked
        /// </summary>
        /// <param name="log">a log to use for logging</param>
        /// <param name="name">the name of the method</param>
        /// <param name="assembly">the name of the calling assembly</param>
        public static void MethodDeprecated([NotNull] Type type, [NotEmpty] string name, [NotEmpty] string assembly)
        {
            Logging.ILogger logger = GetLogger(type);
            logger.Warning(string.Format("Method '{0}.{1}' is depricated but was still called from assembly '{2}'",
                type.FullName, name, assembly));
            
        }

        /// <summary>
        /// Issues a debug message.
        /// </summary>
        /// <param name="type">The type of class/...</param>
        /// <param name="message">The message of warning.</param>
        [Conditional("DEBUG")]
        public static void Debug([NotNull] Type type, [NotEmpty] string message)
        {
            Logging.ILogger logger = GetLogger(type);
            logger.Message(string.Format("Debug: {0}", message));
        }

        /// <summary>
        /// Issues a formatted debug message.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">Message with formats.</param>
        /// <param name="data">The data into message.</param>
        [Conditional("DEBUG")]
        public static void DebugFormatted([NotNull] Type type, [NotEmpty] string message, [NotNull] params object[] data)
        {
            Logging.ILogger logger = GetLogger(type);
            logger.Message("Debug: " + string.Format(message, data));
        }

        /// <summary>
        /// Issues a error.
        /// </summary>
        /// <param name="type">The type of class/...</param>
        /// <param name="message">The message of warning.</param>
        public static void Error([NotNull] Type type, [NotEmpty] string message)
        {
            Logging.ILogger logger = GetLogger(type);
            logger.Error(message);
        }

        /// <summary>
        /// Issues a formatted error.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">Message with formats.</param>
        /// <param name="data">The data into message.</param>
        public static void ErrorFormatted([NotNull] Type type, [NotEmpty] string message, [NotNull] params object[] data)
        {
            Logging.ILogger logger = GetLogger(type);
            logger.Error(string.Format(message, data));
        }

        /// <summary>
        /// Issues a warning.
        /// </summary>
        /// <param name="type">The type of class/...</param>
        /// <param name="message">The message of warning.</param>
        public static void Warning([NotNull] Type type, [NotEmpty] string message)
        {
            Logging.ILogger logger = GetLogger(type);
            logger.Warning(message);
        }

        /// <summary>
        /// Issues a formatted warning.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">Message with formats.</param>
        /// <param name="data">The data into message.</param>
        public static void WarningFormatted([NotNull] Type type, [NotEmpty] string message, [NotNull] params object[] data)
        {
            Logging.ILogger logger = GetLogger(type);
            logger.Warning(string.Format(message, data));
        }

        /// <summary>
        /// Issues not-diposed warning.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        public static void NotDisposed([NotNull] object obj, [NotEmpty] string message)
        {
            Logging.ILogger logger = GetLogger(obj.GetType());
            logger.Warning(string.Format("Object '{0}' not disposed: {1}", obj, message));
        }

        /// <summary>
        /// Issues not-diposed warning.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        public static void NotDisposedFormatted([NotNull] object obj, [NotEmpty] string message, [NotNull] params object[] data)
        {
            Logging.ILogger logger = GetLogger(obj.GetType());
            logger.Warning(string.Format("Object '{0}' not disposed: {1}", obj, string.Format(message, data)));
        }

        /// <summary>
        /// A testing directory.
        /// </summary>
        public static string TestDirectory 
        { 
            get { return "TestData/"; }
        }

        /// <summary>
        /// Returns true if the value value is found in the array array, false otherwise.
        /// </summary>
        public static bool ArrayContains<T>([NotNull] T[] array, T value)
        {
            foreach (T t in array)
            {
                if (t.Equals(value)) return true;
            }
            return false;
        }

        /// <summary>
        /// Merges the arrays.
        /// </summary>
        /// <param name="array1">The array1.</param>
        /// <param name="array2">The array2.</param>
        /// <returns>A merged array</returns>
        public static T[] ArrayMerge<T>([NotNull] T[] array1, [NotNull] T[] array2)
        {
            T[] merged = new T[array1.Length + array2.Length];

            Array.Copy(array1, merged, array1.Length);

            Array.Copy(array2, 0, merged, array1.Length, array2.Length);

            return merged;
        }

        /// <summary>
        /// Loads the file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>byte[] content of the file</returns>
        public static byte[] LoadFile([NotEmpty] string filename)
        {
            return File.ReadAllBytes(filename);
        }

        /// <summary>  
        /// This method adds the items in the first list to the second list.  
        /// This method is  helpful because even when X inherits from Y, List<X> does not inherit from List<Y>, and so you cannot cast between them, only copy.  
        /// </summary>  
        /// <typeparam name="FROM_TYPE"></typeparam>  
        /// <typeparam name="TO_TYPE"></typeparam>  
        /// <param name="listToCopyFrom"></param>  
        /// <param name="listToCopyTo"></param>  
        /// <returns></returns>  
        public static List<TO_TYPE> AddRange<FROM_TYPE, TO_TYPE>([NotNull] List<FROM_TYPE> listToCopyFrom, [NotNull] List<TO_TYPE> listToCopyTo) where FROM_TYPE : TO_TYPE  
        {     
            // loop through the list to copy, and  
            foreach ( FROM_TYPE item in listToCopyFrom )  
            {  
                // add items to the copy tolist  
                listToCopyTo.Add( item );  
            }  
              
            // return the copy to list  
            return listToCopyTo;  
        } 
    }

#if SHARPMEDIA_TESTSUITE

    /// <summary>
    /// Tests the Common.IsTypeSameOrDerived method
    /// </summary>
    [TestSuite]
    internal class CommonTestTypeDerivation {

        class A {}
        class B : A {}
        class C : B {}
        class D {}

        public Action2<uint, object> Dummy() { return null; }

        [CorrectnessTest]
        public void DelegateSignature()
        {

            // We now try to inspect it.
            Type type = typeof(CommonTestTypeDerivation);
            ParameterInfo delegateType = type.GetMethod("Dummy").ReturnParameter;

            type = delegateType.ParameterType;
            MethodInfo invoke = type.GetMethod("Invoke");

            ParameterInfo[] info = invoke.GetParameters();
        }

        [CorrectnessTest]
        public void PrimitiveTypesSame() { Assert.IsTrue(Common.IsTypeSameOrDerived(typeof(bool), typeof(bool))); }
        [CorrectnessTest]
        public void PrimitiveTypesNotSame() { Assert.IsFalse(Common.IsTypeSameOrDerived(typeof(int), typeof(bool))); }
        [CorrectnessTest]
        public void ClassesSame() { Assert.IsTrue(Common.IsTypeSameOrDerived(typeof(A), typeof(A))); }
        [CorrectnessTest]
        public void DerivedClassesSame() { Assert.IsTrue(Common.IsTypeSameOrDerived(typeof(A), typeof(B))); }
        [CorrectnessTest]
        public void OrderReversed() { Assert.IsFalse(Common.IsTypeSameOrDerived(typeof(B), typeof(A))); }
        [CorrectnessTest]
        public void DoubleDerivedClassesSame() { Assert.IsTrue(Common.IsTypeSameOrDerived(typeof(A), typeof(C))); }
        [CorrectnessTest]
        public void UnconnectedClassesSame() { Assert.IsFalse(Common.IsTypeSameOrDerived(typeof(A), typeof(D))); }
        [CorrectnessTest]
        public void ReverseUnconnectedClassesSame() { Assert.IsFalse(Common.IsTypeSameOrDerived(typeof(D), typeof(A))); }
    }
#endif
}
