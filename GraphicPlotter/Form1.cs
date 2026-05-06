using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GraphicPlotter
{
    public partial class Form1 : Form
    {
        private const int MarksCount = 10;
        public Form1()
        {
            InitializeComponent();
            SetupChart();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private double CalculateFunction(double x)
        {
            return Math.Pow(Math.Cos(10 * x), 2) + x;
        }
        private void SetupChart()
        {
            chart1.Series.Clear();
            var series = new Series
            {
                ChartType = SeriesChartType.Spline,
                Color = Color.Red,
                BorderWidth = 2
            };
            chart1.Series.Add(series);

            for (double x = -10; x <= 10; x += 0.1)
            {
                double y = CalculateFunction(x);
                chart1.Series[0].Points.AddXY(x, y);
            }

            chart1.ChartAreas[0].AxisX.Title = "X";
            chart1.ChartAreas[0].AxisY.Title = "Y";
            chart1.ChartAreas[0].AxisX.Minimum = -10;
            chart1.ChartAreas[0].AxisX.Maximum = 10;
            chart1.ChartAreas[0].AxisY.Minimum = -10;
            chart1.ChartAreas[0].AxisY.Maximum = 10;
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawManualGraph(e.Graphics);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            this.Invalidate(); 
        }
        private void DrawManualGraph(Graphics g)
        {
            g.Clear(Color.White);

            Pen axisPen = new Pen(Color.Black, 1);
            Pen graphPen = new Pen(Color.Blue, 2);
            Font font = new Font("Arial", 10);
            Brush brush = new SolidBrush(Color.Black);

            int width = this.ClientSize.Width - 250; 
            int height = this.ClientSize.Height;
            Point center = new Point(width / 2, height / 2);

            g.DrawLine(axisPen, 20, center.Y, width - 20, center.Y); 
            g.DrawLine(axisPen, center.X, 20, center.X, height - 20); 

            g.DrawString("X", font, brush, width - 30, center.Y - 20);
            g.DrawString("Y", font, brush, center.X + 10, 10);

            int marks = MarksCount;
            float xStep = (width - 40) / (2f * marks);
            float yStep = (height - 40) / (2f * marks);

            for (int i = 1; i <= marks; i++)
            {
                int xPos = center.X + (int)(i * xStep);
                g.DrawLine(axisPen, xPos, center.Y - 5, xPos, center.Y + 5);
                g.DrawString((i * 2).ToString(), font, brush, xPos - 10, center.Y + 10);

                xPos = center.X - (int)(i * xStep);
                g.DrawLine(axisPen, xPos, center.Y - 5, xPos, center.Y + 5);
                g.DrawString((-i * 2).ToString(), font, brush, xPos - 10, center.Y + 10);

                int yPos = center.Y - (int)(i * yStep);
                g.DrawLine(axisPen, center.X - 5, yPos, center.X + 5, yPos);
                g.DrawString((i * 2).ToString(), font, brush, center.X + 10, yPos - 10);

                yPos = center.Y + (int)(i * yStep);
                g.DrawLine(axisPen, center.X - 5, yPos, center.X + 5, yPos);
                g.DrawString((-i * 2).ToString(), font, brush, center.X + 10, yPos - 10);
            }
            PointF[] points = new PointF[1000];
            for (int i = 0; i < 1000; i++)
            {
                float x = -10 + i * 20f / 999;
                float y = (float)CalculateFunction(x);
                points[i] = new PointF(
                    center.X + x * xStep / 2,
                    center.Y - y * yStep / 2);
            }
            g.DrawCurve(graphPen, points);


        }
    }
}
