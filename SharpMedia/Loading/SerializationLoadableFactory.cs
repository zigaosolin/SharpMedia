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
    /// A serialization loadable factory.
    /// </summary>
    public sealed class SerializationLoadableFactory : ILoadableFactory
    {
        #region ILoadableFactory Members

        public object Load(System.IO.Stream s)
        {
            return Common.DeserializeFromStream(s);
        }

        public void Save(object value, System.IO.Stream s)
        {
            Common.SerializeToStream(s, value);
        }

        public ulong? Size(object value)
        {
            return Common.GetSerializedObjectSize(value);
        }

        public bool CanLoadMore(System.IO.Stream s)
        {
            return s.Position < s.Length - 1;
        }

        public Type LoadableType
        {
            get { return typeof(object); }
        }

        #endregion
    }
}
