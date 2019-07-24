 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyTransformation
{
    class Program
    {
        private static string TestGraphPath = @"C:\Work\VSB\DependencyTransformation\DependencyTransformation\TestSample\sample.csv";

        static void Main(string[] args)
        {
            DependencyCalculator dc = new DependencyCalculator();
            dc.LoadData("edges karate.csv");
            dc.ParallelThreadTransformation();
            var result = dc.getDependencyMatrix();
        }  
    }
}
 