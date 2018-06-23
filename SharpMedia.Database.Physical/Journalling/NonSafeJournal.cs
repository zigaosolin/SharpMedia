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
    /// A non-safe journal class. Operations are not atomic under this journal
    /// but this makes it faster than other journals. Use only if you can garantie
    /// atomicy.
    /// </summary>
    public class NonSafeJournal : IJournal
    {

        #region Private Members
        Allocator allocator;
        Caching.BlockCache provider;
        object syncRoot = new object();

        // The read-only service.
        IReadService readService;

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="NonSafeJournal"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public NonSafeJournal()
        {
        }

        #endregion

        #region IJournal Members

        public bool Startup(Caching.BlockCache provider, JournalRecovery recovery)
        {
            this.provider = provider;
            this.allocator = new Allocator(provider);
            this.readService = new ReadService(provider);

            // We issue recovery (not written by us but still valid).
           return JournalLog.StartupRecovery(this);
        }

        public void Execute(IOperation operation)
        {
            // 1) We prepare operation.
            OperationStartupData data;
            operation.Prepare(ReadService, out data);

            // 2) We prepare service and allocate.
            NonSafeService service = new NonSafeService(provider, 
                new AllocationContext(allocator.Allocate(operation,
                data.HintBlock, AllocationStrategy.Normal, data.BlockAllocationsRequired), operation, 
                allocator, data.NeedsDynamicAllocation, provider));

            try
            {

                // 3) We execute all stages.
                for (uint i = 0; i < operation.StageCount; i++)
                {
                    operation.Execute(i, service);
                }

            }
            catch (Exception ex)
            {
                throw new DatabaseException(
                    "NonSafe journal has failed, this can result in data curruption. Check inner exception why " +
                    "it has failed.", ex);
            }
            finally
            {
                // 4) We clear the service (to any state we came).
                service.Dispose(allocator);
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
