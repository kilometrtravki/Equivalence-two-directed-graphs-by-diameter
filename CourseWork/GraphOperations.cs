using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace CourseWork
{
    public static class GraphOperations
    {
        /// <summary>
        /// Set new graph's MFI arrays
        /// </summary>
        /// <param name="ArrG"></param>
        /// <param name="ArrP"></param>
        /// <param name="ArrW"></param>
        /// <param name="GD"></param>
        /// <returns></returns>
        public static bool SetGraphData(string ArrG, string ArrP, string ArrW, GraphData GD)
        {
            try
            {
                string[] tmp = ArrG.Split(' ');
                List<int> list = new List<int>();
                foreach (string str in tmp) list.Add(int.Parse(str));

                string[] tmp2 = ArrP.Split(' ');
                List<int> list2 = new List<int>();
                foreach (string str in tmp2) list2.Add(int.Parse(str));

                string[] tmp3 = ArrW.Split(' ');
                List<int> list3 = new List<int>();
                foreach (string str in tmp3) list3.Add(int.Parse(str));

                GD.G = new int[list.Count];
                GD.P = new int[list2.Count];
                GD.Weight = new int[list3.Count];

                for (int i = 0; i < list.Count; i++) GD.G[i] = list[i];
                for (int i = 0; i < list2.Count; i++) GD.P[i] = list2[i];
                for (int i = 0; i < list3.Count; i++) GD.Weight[i] = list3[i];

                return true;
            }
            catch (Exception ex) { return false; }
        }
        /// <summary>
        /// Read graph's arrays from file
        /// </summary>
        /// <param name="arrayG"></param>
        /// <param name="arrayP"></param>
        /// <param name="arrayW"></param>
        /// <returns></returns>
        public static bool ReadFile(out string arrayG, out string arrayP, out string arrayW)
        {
            arrayG = null; arrayP = null; arrayW = null;
            List<string> lines = new List<string>();
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Title = "Open FIle";
            openfile.InitialDirectory = @"C:\\";
            openfile.Filter = "txt files (*.txt)|*.txt";
            try
            {
                if (openfile.ShowDialog() == DialogResult.OK)
                {
                    using (StreamReader sr = new StreamReader(openfile.FileName))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            lines.Add(line);
                        }
                    }
                }
                else
                {
                    arrayG = null; arrayP = null; arrayW = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                arrayG = null; arrayP = null; arrayW = null;
                return false;
            }
            arrayG = lines[0];
            arrayP = lines[1];
            arrayW = lines[2];
            return true;
        }
        /// <summary>
        /// Draw graph's Vertices
        /// </summary>
        /// <param name="pic"></param>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <param name="point"></param>
        /// <param name="pointGet"></param>
        public static void DrawVertex(PictureBox pic, string name, Size size, Point point, ref Point pointGet)
        {
            Font font = new Font("Arial", 11);
            Graphics g = pic.CreateGraphics();
            Pen pen = new Pen(Color.Red);
            SolidBrush redBrush = new SolidBrush(Color.White);
            SolidBrush dotBrush = new SolidBrush(Color.Black);
            pointGet = new Point(point.X + 22, point.Y + 21);
            Rectangle rect = new Rectangle(point.X + 14, point.Y + 14, 30, 30);
            float startAngle = 0.0F;
            float sweepAngle = 360.0F;
            g.FillPie(redBrush, rect, startAngle, sweepAngle);
            g.DrawArc(pen, rect, startAngle, sweepAngle);
            g.DrawString(name, font, dotBrush, pointGet);
        }
        /// <summary>
        /// Find Vertex coordinates and return List<Point>
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static List<PointF> FindVertexCoords(ref GraphData graph)
        {
            float count = graph.P.Length;
            List<PointF> pointsList = new List<PointF>();
            float angle = 0.0f;
            Pen pen = new Pen(Brushes.Black, 3.0f);
            float anglepart = 360 / count;
            Rectangle area = new Rectangle(30, 30, 350, 350);
            for (int i = 0; i < count; i++)
            {
                float x = (float)(170 * Math.Cos(angle * Math.PI / 180F) + 200);
                float y = (float)(170 * Math.Sin(angle * Math.PI / 180F) + 170);
                angle += anglepart;

                pointsList.Add(new PointF(x, y));
            }
            return pointsList;
        }
        /// <summary>
        /// Draw graph's Edges
        /// </summary>
        /// <param name="pic"></param>
        /// <param name="VertexPoints"></param>
        /// <param name="grd"></param>
        public static void DrawVector(PictureBox pic, ref List<PointF> VertexPoints, ref GraphData grd)
        {
            Graphics g = pic.CreateGraphics();
            Pen pen = new Pen(Brushes.Black, 2.0f);
            List<List<int>> myList = GetListsOfVerticesInMfi(ref grd);
            for (int i = 0, k = 0; i < myList.Count; i++)
            {
                foreach (var a in myList[i])
                {
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                    Point startPoint = new Point((int)VertexPoints[a - 1].X + 30, (int)VertexPoints[a - 1].Y + 30);
                    Point endPoint = new Point((int)VertexPoints[i].X + 30, (int)VertexPoints[i].Y + 30);
                    g.DrawLine(pen, endPoint, startPoint);

                    if (endPoint == startPoint)
                    {
                        Rectangle rect = new Rectangle(startPoint.X, startPoint.Y, 40, 40);
                        float startAngle = 0.0F;
                        float sweepAngle = 360.0F;
                        g.DrawArc(pen, rect, startAngle, sweepAngle);
                    }

                    //PointF center = new PointF((startPoint.X + endPoint.X)/2, (startPoint.Y + endPoint.Y) / 2);

                    g.DrawString(grd.Weight[k].ToString(), new Font("Arial", 11), new SolidBrush(Color.Red), new PointF((startPoint.X + endPoint.X) / 2, (startPoint.Y + endPoint.Y) / 2));
                    k++;
                    double ugol = Math.Atan2(startPoint.X - endPoint.X, startPoint.Y - endPoint.Y);
                    Point newENDPoint = new Point(Convert.ToInt32(endPoint.X + 15 * Math.Sin(ugol)), Convert.ToInt32(endPoint.Y + 15 * Math.Cos(ugol)));

                    g.DrawLine(pen, newENDPoint.X, newENDPoint.Y, Convert.ToInt32(newENDPoint.X + 15 * Math.Sin(0.3 + ugol)), Convert.ToInt32(newENDPoint.Y + 15 * Math.Cos(0.3 + ugol)));
                    g.DrawLine(pen, newENDPoint.X, newENDPoint.Y, Convert.ToInt32(newENDPoint.X + 15 * Math.Sin(ugol - 0.3)), Convert.ToInt32(newENDPoint.Y + 15 * Math.Cos(ugol - 0.3)));

                }
            }

        }
        /// <summary>
        /// Draw graph's diametr
        /// </summary>
        /// <param name="pic"></param>
        /// <param name="VertexPoints"></param>
        /// <param name="grd"></param>
        /// <param name="path"></param>
        public static void DrawVectorPath(PictureBox pic, ref List<PointF> VertexPoints, ref GraphData grd, List<int> path)
        {
            Graphics g = pic.CreateGraphics();
            Pen pen = new Pen(Brushes.GreenYellow, 2.0f);
            List<List<int>> myList = GetListsOfVerticesInMfi(ref grd);
            int li = 0;
            for(int i = 0; i < path.Count-1; i++)
            {
                int start = path[i];
                int end = path[i+1];
                Point startPoint = new Point((int)VertexPoints[start].X + 30, (int)VertexPoints[start].Y + 30);
                Point endPoint = new Point((int)VertexPoints[end].X + 30, (int)VertexPoints[end].Y + 30);
                g.DrawLine(pen, endPoint, startPoint);
                if (endPoint == startPoint)
                {
                    Rectangle rect = new Rectangle(startPoint.X, startPoint.Y, 40, 40);
                    float startAngle = 0.0F;
                    float sweepAngle = 360.0F;
                    g.DrawArc(pen, rect, startAngle, sweepAngle);
                }

                double ugol = Math.Atan2(startPoint.X - endPoint.X, startPoint.Y - endPoint.Y);
                Point newENDPoint = new Point(Convert.ToInt32(endPoint.X + 15 * Math.Sin(ugol)), Convert.ToInt32(endPoint.Y + 15 * Math.Cos(ugol)));

                g.DrawLine(pen, newENDPoint.X, newENDPoint.Y, Convert.ToInt32(newENDPoint.X + 15 * Math.Sin(0.3 + ugol)), Convert.ToInt32(newENDPoint.Y + 15 * Math.Cos(0.3 + ugol)));
                g.DrawLine(pen, newENDPoint.X, newENDPoint.Y, Convert.ToInt32(newENDPoint.X + 15 * Math.Sin(ugol - 0.3)), Convert.ToInt32(newENDPoint.Y + 15 * Math.Cos(ugol - 0.3)));

            }
        }
        /// <summary>
        /// Get incoming Edges to each vertex 
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static List<List<int>> GetListsOfVerticesInMfi(ref GraphData graph) // mfi in list of lists
        {
            List<List<int>> myList = new List<List<int>>();
            List<int> a = new List<int>();
            int lastvalue = 0;
            for (int i = 0; i < graph.P.Length; i++)
            {
                a.Clear();
                a = new List<int>();
                for (int j = 0 + lastvalue; j < graph.P[i]; j++)
                {
                    a.Add(graph.G[j]);
                }
                myList.Add(new List<int>(a));
                if (graph.P[i] != 0)
                {
                    lastvalue = graph.P[i];
                }
            }

            return myList;
        }
        /// <summary>
        /// Get Adjacency Matrix
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static int[,] SMatrix(ref GraphData graph)
        {
            int size = graph.P.Length;
            int[,] SMatrixx = new int[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    SMatrixx[i, j] = 0;
            List<List<int>> myList = GetListsOfVerticesInMfi(ref graph);
            for (int i = 0; i < size; i++)
            {
                foreach (var list in myList[i])
                {
                    SMatrixx[list - 1, i] = 1;
                }
            }
            return SMatrixx;
        }
        /// <summary>
        /// Get Weight Matrix
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static int[,] WMatrix(ref GraphData graph)
        {
            int size = graph.P.Length;
            int[,] WMatrixx = new int[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    WMatrixx[i, j] = -1;
            int[,] a = SMatrix(ref graph);
            int k = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (a[j, i] == 1)
                    {
                        WMatrixx[j, i] = graph.Weight[k];
                        k++;
                    }
                }
            }
            return WMatrixx;
        }
        /// <summary>
        /// Delete Selected Vertex
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="Vertices"></param>
        /// <returns></returns>
        public static GraphData DeleteVertex(ref GraphData graph, List<int> Vertices)
        {
            GraphData newGraph = new GraphData();
            newGraph = graph;
            Vertices = Vertices.OrderByDescending(x => x).ToList();
            foreach (var Vertex in Vertices)
            {
                int[,] newMatrix = SMatrix(ref graph);
                int[,] newWMatrix = WMatrix(ref graph);
                List<int> NewG = new List<int>();
                List<int> NewP = new List<int>();
                List<int> NewW = new List<int>();
                for (int i = 0; i < graph.P.Length; i++)
                {
                    for (int j = 0; j < graph.P.Length; j++)
                    {
                        if (i == Vertex || j == Vertex) continue;
                        if (newMatrix[j, i] == 1)
                        {
                            if (j < Vertex)
                                NewG.Add(j + 1);
                            else
                                NewG.Add(j);

                            NewW.Add(newWMatrix[j, i]);
                        }
                    }
                    if (i == Vertex) continue;
                    NewP.Add(NewG.Count);
                }
                newGraph.P = NewP.ToArray();
                newGraph.G = NewG.ToArray();
                newGraph.Weight = NewW.ToArray();
            }
            return newGraph;
        }
        /// <summary>
        /// Find leaves in graph
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static List<int> FindLeaves(ref GraphData graph)
        {
            int[,] arr = SMatrix(ref graph);
            List<int> Leaves = new List<int>();
            for (int i = 0; i < graph.P.Length; i++)
            {
                for (int j = 0; j < graph.P.Length; j++)
                {
                    if (arr[i, j] == 1) break;
                    if (j == graph.P.Length - 1) Leaves.Add(i);
                }
            }
            for (int i = 0,r = 0; i < graph.P.Length; i++)
            {
                r = 0;
                for (int j = 0; j < graph.P.Length; j++)
                {
                    if (arr[i, j] == 1) r++;
                }
                if (r == 1)
                {
                    if (Leaves.IndexOf(i) == -1) Leaves.Add(i);
                }
            }
            return Leaves;
        }
        /// <summary>
        /// Find graph's diametr using Floyd-Warshall algoritm
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static int DiametrWithFloydWarshall(ref GraphData graph,out List<int> path)
        {
            int[,] distance = WMatrix(ref graph);
            int[,] Next = WMatrix(ref graph);

            int NOT_CONNECTED = -1; 

            for (int i = 0; i < graph.P.Length; i++)
            {
                for (int j = 0; j < graph.P.Length; j++)
                {
                    // No edge between node
                    // i and j
                    if (distance[i, j] != -1)
                        Next[i, j] = j;
                    else Next[i, j] = -1;
                }
            }

            for (int k = 0; k < graph.P.Length; ++k)
            {
                for (int i = 0; i < graph.P.Length; ++i)
                {
                    if (distance[i, k] != NOT_CONNECTED)
                    {

                        for (int j = 0; j < graph.P.Length; ++j)
                        {
                            if (distance[k, j] != NOT_CONNECTED && (distance[i, j] == NOT_CONNECTED || distance[i, k] + distance[k, j] < distance[i, j]))
                            {
                                distance[i, j] = distance[i, k] + distance[k, j];
                                Next[i, j] = Next[i, k];

                            }

                        }

                    }

                }
            }

            int diameter = -1;

            int st = -1, end = -1;

            for (int i = 0; i < graph.P.Length; ++i)
            {
                for (int j = 0; j < graph.P.Length; ++j)
                {
                    if (diameter < distance[i, j])
                    {
                        diameter = distance[i, j];
                        st = i;end = j;                    }
                }
            }

            // construct path
            path = new List<int>();
            path.Add(st);

            while (st != end)
            {
                st = Next[st, end];
                path.Add(st);
            }

            return diameter;
        }
        /// <summary>
        /// Checking a graph for connectivity
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static bool Connectivity(ref GraphData graph)
        {
            int[,] graphM = SMatrix(ref graph);
            int size = graph.P.Length;
            int[] VertexState = new int[size];
            bool red = false;
            int k = 0;
            int finalCount = 0;

            for (int i = 0; i < size; i++)
                VertexState[i] = 1; // все черные

            VertexState[0] = 2; // красная

            do
            {
                red = true;


                for (int i = 0; i < size; i++)
                {
                    if (VertexState[i] == 2)
                    {
                        VertexState[i] = 3;
                        k = i;
                        break;
                    }
                }

                for (int i = 0; i < size; i++)
                {

                    if (graphM[k,i] == 1 && VertexState[i] == 1)
                    {
                        VertexState[i] = 2;
                    }
                }

                for (int i = 0; i < size; i++)
                {
                    if (VertexState[i] == 2)
                        red = false;
                }

            } while (red == false);


            for (int i = 0; i < size; i++)
                if (VertexState[i] == 1)
                    finalCount++;

            if (finalCount == 0)
                return true;
            else return false;
            // Если finalCount = 0, то граф связный
        }
    }
  

}
