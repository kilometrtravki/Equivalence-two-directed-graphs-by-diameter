using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourseWork
{
    public partial class MainMenu : Form
    {
        /// <summary>
        /// Graph's data (arrays G,P,W)
        /// </summary>
        GraphData graphData1 = new GraphData();
        /// <summary>
        /// Graph's 2 data (arrays G,P,W)
        /// </summary>
        GraphData graphData2 = new GraphData();
        /// <summary>
        /// graph vertex coordinates
        /// </summary>
        List<PointF> VCoords = new List<PointF>();
        /// <summary>
        /// graph vertex coordinates 2
        /// </summary>
        List<PointF> VCoords2 = new List<PointF>();

        public MainMenu()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Set graph data in first area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Create_Graph_Click(object sender, EventArgs e)
        {
            if (textBoxG.Text == "" || textBoxP.Text == "" || textBox_Weight1.Text == "")
                return;
            try
            {
                string t_G = textBoxG.Text;
                string t_P = textBoxP.Text;
                string t_W = textBox_Weight1.Text;
                GraphOperations.SetGraphData(t_G, t_P, t_W, graphData1);
                if (graphData1.P.Length > 20 || graphData1.G.Length > 50)
                {
                    MessageBox.Show("Vertices can be no more than 20.\nEdges can be no more than 50.", "Error");
                    pictureBox1 = null;
                    VCoords = null;
                    return;
                }
                VCoords = GraphOperations.FindVertexCoords(ref graphData1);
                Random random = new Random();
                Point a = new Point(50, 50);
                GraphOperations.DrawVector(pictureBox1, ref VCoords, ref graphData1);
                for (int i = 0; i < graphData1.P.Length; i++)
                {
                    GraphOperations.DrawVertex(pictureBox1, (i + 1).ToString(), pictureBox1.Size, new Point((int)VCoords[i].X, (int)VCoords[i].Y), ref a);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");

            }
        }
        /// <summary>
        /// Set graph data in second area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_CreateP2_Click(object sender, EventArgs e)
        {
            if (textBoxG2.Text == "" || textBoxP2.Text == "" || textBox_Weight2.Text == "")
                return;
            try
            {
                string t_G = textBoxG2.Text;
                string t_P = textBoxP2.Text;
                string t_W = textBox_Weight2.Text;
                GraphOperations.SetGraphData(t_G, t_P, t_W, graphData2);
                if (graphData2.P.Length > 20 || graphData2.G.Length > 50)
                {
                    MessageBox.Show("Vertices can be no more than 20.\nEdges can be no more than 50.", "Error");
                    pictureBox2 = null;
                    //graphData1 = null;
                    VCoords2 = null;
                    return;
                }
                VCoords2 = GraphOperations.FindVertexCoords(ref graphData2);
                Random random = new Random();
                Point a = new Point(50, 50);
                GraphOperations.DrawVector(pictureBox2, ref VCoords2, ref graphData2);
                for (int i = 0; i < graphData2.P.Length; i++)
                {
                    GraphOperations.DrawVertex(pictureBox2, (i + 1).ToString(), pictureBox2.Size, new Point((int)VCoords2[i].X, (int)VCoords2[i].Y), ref a);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");

            }

        }
        /// <summary>
        /// Clear First area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_clear_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
        }
        /// <summary>
        /// Read data for first graph from file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_ReadFirstGraph_Click(object sender, EventArgs e)
        {
            string sG, sP, sW;
            if (GraphOperations.ReadFile(out sG, out sP, out sW) == true)
            {
                textBoxP.Text = sP;
                textBoxG.Text = sG;
                textBox_Weight1.Text = sW;
            }
            else
            {
                MessageBox.Show("Incorrect input data", "Error");
            }
        }
        /// <summary>
        /// Read data for second graph from file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonReadSecondGraph_Click(object sender, EventArgs e)
        {
            string sG, sP, sW;
            if (GraphOperations.ReadFile(out sG, out sP, out sW) == true)
            {
                textBoxP2.Text = sP;
                textBoxG2.Text = sG;
                textBox_Weight2.Text = sW;
            }
            else
            {
                MessageBox.Show("Incorrect input data", "Error");
            }
        }
        /// <summary>
        /// Clear second area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_clear2_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = null;
        }
        /// <summary>
        /// Compare graphs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Compare_Click(object sender, EventArgs e)
        {
            if (graphData1.P == null || graphData2.P == null)
            {
                return;
            }
            // find leaves
            List<int> leaves1 = GraphOperations.FindLeaves(ref graphData1);
            List<int> leaves2 = GraphOperations.FindLeaves(ref graphData2);

            // delete leaves
            graphData1 = GraphOperations.DeleteVertex(ref graphData1, leaves1);
            graphData2 = GraphOperations.DeleteVertex(ref graphData2, leaves2);
            // find diametrs
            List<int> path1, path2;
            int d1 = GraphOperations.DiametrWithFloydWarshall(ref graphData1, out path1);
            int d2 = GraphOperations.DiametrWithFloydWarshall(ref graphData2,out path2);


            pictureBox1.Image = null;
            pictureBox1.Update();
            pictureBox2.Image = null;
            pictureBox2.Update();

            VCoords = GraphOperations.FindVertexCoords(ref graphData1);
            Random random = new Random();
            Point a = new Point(50, 50);
            GraphOperations.DrawVector(pictureBox1, ref VCoords, ref graphData1);
            GraphOperations.DrawVectorPath(pictureBox1, ref VCoords, ref graphData1,path1);
            for (int i = 0; i < graphData1.P.Length; i++)
            {
                GraphOperations.DrawVertex(pictureBox1, (i + 1).ToString(), pictureBox1.Size, new Point((int)VCoords[i].X, (int)VCoords[i].Y), ref a);
            }

            VCoords2 = GraphOperations.FindVertexCoords(ref graphData2);
            GraphOperations.DrawVector(pictureBox2, ref VCoords2, ref graphData2);
            GraphOperations.DrawVectorPath(pictureBox2, ref VCoords2, ref graphData2,path2);
            for (int i = 0; i < graphData2.P.Length; i++)
            {
                GraphOperations.DrawVertex(pictureBox2, (i + 1).ToString(), pictureBox2.Size, new Point((int)VCoords2[i].X, (int)VCoords2[i].Y), ref a);
            }

            bool sv_1 = GraphOperations.Connectivity(ref graphData1); // is graph 
            bool sv_2 = GraphOperations.Connectivity(ref graphData2); // is graph 
            string ds1 = d1.ToString(); string ds2 = d2.ToString();

            richTextBox1.Text = "";
            richTextBox1.Text += $"First graph:\n" +
                $"Количество вершин: {graphData1.P.Length}\n" +
                $"Количество дуг: {graphData1.G.Length}\n" +
                $"Количество удаленных висячих вершин: {leaves1.Count}\n";
            if (!sv_1) // если граф не связный 
                ds1 = "infinity (граф несвязный)";
            else ds1 = d1.ToString();
            richTextBox1.Text += $"Диаметр графа: {ds1}";

            richTextBox1.Text += $"\nSecond graph:\n" +
                $"Количество вершин: {graphData2.P.Length}\n" +
                $"Количество дуг: {graphData2.G.Length}\n" +
                $"Количество удаленных висячих вершин: {leaves2.Count}\n";
            if (!sv_2) // если граф не связный 
                ds2 = "infinity (граф несвязный)";
            else ds2 = d2.ToString();
            richTextBox1.Text += $"Диаметр графа: {ds2}";

            if (ds1 == ds2)
                richTextBox1.Text += "\nГрафы эквивалентны по диаметру";
            else
                richTextBox1.Text += "\nГрафы не эквивалентны по диаметру";

        }
    }
}