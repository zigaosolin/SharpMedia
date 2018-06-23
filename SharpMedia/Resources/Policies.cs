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

namespace SharpMedia.Resources
{
    /// <summary>
    /// A loading policy descriptor.
    /// </summary>
    public sealed class LoadingPolicy
    {
        /// <summary>
        /// This means that references, transfered through network, first
        /// check if such resource already exists on local computer storage.
        /// </summary>
        public bool AllowLocalMachineLocating = true;

        /// <summary>
        /// This parameter allows the search over network loaders.
        /// </summary>
        public bool AllowNetworkSearch = false;

        /// <summary>
        /// Should the loading be delayed.
        /// </summary>
        public bool DelayLoading = true;



        

    }
}
