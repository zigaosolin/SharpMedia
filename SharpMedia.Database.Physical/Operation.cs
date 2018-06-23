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

namespace SharpMedia.Database.Physical
{
    /// <summary>
    /// Operation defines how something is done.
    /// </summary>
    public interface IOperation
    {
        /// <summary>
        /// Preapres operation. Operation is given read-only access
        /// and is required to fill in operation data.
        /// </summary>
        /// <param name="readService"></param>
        /// <param name="data"></param>
        void Prepare(Journalling.IReadService readService,
                    out Journalling.OperationStartupData data);

        /// <summary>
        /// Number of operation stages (usually only one).
        /// </summary>
        uint StageCount { get; }

        /// <summary>
        /// Executes ceratin stage of operation.
        /// </summary>
        /// <param name="stage">The stage index.</param>
        /// <param name="service">Service, holds all state of operation.</param>
        void Execute(uint stage, Journalling.IService service);
    }
}
