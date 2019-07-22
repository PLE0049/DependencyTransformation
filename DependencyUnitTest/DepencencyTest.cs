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
            Assert.AreEqual(0, dc.Dependency(0, 0));
        }
    }
}
