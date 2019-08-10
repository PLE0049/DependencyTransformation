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
        private int size;

        private double[][] AdjacencyMatrix;
        private double[][] DependencyMatrix;

        Dictionary<int, int> Nodes;

        public double[][] getAdjacencyMatrix()
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
            Nodes = new Dictionary<int, int>();

            size = 0;
            // Look for max value
            foreach (var line in lines)
            {
                string[] attributes = line.Split(';');

                int x = Int32.Parse(attributes[0]);
                CreateNode(x);
                int y = Int32.Parse(attributes[1]);
                CreateNode(y);
            }
            AlocateMatrices(size);

            // Create Adjancy Matrix
            foreach (var line in lines)
            {
                string[] attributes = line.Split(';');
                int x = Int32.Parse(attributes[0]);
                int y = Int32.Parse(attributes[1]);
                double w = Double.Parse(attributes[2].Replace(',', '.'));

                x = Nodes[x];
                y = Nodes[y];

                AdjacencyMatrix[x][y] = w;
                AdjacencyMatrix[y][x] = w;
            }
        }

        public void LoadDependencyMatrix(string path)
        {
            string[] lines = File.ReadAllLines(path);
            Nodes = new Dictionary<int, int>();

            size = 0;
            // Look for max value
            foreach (var line in lines)
            {
                string[] attributes = line.Split(';');

                int x = Int32.Parse(attributes[0]);
                CreateNode(x);
                int y = Int32.Parse(attributes[1]);
                CreateNode(y);
            }
            AlocateMatrices(size);

            // Create Adjancy Matrix
            foreach (var line in lines)
            {
                string[] attributes = line.Split(';');
                int x = Int32.Parse(attributes[0]);
                int y = Int32.Parse(attributes[1]);
                double w = Double.Parse(attributes[2].Replace(',', '.'));

                x = Nodes[x];
                y = Nodes[y];

                DependencyMatrix[x][y] = w;
            }
        }
        private void CreateNode(int node)
        {
            if (!Nodes.ContainsKey(node))
            {
                Nodes[node] = size;
                size++;
            }
        }

        private void AlocateMatrices(int size)
        {
            // Alocate space for 
            AdjacencyMatrix = new double[size][];
            DependencyMatrix = new double[size][];
            for (int i = 0; i < size; i++)
            {
                AdjacencyMatrix[i] = new double[size];
                DependencyMatrix[i] = new double[size];
            }
        }

        public void Dependency(Object _elem)
        {
            Coord coords = (Coord)_elem;
            DependencyMatrix[coords.x][coords.y] = Dependency(coords.x, coords.y);
        }

        bool areNeighbours(int x, int y )
        {
            return AdjacencyMatrix[x][y] > 0;
        }

        public double Dependency(int x, int y)
        {
            double divident = AdjacencyMatrix[x][y] + getCommonNeighboutsWeightsTimesCoef(x,y);
            double divisor = getNeighboursWeightSum(x);

            return divident / divisor;
        }

        public int GetRealId(int x)
        {
            return Nodes[x];
        }

        public double DependencyByRealID(int x, int y)
        {
            return Dependency(GetRealId(x), GetRealId(y));
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
            double[] a = AdjacencyMatrix[node];
            int i = 0;
            double sum = 0;
            for (i = 0; i < a.Length; i++)
            {
                sum = sum + a[i];
            }
            return (sum);
        }

        public void ParallelNaiveThreadTransformation()
        {
            Thread[,] th = new Thread[size, size];
            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    if (areNeighbours(i, j))
                    {
                        Coord elem = new Coord(i, j);
                        th[i, j] = new Thread(new ParameterizedThreadStart(this.Dependency));
                        th[i, j].Start(elem);
                    }
                }
            }

            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    if(th[i, j] != null)
                    {
                        th[i, j].Join();
                    }
                }
            }
        }

        public void ParallelNativeForTransformation(int threads = 0)
        {
            if (threads == 0)
            {
                threads = Environment.ProcessorCount;
            }

            Parallel.For(0, size, new ParallelOptions() { MaxDegreeOfParallelism = threads }, i => {
                for (int j = 0; j < size; ++j)
                {
                    if (areNeighbours(i, j))
                    {
                        Coord elem = new Coord(i, j);
                        this.Dependency(elem);
                    }
                }
            });
        }

        public void SequentionalTransformation()
        {
            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                { 
                    if (areNeighbours(i, j))
                    {
                        Coord elem = new Coord(i, j);
                        this.Dependency(elem);
                    }
                }
            }
        }


        public void ParralelOwnForTransformation(int threads = 0)
        {
            if(threads == 0)
            {
                threads = Environment.ProcessorCount;
            }

            ParallelProcessor.ThreadsCount = threads;
            ParallelProcessor.For(0, size, delegate (int i)
            {
                for (int j = 0; j < size; ++j)
                {
                    if (areNeighbours(i, 0))
                    {
                        Coord elem = new Coord(i, 0);
                        this.Dependency(elem);
                    }
                }
            });
        }
        
        public void ParralelTaskTransformation(int threads = 0)
        {
            if (threads == 0)
            {
                threads = Environment.ProcessorCount;
            }

            // Create a scheduler that uses two threads. 
            LimitedConcurrencyLevelTaskScheduler lcts = new LimitedConcurrencyLevelTaskScheduler(threads);
            List<Task> tasks = new List<Task>();

            // Create a TaskFactory and pass it our custom scheduler. 
            TaskFactory factory = new TaskFactory(lcts);

            for (var i = 0; i < size; i++)
            {
                for (int j = 0; j < size; ++j)
                {
                    if (areNeighbours(i, j))
                    {
                        Coord elem = new Coord(i, j);
                        Task t = factory.StartNew((parametr) =>
                        {
                            Coord ii = (Coord)parametr;
                            this.Dependency(elem);
                        },
                        elem);

                        tasks.Add(t);
                    }
                }                  
            }
            Task.WaitAll(tasks.ToArray());
        }
    }
}
