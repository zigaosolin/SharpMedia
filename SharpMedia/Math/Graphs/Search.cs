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

namespace SharpMedia.Math.Graphs
{

    public enum SearchStatus
    {
        /// <summary>
        /// Search is running.
        /// </summary>
        Running,

        /// <summary>
        /// Search is running and has at least one solution; may be suboptimal
        /// or not the only one.
        /// </summary>
        RunningWithSolution,

        /// <summary>
        /// No solution found.
        /// </summary>
        NotFound,

        /// <summary>
        /// Search is complete.
        /// </summary>
        Complete
    }

    /// <summary>
    /// An search interface.
    /// </summary>
    public interface ISearch
    {
        /// <summary>
        /// The status of search.
        /// </summary>
        SearchStatus Status { get; }

        /// <summary>
        /// A next iteration of search. Iteration can be any length. The algorithm
        /// is required to stop iteration when new result is obtained. Iteration can
        /// also stop if number of operation exceed.
        /// </summary>
        /// <returns>Is the search complete.</returns>
        bool NextIteration();

        /// <summary>
        /// Searches full space at once.
        /// </summary>
        void Search();

        /// <summary>
        /// How much of searhing (approximation) is done. Can be used in MT context.
        /// </summary>
        /// <remarks>Always return 0.0f if not supported.</remarks>
        float DoneRatio { get; }

        /// <summary>
        /// Obtains full or temporary result.
        /// </summary>
        SearchResult Result { get; }

        /// <summary>
        /// Obtains all optimal results of the same cost.
        /// </summary>
        SearchResult[] Results { get; }
    }

}
