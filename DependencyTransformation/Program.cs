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
        static void Main(string[] args)
        {
            DependencyCalculator dc;
            double[][] result;
            Stopwatch sw;
            string GraphPath;
            int TotalNumberOfCpus = Environment.ProcessorCount;
            int CpuRanges;
            int ExperimentCount;
            


            Console.WriteLine("Enter graph file path: ");
            GraphPath = Console.ReadLine();

            Console.WriteLine("Total cpus: {0}", TotalNumberOfCpus);
            Console.WriteLine("Enter number of cpu ranges <1,10>:");
            CpuRanges = Int32.Parse(Console.ReadLine());
            int CpuLimit = TotalNumberOfCpus / CpuRanges;

            Console.WriteLine("Enter number of experiments:");
            ExperimentCount = Int32.Parse(Console.ReadLine());

            Console.WriteLine("");
            Console.WriteLine("Start of experiment");
            Console.WriteLine("");

            for (int j = 1; j <= CpuRanges; j++)
            {
                long[] time = new long[5];

                for (int i = 0; i < ExperimentCount; i++)
                {
                    time[i] = 0;
                }

                int CurrentLogicalCPUsInUse = CpuLimit*j;

                Console.WriteLine("Start of the round with limit of {0} logical cpus", CurrentLogicalCPUsInUse);

                for (int i = 0; i < ExperimentCount; i++)
                {                  
                    sw = new Stopwatch();
                    dc = new DependencyCalculator();                    
                    dc.LoadData(GraphPath);
                    sw.Start();
                    dc.ParralelTaskTransformation(CurrentLogicalCPUsInUse);
                    result = dc.getDependencyMatrix();
                    sw.Stop();
                    time[1] += sw.ElapsedTicks;
                    sw.Reset();
                    dc = null;

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    dc = new DependencyCalculator();                
                    dc.LoadData(GraphPath);
                    sw.Start();
                    dc.ParralelOwnForTransformation(CurrentLogicalCPUsInUse);
                    result = dc.getDependencyMatrix();
                    sw.Stop();
                    time[2] += sw.ElapsedTicks;
                    sw.Reset();
                    dc = null;

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    dc = new DependencyCalculator();                   
                    dc.LoadData(GraphPath);
                    sw.Start();
                    dc.ParallelNativeForTransformation(CurrentLogicalCPUsInUse);
                    result = dc.getDependencyMatrix();
                    sw.Stop();
                    time[3] += sw.ElapsedTicks;
                    sw.Reset();
                    dc = null;


                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }

                Console.WriteLine("Method: Tasks; Ticks: {0}", time[1] / ExperimentCount);
                Console.WriteLine("Method: Parallel Manager; Ticks: {0}", time[2] / ExperimentCount);
                Console.WriteLine("Method: Parallel For; Ticks: {0}", time[3] / ExperimentCount);
                Console.WriteLine("");
            }
            Console.ReadKey();
        }  
    }
}
 