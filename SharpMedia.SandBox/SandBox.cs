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
using System.Threading;
using System.IO;
using SharpMedia.Math;
using SharpMedia.Database;
using SharpMedia.Database.Memory;
using SharpMedia.Threading.Default;
using SharpMedia.Threading;
using SharpMedia.Components.Configuration;
using SharpMedia.Components.Configuration.ComponentProviders;
using SharpMedia.Components.TextConsole;
using SharpMedia.Threading.Optimizers;
using SharpMedia.Math.Matrix;
using System.Collections.Generic;

namespace SharpMedia.SandBox
{
    public class SandBox
    {
        static readonly int count = 100 * 100 * 10;

        static void Main(string[] args)
        {
            IComponentDirectory icd = new ComponentDirectory();
            icd.Register(new ConfiguredComponent(typeof(StandardConsole).FullName, "Console"));

            using (ThreadControl tc = new ThreadControl())
            {
                tc.Components = icd;
                (tc as IComponentPostInitialization).PostComponentInit(icd);

                tc.Register(new LastResortOptimizer());

                Parallel parallel = new Parallel();
                parallel.ThreadControl = tc;
                long baseTime = DateTime.Now.Ticks;

                Int32 finished = 0;

                long[] tickTimes = new long[count];
                int[]  threadIds = new int[count];

                List<int> ilist = new List<int>();
                Random r = new Random();
                for (int i = 0; i < count; ++i)
                {
                    ilist.Add(r.Next());
                }

                Console.Out.WriteLine("a1:" + (DateTime.Now.Ticks - baseTime));

                parallel.ForEach(ilist,
                    delegate(int i) 
                    {
                        // Console.Out.WriteLine("# {0} (finished {1}, Thread {2})", i, finished, Thread.CurrentThread.ManagedThreadId);

                        finished++;
                    });

                Console.Out.WriteLine("a3:" + (DateTime.Now.Ticks - baseTime));

                while (finished < count)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(0.5f));
                }

                long startupTime = TimeSpan.FromMilliseconds(10).Ticks;
                /*
                for (int i = 0; i < 100; ++i)
                {
                    Console.Out.WriteLine("{0}: Thread: {2} {1} ticks", i, 
                        tickTimes[i] - baseTime - startupTime,
                        threadIds[i]);
                }
                 */

                foreach (IWorkUnit wu in tc.WorkUnits.All<ICPUWorkUnit>())
                {
                    Console.Out.WriteLine(wu);
                }
            }
        }
    }
}