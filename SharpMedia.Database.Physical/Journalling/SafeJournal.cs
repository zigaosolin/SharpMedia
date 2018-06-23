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

namespace SharpMedia.Database.Physical.Journalling
{
    /// <summary>
    /// A safe journal class.
    /// </summary>
    public class SafeJournal : IJournal
    {

        #region Private Members
        Allocator allocator;
        Caching.BlockCache provider;
        JournalLog journalLog;
        object syncRoot = new object();

        // The read-only service.
        IReadService readService;

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="NonSafeJournal"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public SafeJournal()
        {
        }

        #endregion

        #region IJournal Members

        public bool Startup(Caching.BlockCache provider, JournalRecovery recovery)
        {
            this.provider = provider;
            this.allocator = new Allocator(provider);
            this.readService = new ReadService(provider);
            this.journalLog = new JournalLog(allocator);

            // We issue recovery and ignore operations (certainly not written by us).
            return JournalLog.StartupRecovery(this);    
        }

        public void Execute(IOperation operation)
        {
            // 1) We prepare operation.
            OperationStartupData data;
            operation.Prepare(ReadService, out data);

            // 2) We prepare service and allocate.
            SafeService service = new SafeService(provider,
                new AllocationContext(allocator.Allocate(operation,
                data.HintBlock, AllocationStrategy.Normal, data.BlockAllocationsRequired), operation,
                allocator, data.NeedsDynamicAllocation, provider));

            // 3) We persist stages.

            try
            {

                // 4) We execute all stages.
                for (uint i = 0; i < operation.StageCount; i++)
                {
                    operation.Execute(i, service);
                    service.Commit(allocator, journalLog);
                }

            }
            catch (Exception ex)
            {
                // Rollback is automatic.
                throw new DatabaseException(
                    "NonSafe journal has failed, this can result in data curruption. Check inner exception why " +
                    "it has failed.", ex);
            }

        }

        public IReadService ReadService
        {
            get { return readService; }
        }

        public string GetPhysicalLocation(ulong address)
        {
            return provider.GetPhysicalLocation(address);
        }

        public ulong FreeSpace
        {
            get { return allocator.FreeSpace; }
        }

        public ulong DeviceStorage
        {
            get { return allocator.BlockCount * (ulong)ReadService.BlockSize; }
        }

        #endregion
    }
}
