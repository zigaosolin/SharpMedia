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
using SharpMedia.Graphics.Vector;

namespace SharpMedia.Graphics.GUI.Styles.SubStyles
{
    /// <summary>
    /// Allows logic for transforming mapping coordinates.
    /// </summary>
    public abstract class TransformableBase : IPreChangeNotifier
    {
        #region Private Members
        object syncRoot = new object();
        protected Action<IPreChangeNotifier> onChange;

        ITransform mappingTransform = null;
        IMapper mapper = null;

        protected void Changed()
        {
            Action<IPreChangeNotifier> n = onChange;
            if (n != null)
            {
                n(this);
            }
        }

        protected void ChangeDelegate(IPreChangeNotifier n)
        {
            Changed();
        }

        protected abstract TransformableBase TransformableParent
        {
            get;
        }

        #endregion

        #region Public Members

        public TransformableBase()
        {
        }

        /// <summary>
        /// Gets or sets mapping transform.
        /// </summary>
        public ITransform MappingTransform
        {
            get
            {
                return mappingTransform;
            }
            set
            {
                mappingTransform = value;
                Changed();
            }
        }

        /// <summary>
        /// Gets or sets custom mapper.
        /// </summary>
        public IMapper Mapper
        {
            get
            {
                return mapper;
            }
            set
            {
                mapper = value;
                Changed();
            }
        }

        #endregion

        #region IPreChangeNotifier Members

        public event Action<IPreChangeNotifier> OnChange
        {
            add
            {
                lock (syncRoot)
                {
                    onChange += value;
                }
            }
            remove
            {
                lock (syncRoot)
                {
                    onChange -= value;
                }
            }
        }

        #endregion
    }
}
