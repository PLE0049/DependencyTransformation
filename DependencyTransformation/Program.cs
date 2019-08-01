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
        private static string GraphNewmanPath = "Data/Newman.csv";
        private static string GraphUSAirportsPath = "Data/USairport.csv";
        private static string GraphAstroPhPath = "Data/astro-ph.csv";
        private static string GraphAsPath = "Data/as-22july06.csv";
        private static string GraphDecagonPath = "Data/PP-Decagon_ppi.csv";

        static void Main(string[] args)
        {
            DependencyCalculator dc;
            double[][] result;
            Stopwatch sw;
            string GraphPath = GraphDecagonPath;
            long[] time = new long[5];

            for(int i=0; i<1; i++)
            {
                time[i] = 0;
            }

            Console.WriteLine("Start of experiment");
            for (int i = 0; i < 5; i++ )
            {
                Console.WriteLine("Start of the round {0}",i);
                sw = new Stopwatch();
                dc = new DependencyCalculator();
                sw.Start();
                dc.LoadData(GraphPath);
                dc.SequentionalTransformation();
                result = dc.getDependencyMatrix();
                sw.Stop();
                time[0] += sw.ElapsedTicks;
                sw.Reset();
                dc = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();


                dc = new DependencyCalculator();
                sw.Start();
                dc.LoadData(GraphPath);
                dc.ParralelTaskTransformation();
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
                dc.ParralelOwnForTransformation();
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
                dc.ParallelNativeForTransformation();
                result = dc.getDependencyMatrix();
                sw.Stop();
                time[3] += sw.ElapsedTicks;
                sw.Reset();
                dc = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();


                /* dc = new DependencyCalculator();
                 sw.Start();
                 dc.LoadData(GraphPath);
                 dc.ParallelNaiveThreadTransformation();
                 result = dc.getDependencyMatrix();
                 sw.Stop();
                 time[4] += sw.ElapsedTicks;
                 sw.Reset();*/
            }

            Console.WriteLine("Method: Sequential; Ticks: {0}", time[0]/5);
            Console.WriteLine("Method: Tasks; Ticks: {0}", time[1]/5);
            Console.WriteLine("Method: Parallel Manager; Ticks: {0}", time[2]/5);
            Console.WriteLine("Method: Native Parallel For; Ticks: {0}", time[3]/5);
            //Console.WriteLine("Method: Naive Parallel; Ticks: {0} ms", time[4]/5);
        }  
    }
}
 