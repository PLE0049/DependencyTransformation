using DependencyTransformation;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace DependencyUnitTest
{
    public class DepencencyTest
    {
        private const string SampleTestGraphPath = "SampleData/sample.csv";
        private const string KarateTestGraphPath = "SampleData/karate.csv";
        private const string LesmisTestGraphPath = "SampleData/lesmis.csv";
        private const string KarateDepTestGraphPath = "SampleData/karate_dep.csv";
        private const string LesmisDepTestGraphPath = "SampleData/lesmis_dep.csv";
        private const int PercisionDecimalPlaces = 5;

        private readonly DependencyCalculator _calculator;

        public DepencencyTest()
        {
            _calculator = new DependencyCalculator();
            _calculator.LoadData(SampleTestGraphPath);
        }

        [Fact]
        public void TestDependencyResult()
        {
            
            Assert.Equal(1, _calculator.Dependency(1, 4), PercisionDecimalPlaces);
            Assert.Equal(0.4, _calculator.Dependency(7, 4), PercisionDecimalPlaces);
            Assert.Equal(0.75, _calculator.Dependency(8,7), PercisionDecimalPlaces);
        }

        [Fact]
        public void TestDependencyMatrixResult()
        {
            _calculator.ParallelNaiveThreadTransformation();
            var depencencyMatrix = _calculator.getDependencyMatrix();
            Assert.Equal(1, depencencyMatrix[1][4], PercisionDecimalPlaces);
            Assert.Equal(0.4, depencencyMatrix[7][4], PercisionDecimalPlaces);
            Assert.Equal(0.75, depencencyMatrix[8][7], PercisionDecimalPlaces);
        }

        [Fact]
        public void TestFileLoad()
        {
            int[][] matrix = _calculator.getAdjacencyMatrix();
            Assert.Equal(1, matrix[4][5]);
            Assert.Equal(1, matrix[6][5]);
            Assert.Equal(1, matrix[5][7]);
            Assert.Equal(0, matrix[3][6]);
        }

        [Fact]
        public void TestKarateResult()
        {
            TestParrallelDependencyOnDataset(LesmisTestGraphPath, LesmisDepTestGraphPath);
        }

        [Fact]
        public void TestLesmisResult()
        {
            TestParrallelDependencyOnDataset(KarateTestGraphPath, KarateDepTestGraphPath);
        }

        private void TestParrallelDependencyOnDataset(string SourcePath, string ResultPath)
        {
            DependencyCalculator cal = new DependencyCalculator();
            cal.LoadData(SourcePath);

            double[][] result = LoadResultMatrix(ResultPath);

            List<Action> Methods = new List<Action>();
            Methods.Add(cal.ParallelNativeForTransformation);
            Methods.Add(cal.ParralelTaskTransformation);
            Methods.Add(cal.ParralelOwnForTransformation);

            int size = result.Length;

            foreach (Action parallelMethod in Methods)
            {
                parallelMethod();
                var depencencyMatrix = cal.getDependencyMatrix();
                for (int i = 0; i < size; ++i)
                {
                    for (int j = 0; j < size; ++j)
                    {
                        Assert.Equal(result[i][j], depencencyMatrix[i][j], PercisionDecimalPlaces);
                    }
                }
            }
        }


        private double[][] LoadResultMatrix(string path)
        {
            string[] lines = File.ReadAllLines(path);
            double[][] DependencyMatrix;
            int size = 0;
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

            DependencyMatrix = new double[size][];
            for (int i = 0; i < size; i++)
            {
                DependencyMatrix[i] = new double[size];
            }

            foreach (var line in lines)
            {
                string[] attributes = line.Split(';');
                int x = Int32.Parse(attributes[0]);
                int y = Int32.Parse(attributes[1]);
                double w = Double.Parse(attributes[2].Replace(',', '.'));
                DependencyMatrix[x][y] = w;
            }

            return DependencyMatrix;
        }
    }
}
