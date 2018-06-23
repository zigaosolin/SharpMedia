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

namespace SharpMedia.Loading
{
    /// <summary>
    /// Raw loading factory.
    /// </summary>
    /// <remarks>Serves objects as byte[].</remarks>
    public sealed class RawLoadableFactory : ILoadableFactory
    {

        public RawLoadableFactory()
        {
        }

        #region ILoadableFactory Members

        public object Load(System.IO.Stream s)
        {
            long length = s.Length - s.Position;
            byte[] data = new byte[length];

            s.Read(data, 0, (int)length);
            return data;
        }

        public void Save(object value, System.IO.Stream s)
        {
            byte[] value2 = value as byte[];
            s.Write(value2, 0, value2.Length);
        }

        public ulong? Size(object value)
        {
            return (ulong)(value as byte[]).Length;
        }

        public bool CanLoadMore(System.IO.Stream s)
        {
            return false;
        }

        public Type LoadableType
        {
            get { return typeof(byte[]); }
        }

        #endregion
    }
}
