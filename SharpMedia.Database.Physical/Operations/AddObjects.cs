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
using SharpMedia.Database.Physical.Journalling;
using System.Diagnostics;

namespace SharpMedia.Database.Physical.Operations
{
    /// <summary>
    /// Add objects as an atomic operation.
    /// </summary>
    internal class AddObjects : IOperation
    {
        #region Private Members
        AddObject[] subOperations;
        #endregion

        #region Constructors

        public AddObjects(ulong treeAddress, uint index, object[] data)
        {
            subOperations = new AddObject[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                subOperations[i] = new AddObject(treeAddress, (uint)(index + i), data[i] as byte[]);
            }
        }

        #endregion

        #region IOperation Members

        public void Prepare(SharpMedia.Database.Physical.Journalling.IReadService readService, 
            out OperationStartupData data)
        {
            // We extract all data.
            OperationStartupData[] subData = new OperationStartupData[subOperations.Length];
            for (int i = 0; i < subOperations.Length; i++)
            {
                subOperations[i].Prepare(readService, out subData[i]);
            }

            // We merge data.
            data = OperationStartupData.Merge(subData);
        }

        public uint StageCount
        {
            get { return 1; }
        }

        public void Execute(uint stage, SharpMedia.Database.Physical.Journalling.IService service)
        {
            Debug.Assert(stage == 0);

            foreach (AddObject op in subOperations)
            {
                op.Execute(stage, service);
            }
        }

        #endregion
    }
}
