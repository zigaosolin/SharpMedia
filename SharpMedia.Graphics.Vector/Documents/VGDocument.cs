using System;
using System.Collections.Generic;
using System.Text;
using SharpMedia.Components.Configuration;

namespace SharpMedia.Graphics.Vector.Documents
{

    /// <summary>
    /// A vector graphics's document is collection of vector graphics entities.
    /// </summary>
    [Serializable]
    public sealed class VGDocument : VGScope
    {
        public VGDocument(string name) : base(name) { }
    }
}
