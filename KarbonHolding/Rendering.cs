using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;



namespace KarbonHolding
{
    public class Rendering
    {
        private static readonly Pen Pen = new Pen(Color.Blue, 2);
        private static Graphics _graph;

        //отрисовка линиями
        public void Render(Points point1, Points point2)
        {
            _graph.DrawLine(Pen, (float)point1.X, (float)point1.Y, (float)point2.X, (float)point2.Y);
        }
        //полигоны
        public void RenderPolygon(Points point1, Points point2, Points point3,Points point4,Color color)
        {
            var points1 = new Point((int)point1.X, (int)point1.Y);
            var points2 = new Point((int)point2.X, (int)point2.Y);
            var points3 = new Point((int)point3.X, (int)point3.Y);
            var points4 = new Point((int)point4.X, (int)point4.Y);
            Point[] curvePoints = { points1, points2, points3,points4 };

            _graph.FillPolygon(new SolidBrush(color), curvePoints);
        }
        public void DrawPolygon(Points point1, Points point2, Points point3, Points point4, Color color)
        {
            var points1 = new Point((int)point1.X, (int)point1.Y);
            var points2 = new Point((int)point2.X, (int)point2.Y);
            var points3 = new Point((int)point3.X, (int)point3.Y);
            var points4 = new Point((int)point4.X, (int)point4.Y);
            Point[] curvePoints = { points1, points2, points3, points4 };

            _graph.DrawPolygon(new Pen(color,1), curvePoints);
        }
        public void RenderHead(Points[] points, Color color)
        {
            Point[] buf = new Point[points.GetLength(0)];
            for (var i = 0; i < points.GetLength(0); i++)
            {
                buf[i] = new Point((int)points[i].X, (int)points[i].Y);
            }
            
            _graph.FillPolygon(new SolidBrush(color), buf);
        }
        

        //грапх
        public void Gener(Control pictureBox)
        {
            _graph = pictureBox.CreateGraphics();
            _graph.SmoothingMode = SmoothingMode.AntiAlias;
            _graph.Clear(Color.White);

            _graph.TranslateTransform(pictureBox.Width / 2, pictureBox.Height / 2+50);
            _graph.ScaleTransform(1, -1);
        }
    }
}


