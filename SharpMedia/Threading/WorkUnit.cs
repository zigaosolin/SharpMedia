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
using System.Reflection;
using System.Collections.ObjectModel;
using SharpMedia.Math;

namespace SharpMedia.Threading
{
    /// <summary>
    /// Information about a work unit (CPU Core, GPU, ...)
    /// </summary>
    [Serializable]
    public abstract class WorkUnit
    {
        private WorkUnitPerformance performance;
        private int                 index;
        private double              availability;

        /// <summary>
        /// Performance of this working unit
        /// </summary>
        public virtual WorkUnitPerformance Performance
        {
            get { return performance; }
            set { performance = value; }
        }

        /// <summary>
        /// Index of the working unit of the same kind
        /// </summary>
        /// <example>
        /// CPU Core#
        /// </example>
        public virtual int Index
        {
            get { return index; }
        }

        /// <summary>
        /// Availability of the work unit, generally as a percentage
        /// </summary>
        public virtual double Availability
        {
            get { return availability; }
            set { availability = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="performance"></param>
        /// <param name="index"></param>
        protected WorkUnit(WorkUnitPerformance performance, int index)
        {
            this.performance = performance;
            this.index       = index;
        }

        /// <summary>
        /// Locks the working unit for exclusive work
        /// </summary>
        public abstract IDisposable Block { get; }
    }


    /// <summary>
    /// Allocation to work units
    /// </summary>
    [Serializable]
    public abstract class WorkUnitAllocation
    {
        /// <summary>
        /// Set the allocation to request an exclusive lock of a working unit
        /// </summary>
        public abstract void AddExclusiveLock(WorkUnit unit);

        /// <summary>
        /// Remove a request of an exclusive lock of a working unit
        /// </summary>
        public abstract void RemoveExclusiveLock(WorkUnit unit);

        /// <summary>
        /// Request a type of a working unit
        /// </summary>
        /// <param name="workUnitType">Type of the working unit</param>
        /// <param name="minimum">Minimum counts of such working units requested</param>
        /// <param name="maximum">Maximum counts of such working units requested</param>
        public abstract void Request(Type workUnitType, int minimum, int maximum);

        public abstract ReadOnlyCollection<WorkUnit> ExlusiveLocks { get; }
        public abstract Vector2i Requires(Type workUnit);
    }

    /// <summary>
    /// Performance of a working unit
    /// </summary>
    [Serializable]
    public class WorkUnitPerformance
    {
        Dictionary<string, PropertyInfo> name2Property = new Dictionary<string, PropertyInfo>();

        /// <summary>
        /// Shorthand for extending the Performance counters
        /// </summary>
        /// <param name="index">Name of the performance counter</param>
        /// <returns></returns>
        public TimeSpan this[string index] 
        {
            get 
            {
                if (name2Property.ContainsKey(index))
                    return (TimeSpan) name2Property[index].GetValue(this, null);

                return TimeSpan.MaxValue;
            }
            set
            {
                if (name2Property.ContainsKey(index))
                    name2Property[index].SetValue(this, value, null);
            }
        }

        protected WorkUnitPerformance()
        {
            foreach (PropertyInfo p in GetType().GetProperties())
            {
                if (p.PropertyType == typeof(TimeSpan))
                {
                    name2Property[p.Name] = p;
                }
            }
        }
    }
}
