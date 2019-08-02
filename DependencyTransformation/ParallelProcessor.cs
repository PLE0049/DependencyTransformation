using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DependencyTransformation
{
    public sealed class ParallelProcessor
    {

        public delegate void ForLoopBody(int index);

        // Optionaly we can use ProcessorCount
        private static int threadsCount = System.Environment.ProcessorCount*2;

        // Ensure thread safe singleton
        private static object sync = new Object();

        // !! Volatile !!  
        private static volatile ParallelProcessor instance = null;

        private Thread[] threads = null;

        private AutoResetEvent[] jobAvailable = null;
        private ManualResetEvent[] threadIdle = null;

        private int currentIndex;
        private int stopIndex;
        private ForLoopBody loopBody;

        public static int ThreadsCount
        {
            get { return threadsCount; }
            set
            {
                lock (sync)
                {
                    threadsCount = Math.Max(1, value);
                }
            }
        }

        public static void For(int start, int stop, ForLoopBody loopBody)
        {
            lock (sync)
            {
                ParallelProcessor instance = Instance;

                instance.currentIndex = start - 1;
                instance.stopIndex = stop;
                instance.loopBody = loopBody;

                for (int i = 0; i < threadsCount; i++)
                {
                    instance.threadIdle[i].Reset();
                    instance.jobAvailable[i].Set();
                }

                for (int i = 0; i < threadsCount; i++)
                {
                    instance.threadIdle[i].WaitOne();
                }
            }
        }

        private static ParallelProcessor Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ParallelProcessor();
                    instance.Initialize();
                }
                else
                {
                    if (instance.threads.Length != threadsCount)
                    {
                        instance.Terminate();
                        instance.Initialize();
                    }
                }
                return instance;
            }
        }

        private void Initialize()
        {
            jobAvailable = new AutoResetEvent[threadsCount];
            threadIdle = new ManualResetEvent[threadsCount];
            threads = new Thread[threadsCount];

            for (int i = 0; i < threadsCount; i++)
            {
                jobAvailable[i] = new AutoResetEvent(false);
                threadIdle[i] = new ManualResetEvent(true);

                threads[i] = new Thread(new ParameterizedThreadStart(WorkerThread));
                threads[i].IsBackground = true;
                threads[i].Start(i);
            }
        }

        private void Terminate()
        {
            loopBody = null;
            for (int i = 0, threadsCount = threads.Length; i < threadsCount; i++)
            {
                jobAvailable[i].Set();
                threads[i].Join();
                jobAvailable[i].Close();
                threadIdle[i].Close();
            }
            jobAvailable = null;
            threadIdle = null;
            threads = null;
        }

        private void WorkerThread(object index)
        {
            int threadIndex = (int)index;
            int localIndex = 0;

            while (true)
            {
                jobAvailable[threadIndex].WaitOne();

                if (loopBody == null)
                    break;

                while (true)
                {
                    localIndex = Interlocked.Increment(ref currentIndex);

                    if (localIndex >= stopIndex)
                        break;

                    loopBody(localIndex);
                }
                threadIdle[threadIndex].Set();
            }
        }
    }
}

