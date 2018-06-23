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
using SharpMedia.AspectOriented;
using System.Collections.ObjectModel;

namespace SharpMedia.Threading.Default
{
    /// <summary>
    /// A Registry of working units
    /// </summary>
    internal sealed class WorkUnitRegistry : IWorkUnitRegistry
    {
        Dictionary<Type, List<IWorkUnit>> workUnits = new Dictionary<Type, List<IWorkUnit>>();

        #region IWorkUnitRegistry Members

        public void Register([NotNull] IWorkUnit workUnit)
        {
            Type t = workUnit.Type;
            if (!workUnits.ContainsKey(t))
            {
                workUnits[t] = new List<IWorkUnit>();
            }

            workUnits[t].Add(workUnit);
        }

        public IWorkUnit Default([NotNull] Type wuType)
        {
            return workUnits.ContainsKey(wuType) ? workUnits[wuType][0] : null;
        }

        int lastRoundRobinIndex = 0;
        public IWorkUnit FreeMost([NotNull] Type wuType)
        {
            List<IWorkUnit> copy = new List<IWorkUnit>(workUnits[wuType]);
            copy.Sort(
                delegate(IWorkUnit left, IWorkUnit right) 
                {
                    float f = left.Usage - right.Usage;
                    return f == 0 ? 0 : (f > 0 ? 1 : -1); 
                });

            float usage = copy[0].Usage;

            copy.RemoveAll(delegate(IWorkUnit wu) { return wu.Usage != usage; });

            return copy[lastRoundRobinIndex++ % copy.Count];
        }

        public ReadOnlyCollection<T1> All<T1>() where T1 : IWorkUnit
        {
            if (workUnits.ContainsKey(typeof(T1)))
            {
                List<T1> results = new List<T1>();
                foreach (object obj in workUnits[typeof(T1)])
                {
                    results.Add((T1) obj);
                }

                return results.AsReadOnly();
            }

            return new ReadOnlyCollection<T1>(new List<T1>());
        }

        #endregion
    }
}