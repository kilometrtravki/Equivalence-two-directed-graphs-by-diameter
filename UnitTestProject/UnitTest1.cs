using Microsoft.VisualStudio.TestTools.UnitTesting;
using CourseWork;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        GraphData graph = new GraphData();
        [TestMethod]
        public void FindLeavesTest()
        {
            graph.P = new int[] { 3, 5, 8, 11, 12, 16, 19, 23 };
            graph.G = new int[] { 2, 6, 8, 1, 3, 2, 4, 8, 3, 6, 7, 6, 1, 4, 5, 7, 4, 6, 8, 1, 3, 6, 7 };
            graph.Weight = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            List<int> leaves_actual = GraphOperations.FindLeaves(ref graph);
            List<int> leaves_expected = new List<int>() { 4 };

            CollectionAssert.AreEqual(leaves_actual, leaves_expected);

        }
        [TestMethod]
        public void DeleteVertexTest()
        {
            graph.P = new int[] { 3, 5, 8, 11, 12, 16, 19, 23 };
            graph.G = new int[] { 2, 6, 8, 1, 3, 2, 4, 8, 3, 6, 7, 6, 1, 4, 5, 7, 4, 6, 8, 1, 3, 6, 7 };
            graph.Weight = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            GraphOperations.DeleteVertex(ref graph, new List<int>() { 4 });
            GraphData ExpectedGraph = new GraphData();
            ExpectedGraph.P = new int[] { 3, 5, 8, 11, 14, 17, 21 };
            ExpectedGraph.G = new int[] { 2, 5, 7, 1, 3, 2, 4, 7, 3, 5, 6, 1, 4, 6, 4, 5, 7, 1, 3, 5, 6 };
            ExpectedGraph.Weight = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            CollectionAssert.AreEqual(graph.G, ExpectedGraph.G);
            CollectionAssert.AreEqual(graph.P, ExpectedGraph.P);
            CollectionAssert.AreEqual(graph.Weight, ExpectedGraph.Weight);

        }
        [TestMethod]
        public void FindDiametrTest()
        {
            graph.P = new int[] { 3, 5, 8, 11, 12, 16, 19, 23 };
            graph.G = new int[] { 2, 6, 8, 1, 3, 2, 4, 8, 3, 6, 7, 6, 1, 4, 5, 7, 4, 6, 8, 1, 3, 6, 7 };
            graph.Weight = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            List<int> leaves = GraphOperations.FindLeaves(ref graph);
            GraphOperations.DeleteVertex(ref graph, leaves);
            List<int> path = new List<int>();
            int actual_diametr = GraphOperations.DiametrWithFloydWarshall(ref graph, out path);
            int expected_diametr = 3;
            Assert.AreEqual(actual_diametr, expected_diametr);
        }
        [TestMethod]
        public void ConnectivityOfGraphTest()
        {
            graph.P = new int[] { 3, 5, 8, 11, 12, 16, 19, 23 };
            graph.G = new int[] { 2, 6, 8, 1, 3, 2, 4, 8, 3, 6, 7, 6, 1, 4, 5, 7, 4, 6, 8, 1, 3, 6, 7 };
            graph.Weight = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            bool actual = GraphOperations.Connectivity(ref graph);
            Assert.AreEqual(actual, true);

            graph.P = new int[] { 3, 6, 8, 11, 14, 0, 17, 20, 22, 24 };
            graph.G = new int[] { 2, 2, 7, 1, 1, 9, 4, 5, 3, 5, 5, 3, 4, 4, 1, 8, 10, 7, 9, 10, 2, 8, 7, 8 };
            graph.Weight = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            actual = GraphOperations.Connectivity(ref graph);
            Assert.AreEqual(actual, false);
        }
        [TestMethod]
        public void GetConnectivityMatrixOfGraphTest()
        {
            graph.P = new int[] { 2, 4, 6, 8, 9 };
            graph.G = new int[] { 4, 5, 1, 3, 2, 4, 1, 5, 2 };
            graph.Weight = new int[] { -2, 5, 2, -5, 6, 1, 3, 10, 7 };

            int[,] actual = GraphOperations.SMatrix(ref graph);
            int[,] expected = new int[,] {
            {0, 1, 0, 1, 0 },
            {0, 0, 1, 0, 1 },
            {0, 1, 0, 0, 0 },
            {1, 0, 1, 0, 0 },
            {1, 0, 0, 1, 0 } };
            CollectionAssert.AreEqual(actual, expected);
        }
        [TestMethod]
        public void GraphEquivalenceTest()
        {
            graph.P = new int[] { 3, 5, 8, 11, 12, 16, 19, 23 };
            graph.G = new int[] { 2, 6, 8, 1, 3, 2, 4, 8, 3, 6, 7, 6, 1, 4, 5, 7, 4, 6, 8, 1, 3, 6, 7 };
            graph.Weight = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            GraphData graph2 = new GraphData();
            graph2.P = new int[] { 2, 7, 11, 14, 17, 18, 21, 24 };
            graph2.G = new int[] { 4, 8, 3, 4, 5, 7, 8, 2, 5, 6, 8, 1, 2, 7, 2, 3, 7, 3, 2, 4, 5, 1, 2, 3 };
            graph2.Weight = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            GraphData graph3 = new GraphData();
            graph3.P = new int[] { 1, 2, 5, 6, 7, 8, 9, 10, 13, 15 };
            graph3.G = new int[] { 2, 1, 2, 6, 10, 1, 1, 7, 10, 9, 1, 3, 6, 8, 9 };
            graph3.Weight = new int[] { 3, 4, 5, 6, 22, 2, 3, 4, 51, 2, 1, 3, 4, 2, 1 };

            List<int> leaves1 = GraphOperations.FindLeaves(ref graph);
            List<int> leaves2 = GraphOperations.FindLeaves(ref graph2);
            List<int> leaves3 = GraphOperations.FindLeaves(ref graph3);

            GraphOperations.DeleteVertex(ref graph, leaves1);
            GraphOperations.DeleteVertex(ref graph2, leaves2);
            GraphOperations.DeleteVertex(ref graph3, leaves3);

            List<int> path = new List<int>();
            int diametr1 = GraphOperations.DiametrWithFloydWarshall(ref graph, out path);
            int diametr2 = GraphOperations.DiametrWithFloydWarshall(ref graph2, out path);
            int diametr3 = GraphOperations.DiametrWithFloydWarshall(ref graph3, out path);

            bool result = diametr1 == diametr2;
            Assert.IsTrue(result);
            result = diametr1 == diametr3;
            Assert.IsFalse(result);
        }
    }
}
