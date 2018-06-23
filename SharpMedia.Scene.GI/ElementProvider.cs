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
using SharpMedia.Math;

namespace SharpMedia.Scene.GI
{

    /// <summary>
    /// Read-write element holder.
    /// </summary>
    public class Element
    {
        /// <summary>
        /// Position of the element.
        /// </summary>
        public Vector3d Position;

        /// <summary>
        /// Normal of the element.
        /// </summary>
        public Vector3d Normal;
    }

    /// <summary>
    /// Element providers are classes that can provide elements for GI algorithms based
    /// on certain criteria. 
    /// </summary>
    /// <remarks>
    /// One can provide vertices as elements, triangles as elements, texels as elements ...
    /// Element provider is also capable of doing dynamic subdivision for element using
    /// element feedbacks.
    /// </remarks>
    public interface IElementProvider
    {
        /// <summary>
        /// Obtains next element. Element is also held by provider to write feedback.
        /// </summary>
        /// <returns>The element, or null if no more element exists.</returns>
        Element NextElement();   

    }
}
