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
using SharpMedia.Threading.Performance;
using System.Threading;
using SharpMedia.Components.TextConsole;
using SharpMedia.Components.Configuration;

namespace SharpMedia.Threading.Default
{
    /// <summary>
    /// Represents a physical CPU working unit
    /// </summary>
    internal class CPUWorkUnit : ICPUWorkUnit, IComponentPostInitialization, IDisposable
    {
        // number of the CPU core in charge of
        int      cpuCoreNumber      = 0;

        // slice number of that core
        int      coreSliceNumber    = 0;

        // total slices per (this) core
        int      slicesPerCore      = 0;

        // amount of time spent busy (last second)
        TimeSpan busyTimeLastSecond = TimeSpan.Zero;

        // projected busy time for the next second
        TimeSpan projectedBusyTime  = TimeSpan.Zero;

        // performance of the FPU sub-unit
        FPUPerformance         fpuPerformance          = null;

        // performance of the ALU sub-unit
        InstructionPerformance instructionsPerformance = null;

        // work unit thread
        Thread workUnitThread = null;

        // should the work unit thread exit?
        bool workUnitThreadAbort = false;

        // queue of delegates to execute
        Queue<ReturnsVoid> delegatesToExecute = new Queue<ReturnsVoid>();

        // maximum depth of the queue
        int maxDepth = 0;

        /// <summary>
        /// Executes work on the CPU WU
        /// </summary>
        protected void WorkerMethod()
        {
            while (!workUnitThreadAbort)
            {
                Thread.BeginThreadAffinity();
                while (delegatesToExecute.Count > 0)
                {
                    delegatesToExecute.Dequeue()();
                }
                Thread.EndThreadAffinity();

                workUnitThread.Suspend();
            }
        }

        private ITextConsole console;
        [Required]
	    public ITextConsole Console
	    {
		    get { return console;}
		    set { console = value;}
	    }

        internal CPUWorkUnit(int coreNumber, int sliceNumber, CPUWorkUnit previous, int slicesPerCore)
        {
            this.workUnitThread  = new Thread(WorkerMethod);
            this.cpuCoreNumber   = coreNumber;
            this.coreSliceNumber = sliceNumber;
            this.slicesPerCore   = slicesPerCore;
        }

        #region ICPUWorkUnit Members

        public FPUPerformance FPU
        {
            get { return fpuPerformance; }
        }

        public InstructionPerformance Instructions
        {
            get { return instructionsPerformance; }
        }

        public uint ThreadCount
        {
            get { return 1; }
        }

        public uint MinThreadCount
        {
            get
            {
                return 1;
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public uint MaxThreadCount
        {
            get
            {
                return 1;
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public void Execute(ReturnsVoid del)
        {
            
            delegatesToExecute.Enqueue(del);
            if (delegatesToExecute.Count > maxDepth) maxDepth = delegatesToExecute.Count;

            if (workUnitThread.ThreadState == ThreadState.Suspended)
            {
                workUnitThread.Resume();
            }
            else if(workUnitThread.ThreadState != ThreadState.Running)
            {
                workUnitThread = new Thread(WorkerMethod);
                workUnitThread.Start();
            }
        }

        #endregion

        #region IWorkUnit Members

        public IMemoryUnit PrimaryMemoryUnit
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IMemoryUnit[] AccessibleMemoryUnits
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public float Usage
        {
            get { return delegatesToExecute.Count * 1.0f; }
        }

        #endregion

        #region IHardwareUnit Members

        public string ComputerName
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Guid HardwareId
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void MeasurePerformance(TimeSpan maxMeasurmentTime)
        {
            /* measure performance */
            SharpMedia.Math.Matrix.Matrix4x4f mtxa = SharpMedia.Math.Matrix.Matrix4x4f.Identity;
            SharpMedia.Math.Matrix.Matrix4x4f mtxb = SharpMedia.Math.Matrix.Matrix4x4f.Identity;
            SharpMedia.Math.Matrix.Matrix4x4f mtxc = SharpMedia.Math.Matrix.Matrix4x4f.Identity;
            int count = 0;

            Console.Out.WriteLine("ThreadControl: Measuring performance, core {0}.{1}", this.cpuCoreNumber, this.coreSliceNumber);

            DateTime dtTarget = DateTime.Now + TimeSpan.FromSeconds(0.25f);

            while(dtTarget > DateTime.Now)
            {
                mtxc = mtxa * mtxb;
                ++count;
            }

            /* FIXME: add other performance meters */

            // * 4 is because we're using 250ms but are targetting a 1000ms figure
            count = (count * 4) / slicesPerCore;

            fpuPerformance = new FPUPerformance(count, count, count, count);
        }

        public IPerformance[] PerformanceMeasurments
        {
            get { return new IPerformance[] { fpuPerformance, instructionsPerformance }; }
        }

        public CommunicationPerformance Communication
        {
            get { return null; }
        }

        public Type Type
        {
            get { return typeof(ICPUWorkUnit); }
        }

        #endregion

        #region IComponentPostInitialization Members

        public void PostComponentInit(IComponentDirectory directory)
        {
            MeasurePerformance(TimeSpan.FromSeconds(1));
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (workUnitThread != null)
            {
                workUnitThreadAbort = true;
                if (workUnitThread.ThreadState == ThreadState.Suspended)
                {
                    workUnitThread.Resume();
                }

                workUnitThread.Join();
            }
        }

        #endregion

        public override string ToString()
        {
            return base.ToString() + " MaxDepth :" + maxDepth;
        }
    }
}
