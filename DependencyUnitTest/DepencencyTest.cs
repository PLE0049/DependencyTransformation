using System;
using DependencyTransformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyUnitTest
{
    [TestClass]
    public class DepencencyTest
    {
        [TestMethod]
        public void TestDependencyResult()
        {
            DependencyCalculator dc = new DependencyCalculator();
            dc.LoadData("edges karate.csv");
            Assert.AreEqual(0.420084025208403, dc.Dependency(33, 34));
        }

        [TestMethod]
        public void TestFileLoad()
        {
            DependencyCalculator dc = new DependencyCalculator();
            dc.LoadData("edges karate.csv");
            Assert.AreEqual(0, dc.Dependency(0, 0));
        }
    }
}
