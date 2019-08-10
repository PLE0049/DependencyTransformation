 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DependencyTransformation
{
    class Program
    {
        private static string GraphKaratePath = "Data/Karate.csv";
        private static string GraphLesmisPath = "Data/lesmis.csv";

        static void Main(string[] args)
        {
            DependencyCalculator dc;
            double[][] result;
            Stopwatch sw;
            string GraphPath = GraphKaratePath;
            int TotalNumberOfCpus = Environment.ProcessorCount;
            const int ExperimentsCount = 4;
            int CpuLimit = TotalNumberOfCpus / 4;

            for (int j = 0; j < ExperimentsCount; j++)
            {
                long[] time = new long[5];

                for (int i = 0; i < 1; i++)
                {
                    time[i] = 0;
                }

                int CurrentLogicalCPUsInUse = CpuLimit + j;

                Console.WriteLine("Start of experiment");
                Console.WriteLine("Start of the round with limit of {0} logical cpus", CurrentLogicalCPUsInUse);
                for (int i = 0; i < 1; i++)
                {
                    sw = new Stopwatch();
                    dc = new DependencyCalculator();
                    sw.Start();
                    dc.LoadData(GraphPath);
                    dc.ParralelTaskTransformation(CurrentLogicalCPUsInUse);
                    result = dc.getDependencyMatrix();
                    sw.Stop();
                    time[1] += sw.ElapsedTicks;
                    sw.Reset();
                    dc = null;

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    dc = new DependencyCalculator();
                    sw.Start();
                    dc.LoadData(GraphPath);
                    dc.ParralelOwnForTransformation(CurrentLogicalCPUsInUse);
                    result = dc.getDependencyMatrix();
                    sw.Stop();
                    time[2] += sw.ElapsedTicks;
                    sw.Reset();
                    dc = null;

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    dc = new DependencyCalculator();
                    sw.Start();
                    dc.LoadData(GraphPath);
                    dc.ParallelNativeForTransformation(CurrentLogicalCPUsInUse);
                    result = dc.getDependencyMatrix();
                    sw.Stop();
                    time[3] += sw.ElapsedTicks;
                    sw.Reset();
                    dc = null;


                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    Console.WriteLine("Method: Tasks; Ticks: {0}", time[1] / 1);
                    Console.WriteLine("Method: Parallel Manager; Ticks: {0}", time[2] / 1);
                    Console.WriteLine("Method: Native Parallel For; Ticks: {0}", time[3] / 1);

                }
            }
        }  
    }
}
 