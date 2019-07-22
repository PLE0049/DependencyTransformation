 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyTransformation
{
    class Program
    {
        static void Main(string[] args)
        {
            DependencyCalculator dc = new DependencyCalculator();
            dc.LoadData("edges lesmis.csv");
            var result = dc.Dependency(1, 0);

        }  
    }
}
 