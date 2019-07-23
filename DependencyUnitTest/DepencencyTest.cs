using System;
using DependencyTransformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyUnitTest
{
    [TestClass]
    public class DepencencyTest
    {
        private string TestGraphPath = @"C:\Work\VSB\DependencyTransformation\DependencyTransformation\TestSample\sample.csv";

        [TestMethod]
        public void TestDependencyResult()
        {
            DependencyCalculator dc = new DependencyCalculator();
            dc.LoadData(TestGraphPath);
            Assert.AreEqual(1, dc.Dependency(1, 4));
            Assert.AreEqual(0.4, dc.Dependency(7, 4));
            Assert.AreEqual(0.75, dc.Dependency(8,7));
        }

        [TestMethod]
        public void TestFileLoad()
        {
            DependencyCalculator dc = new DependencyCalculator();
            dc.LoadData(TestGraphPath);
            int[][] matrix = dc.getAdjacencyMatrix();
            Assert.AreEqual(1, matrix[4][5]);
            Assert.AreEqual(1, matrix[6][5]);
            Assert.AreEqual(1, matrix[5][7]);
            Assert.AreEqual(0, matrix[3][6]);
        }
    }
}
