using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMedia.Scene
{
    /// <summary>
    /// A notification interface for scene components.
    /// </summary>
    public interface ISCNotify
    {
        /// <summary>
        /// Called before component is attached to node.
        /// </summary>
        /// <param name="node">The scene node.</param>
        /// <remarks>Can return false, which means that this is invalid attachment.
        /// If you want to give more information, you can throw a custom exception.</remarks>
        bool AttachedTo(SceneNode node);

        /// <summary>
        /// Called before scene component is detached from node.
        /// </summary>
        /// <param name="node">The scene node.</param>
        /// <returns>Can return false if detaching scene component is invalid (for example,
        /// there are dependencies that must be detached first).</returns>
        bool DetachedFrom(SceneNode node);

    }

    /// <summary>
    /// Assigns a queriable name to component.
    /// </summary>
    public interface ISCNamed
    {
        /// <summary>
        /// Obtains the name of component.
        /// </summary>
        string Name { get; }

    }
}
