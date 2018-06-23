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
using System.Reflection;

namespace SharpMedia.Loading
{
    public class GenericLoadableFactory<Loadable> : ILoadableFactory 
        where Loadable: class, ILoadable, new()
    {
        #region ILoadableFactory Members

        public object Load(System.IO.Stream s)
        {
            Loadable l = new Loadable();
            return l.Load(s) ? l : null;
        }

        public ulong? Size(object value)
        {
            return (value as ILoadable).Size;
        }

        public void Save(object value, Stream s)
        {
            (value as ILoadable).Save(s);
        }

        public bool CanLoadMore(Stream s)
        {
            if (s.Length - 1 <= s.Position) return false;

            MethodInfo methodInfo = typeof(Loadable).GetMethod("Identify", BindingFlags.Static);
            if (methodInfo != null)
            {
                return (bool)methodInfo.Invoke(null, new object[] { s });
            }

            return false;
        }

        public Type LoadableType
        {
            get { return typeof(Loadable); }
        }

        #endregion
    }
}
