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
using System.Collections.ObjectModel;

namespace SharpMedia.Threading
{
    /// <summary>
    /// A work unit is responsible for executing work.
    /// </summary>
    /// <remarks>Each work unit is capable of executing work.</remarks>
    public interface IWorkUnit : IHardwareUnit
    {
        /// <summary>
        /// Memory unit.
        /// </summary>
        IMemoryUnit   PrimaryMemoryUnit { get; }

        /// <summary>
        /// Accessible memory units.
        /// </summary>
        IMemoryUnit[] AccessibleMemoryUnits { get; }

        /// <summary>
        /// The usage of work unit, in range [0,1].
        /// </summary>
        float Usage { get; }
    }

    /// <summary>
    /// Work Unit registry
    /// </summary>
    public interface IWorkUnitRegistry 
    {
        /// <summary>
        /// Registers a work unit
        /// </summary>
        /// <param name="workUnit">Work Unit</param>
        void Register(IWorkUnit workUnit);

        /// <summary>
        /// Obtain default work unit
        /// </summary>
        /// <param name="wuType">Work Unit Type</param>
        /// <returns>Work unit</returns>
        IWorkUnit Default(Type wuType);

        /// <summary>
        /// Obtain work unit that is most free
        /// </summary>
        /// <param name="wuType">Work Unit Type</param>
        /// <returns>Work unit</returns>
        IWorkUnit FreeMost(Type wuType);

        /// <summary>
        /// Obtain all work units of a specified type
        /// </summary>
        /// <typeparam name="T1">Work Unit interface type</typeparam>
        /// <returns>All work units of a specified type</returns>
        ReadOnlyCollection<T1> All<T1>() where T1 : IWorkUnit;
    }
}
