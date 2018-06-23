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
using SharpMedia.Graphics.Shaders.Operations;
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics.Shaders
{
    /// <summary>
    /// An operation's scope. A scope is defined by ShaderCode or by special scoping operations.
    /// </summary>
    public interface IScope
    {
        /// <summary>
        /// Scope was changed.
        /// </summary>
        void SignalChanged();

        /// <summary>
        /// Checks if the scope is within other scope.
        /// </summary>
        /// <param name="other">The other scope.</param>
        bool IsInScope([NotNull] IScope other);

        /// <summary>
        /// Obtains parent scope of this scope. Can be null if this is
        /// topmost scope.
        /// </summary>
        IScope ParentScope
        {
            get;
        }
    }

    /// <summary>
    /// Helps with scope validation.
    /// </summary>
    public static class ScopeHelper
    {

        /// <summary>
        /// Checks if valid in terms of scopes.
        /// </summary>
        /// <param name="op">The operation is question.</param>
        /// <returns></returns>
        public static bool IsScopeValid([NotNull] IOperation op)
        {
            IScope s = op.Scope;
            if (s == null) return false;

            // We now check if all inputs are from correct scopes.
            Pin[] inputs = op.Inputs;
            if (inputs == null) return false;

            foreach (Pin p in inputs)
            {
                IOperation owner = p.Owner;
                IScope ownerScope = owner.Scope;

                if (ownerScope == null) return false;
                if (!s.IsInScope(ownerScope)) return false;
            }

            return true;
        }

        /// <summary>
        /// Obtains the root scope of specified scope.
        /// </summary>
        /// <param name="scope">The specified scope.</param>
        /// <returns>Returning scope.</returns>
        public static IScope GetRootScope([NotNull] IScope scope)
        {
            while (scope.ParentScope != null)
            {
                scope = scope.ParentScope;
            }
            return scope;
        }

        /// <summary>
        /// Obtains scope of operation.
        /// </summary>
        /// <param name="scope">The in/out scope.</param>
        /// <param name="pins">Input pins of operation.</param>
        public static void GetScope(ref IScope scope, params Pin[] pins)
        {
            // Signals on changed before making any changes.
            if (scope != null)
            {
                scope.SignalChanged();
            }

            IScope deepest = null;
            if(scope != null) deepest = GetRootScope(scope);

            foreach (Pin p in pins)
            {
                if (deepest != null && p.Owner.Scope.IsInScope(deepest))
                {
                    deepest = p.Owner.Scope;
                } 
                // Make validation.
                else if(deepest != null && p.Owner.Scope != deepest && !deepest.IsInScope(p.Owner.Scope)) 
                {
                    throw new InvalidOperationException("Trying to construct operation from two unrelated scopes.");
                }
            }

            // Signal the deepest scope that it was changed (may throw).
            if (deepest != null)
            {
                deepest.SignalChanged();
            }

            // We have found the deepest scope.
            scope = deepest;
        }

    }
}
