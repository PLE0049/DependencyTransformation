using DependencyTransformation;
using Xunit;

namespace DependencyUnitTest
{
    public class DepencencyTest
    {
        private const string TestGraphPath = "SampleData/sample.csv";
        private const int PercisionDecimalPlaces = 5;

        private readonly DependencyCalculator _calculator;

        public DepencencyTest()
        {
            _calculator = new DependencyCalculator();
            _calculator.LoadData(TestGraphPath);
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
            _calculator.ParallelThreadTransformation();
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
    }
}
