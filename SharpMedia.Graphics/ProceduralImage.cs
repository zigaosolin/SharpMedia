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

namespace SharpMedia.Graphics
{

    /// <summary>
    /// A delegate used for updating image.
    /// </summary>
    /// <param name="image">The actual image to update. Use lock/unlock mechanism to do that.</param>
    /// <param name="data">The used asociated data, used to update.</param>
    public delegate void UpdateImage([NotNull] ProceduralImage image, object data);

    /// <summary>
    /// Image that is dynamically updated. This image type is useful for creating
    /// images on-the-fly, such as some random distortion fields ...
    /// </summary>
    public abstract class ProceduralImage : Image
    {
        /// <summary>
        /// A data asociated with dynamic updating.
        /// </summary>
        protected object argumentData;

        /// <summary>
        /// The procedure.
        /// </summary>
        protected UpdateImage procedure;

        /// <summary>
        /// The update argument setter.
        /// </summary>
        public object UpdateArgument
        {
            get 
            { 
                return argumentData; 
            }
            set 
            {
                lock (syncRoot)
                {
                    argumentData = value;
                }
            }
        }

        /// <summary>
        /// The procedure accessor.
        /// </summary>
        public UpdateImage Procedure
        {
            set
            {
                lock (syncRoot)
                {
                    procedure = value;
                }
            }
            get
            {
                return procedure;
            }
        }

        /// <summary>
        /// Updates the image, if procedure is set.
        /// </summary>
        public void Update()
        {
            UpdateImage u = procedure;
            if (u != null)
            {
                u(this, argumentData);
            }
            
        }
    }
}
