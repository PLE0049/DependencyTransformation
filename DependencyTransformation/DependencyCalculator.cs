using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DependencyTransformation
{
    public class DependencyCalculator
    {
        long n, m, o;

        int[][] AdjacencyMatrix;
        double[][] DependencyMatrix;

        public void LoadData(string path)
        {
            string[] lines = File.ReadAllLines(path);

            foreach(var line in lines)
            {

            }
        }

        public void Dependency(Object _elem)
        {


        }

        public double Dependency(int x, int y)
        {
            return 0;
        }

        public void ParallelThreadMultiply()
        {
            Thread[,] th = new Thread[n, o];
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < o; ++j)
                {
                    Coord elem = new Coord(i,j);
                    th[i, j] = new Thread(new ParameterizedThreadStart(this.Dependency));
                    th[i, j].Start();
                }
            }

            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < o; ++j)
                {
                    th[i, j].Join();
                }
            }
        }

        public void ParralelMatrixTransformation(double[,] a, double[,] result)
        {
            int s = a.GetLength(0);

            ParallelProcessor.For(0, s, delegate (int i)
            {
                for (int j = 0; j < s; j++)
                {
                    for (int k = 0; k < s; k++)
                    {
                        result[i, j] = Dependency(i, j);
                    }
                }
            });
        }
    }
}
