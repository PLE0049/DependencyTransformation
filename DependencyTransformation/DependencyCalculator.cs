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
        int size;

        int[][] AdjacencyMatrix;
        double[][] DependencyMatrix;

        public int[][] getAdjacencyMatrix()
        {
            return AdjacencyMatrix;
        }

        public double[][] getDependencyMatrix()
        {
            return DependencyMatrix;
        }

        public void LoadData(string path)
        {
            string[] lines = File.ReadAllLines(path);

            size = 0;
            // Look for max value
            foreach (var line in lines)
            {
                string[] attributes = line.Split(';');

                int x = Int32.Parse(attributes[0]);
                int y = Int32.Parse(attributes[1]);
                size = size < x ? x : size;
                size = size < y ? y : size;
            }
            size++;
            AlocateMatrices(size);

            // Create Adjancy Matrix
            foreach (var line in lines)
            {
                string[] attributes = line.Split(';');
                int x = Int32.Parse(attributes[0]);
                int y = Int32.Parse(attributes[1]);
                int w = Int32.Parse(attributes[2]);
                AdjacencyMatrix[x][y] = w;
                AdjacencyMatrix[y][x] = w;
            }
        }

        private void AlocateMatrices(int size)
        {
            // Alocate space for 
            AdjacencyMatrix = new int[size][];
            DependencyMatrix = new double[size][];
            for (int i = 0; i < size; i++)
            {
                AdjacencyMatrix[i] = new int[size];
                DependencyMatrix[i] = new double[size];
            }
        }

        public void Dependency(Object _elem)
        {
            Coord coords = (Coord)_elem;

            if(areNeighbours(coords.x, coords.y))
            {
                DependencyMatrix[coords.x][coords.y] = Dependency(coords.x, coords.y);
            }
            else
            {
                DependencyMatrix[coords.x][coords.y] = 0;
            }
        }

        bool areNeighbours(int x, int y )
        {
            return AdjacencyMatrix[x][y] > 0;
        }

        public double Dependency(int x, int y)
        {
            double divident = AdjacencyMatrix[x][y] + getCommonNeighboutsWeightsTimesCoef(x,y);
            double divisor = getNeighboursWeightSum(x);

            return divident/divisor;
        }

        private List<int> CommonNeighbours(int x, int y)
        {
            List<int> CommonNeighbours = new List<int>();

            for(int i = 0; i < size; i++)
            {
                if(AdjacencyMatrix[x][i] > 0 && AdjacencyMatrix[y][i] > 0)
                {
                    CommonNeighbours.Add(i);
                }
            }

            return CommonNeighbours;
        }

        private double getCommonNeighboutsWeightsTimesCoef(int x, int y)
        {
            double WeightsTimesCoef = 0;
            List<int> commonNeighbours = CommonNeighbours(x, y);

            foreach(int neighbour in commonNeighbours)
            {
                WeightsTimesCoef += (getCommonNeighboursWeight(x, neighbour) * getNeighboursCoeficient(x, neighbour, y));
            }

            return WeightsTimesCoef;
        }

        private double getCommonNeighboursWeight(int x, int neighbour)
        {
            return AdjacencyMatrix[x][neighbour];
        }

        private double getNeighboursCoeficient(int x, int neighbour, int y)
        {
            double divident = AdjacencyMatrix[neighbour][y];
            double divisor = AdjacencyMatrix[x][neighbour] + AdjacencyMatrix[neighbour][y];

            return divident/divisor;
        }
        private double getNeighboursWeightSum(int node)
        {
            int[] a = AdjacencyMatrix[node];
            int i, sum = 0;
            for (i = 0; i < a.Length; i++)
            {
                sum = sum + a[i];
            }
            return (sum);
        }

        public void ParallelThreadTransformation()
        {
            Thread[,] th = new Thread[size, size];
            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    Coord elem = new Coord(i,j);
                    th[i, j] = new Thread(new ParameterizedThreadStart(this.Dependency));
                    th[i, j].Start(elem);
                }
            }

            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    th[i, j].Join();
                }
            }
        }

        public void ParralelMatrixTransformation(double[,] a, double[,] result)
        {
            /*int s = a.GetLength(0);
            
            ParallelProcessor.For(0, s, delegate (int i)
            {
                for (int j = 0; j < s; j++)
                {
                    for (int k = 0; k < s; k++)
                    {
                        result[i, j] = Dependency(i, j);
                    }
                }
            });*/
        }
    }
}
