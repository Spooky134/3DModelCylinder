using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace KarbonHolding
{
    class MyFigure
    {
        private Rendering ren = new Rendering();

        private Points[,] _Cilinder;
        private Points[,] _bufferCilinder;
        private Points[,] _Piped=new Points[4,2];
        private Points[,] _bufferPiped = new Points[4,2];
        private List<Points[]> polygonList = new List<Points[]>();
        private List<Color> colorList = new List<Color>();
        Points[] a;
        Points[] b;

        private readonly Points _viewpoint = new Points(0, 0, 10000);
        Points _lightpoint = new Points(0, 0, 1000);
        private double _ia = 127;
        private const double Ka = 1;
        private double _il = 127;
        private double _kd = 1;

        public double Radius { get; set; }
        public int Aprox { get; set; }
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }

        public MyFigure(double radius, int aproximation, double a, double b, double c)
        {
            Radius = radius;
            Aprox = aproximation;
            A = a;
            B = b;
            C = c;
        }

        public void Init()
        {
            InitPiped();
            InitCilinder();
        }
        private void InitPiped()
        {
            _Piped[0, 0] = new Points(C / 2, 0, -A / 2);
            _Piped[1, 0] = new Points(C / 2, 0, A / 2);
            _Piped[2, 0] = new Points(-C / 2, 0, A / 2);
            _Piped[3, 0] = new Points(-C / 2, 0, -A / 2);

            //верхнее основание 
            _Piped[0, 1] = new Points(C / 2, B, -A / 2);
            _Piped[1, 1] = new Points(C / 2, B, A / 2);
            _Piped[2, 1] = new Points(-C / 2, B, A / 2);
            _Piped[3, 1] = new Points(-C / 2, B, -A / 2);
        }
        private void InitCilinder()
        {
            _Cilinder = new Points[Aprox, 2];
            _bufferCilinder = new Points[Aprox, 2];

            for (var i = 0; i < Aprox; i++)
            {
                _Cilinder[i, 0] = Expansion(Radius, i * 360 / Aprox, 0);
                _Cilinder[i, 1] = Expansion(Radius, i * 360 / Aprox, B);
            }
        }


        private static Points Expansion(double r, double deg, double height)
        {
            var point = new Points
            {
                X = r * Math.Cos(deg * Math.PI / 180),
                Y = height,
                Z = r * Math.Sin(deg * Math.PI / 180)
            };

            return point;
        }

        //преобразования
        public void Rotate(double alpha, double beta, double gama)
        {
            for (var i = 0; i < Aprox; i++)
            {
                _Cilinder[i, 0] = _Cilinder[i, 0].Rotate(alpha, beta, gama);
                _Cilinder[i, 1] = _Cilinder[i, 1].Rotate(alpha, beta, gama);
            }
            for (var i = 0; i < 4; i++)
            {
                _Piped[i, 0] = _Piped[i, 0].Rotate(alpha, beta, gama);
                _Piped[i, 1] = _Piped[i, 1].Rotate(alpha, beta, gama);
            }
        }
        public void Scale(double sx, double sy, double sz) 
        {
            for (var i = 0; i < Aprox; i++)
            {
                _Cilinder[i, 0] = _Cilinder[i, 0].Scale(sx, sy, sz);
                _Cilinder[i, 1] = _Cilinder[i, 1].Scale(sx, sy, sz);
            }
            for (var i = 0; i < 4; i++)
            {
                _Piped[i, 0] = _Piped[i, 0].Scale(sx, sy, sz);
                _Piped[i, 1] = _Piped[i, 1].Scale(sx, sy, sz);
            }
        }
        public void Move(double dx, double dy, double dz)
        {
            for (var i = 0; i < Aprox; i++)
            {
                _Cilinder[i, 0] = _Cilinder[i, 0].Move(dx, dy, dz);
                _Cilinder[i, 1] = _Cilinder[i, 1].Move(dx, dy, dz);
            }
            for (var i = 0; i < 4; i++)
            {
                _Piped[i, 0] = _Piped[i, 0].Move(dx, dy, dz);
                _Piped[i, 1] = _Piped[i, 1].Move(dx, dy, dz);
            }
        }

        //отрисовка
        public void RenderFigure(PictureBox picture)
        {
            Projection();
            switch (Data.FDraw)
            {
                case 1:
                    Render(picture);
                    break;
                case 2:
                    RenderPoligon(picture);
                    break;
            };
        }
        public void Render(PictureBox picture)
        {
            ren.Gener(picture);
            RenderCilinder();
            RenderPiped();
        }

        public void RenderCilinder()
        {
            ren.Render(_bufferCilinder[Aprox - 1, 0], _bufferCilinder[0, 0]);
            ren.Render(_bufferCilinder[Aprox - 1, 1], _bufferCilinder[0, 1]);
            for (var i = 1; i < Aprox; i++)
            {
                ren.Render(_bufferCilinder[i, 0], _bufferCilinder[i - 1, 0]);
                ren.Render(_bufferCilinder[i, 1], _bufferCilinder[i - 1, 1]);
            }

            for (var i = 0; i < Aprox; i++)
            {
                ren.Render(_bufferCilinder[i, 0], _bufferCilinder[i, 1]);
            }
        }
        public void RenderPiped()
        {
            ren.Render(_bufferPiped[3, 0], _bufferPiped[0, 0]);
            ren.Render(_bufferPiped[3, 1], _bufferPiped[0, 1]);
            for (var i = 1; i < 4; i++)
            {
                ren.Render(_bufferPiped[i - 1, 0], _bufferPiped[i, 0]);
                ren.Render(_bufferPiped[i - 1, 1], _bufferPiped[i, 1]);
            }
            for (var i = 0; i < 4; i++)
            {
                ren.Render(_bufferPiped[i, 0], _bufferPiped[i, 1]);
            }
        }
        public void RenderPoligon(PictureBox picture)
        {
            ren.Gener(picture);
            for (var i=0; i<polygonList.Count; i++)
            {
                if (polygonList[i].Length == 4)
                {
                    ren.RenderPolygon(polygonList[i][0], polygonList[i][1], polygonList[i][2], polygonList[i][3],Color.CornflowerBlue);
                    ren.DrawPolygon(polygonList[i][0], polygonList[i][1], polygonList[i][2], polygonList[i][3],Color.Black);
                }
                else
                {
                    ren.RenderHead(polygonList[i],Color.CornflowerBlue);
                    ren.DrawPolygon(polygonList[i][0], polygonList[i][1], polygonList[i][2], polygonList[i][3], Color.Black);
                }
            }
           
        }

        //проекции
        public void Projection()
        {
            _lightpoint = new Points(0, 0, 1000);
            switch (Data.FProj)
            {
                case 1:
                    Array.Copy(_Piped, _bufferPiped, _Piped.Length);
                    Array.Copy(_Cilinder, _bufferCilinder, _Cilinder.Length);
                    CreatePolygon();
                    RobertsAlgorithm();
                    Painting();
                    break;
                case 2:
                    Horizontal();
                    break;
                case 3:
                    Profile();
                    break;
                case 4:
                    Axonometric(Data.Psi, Data.Fi);
                    break;
                case 5:
                    Oblique(Data.L, Data.A);
                    break;
                case 6:
                    Array.Copy(_Piped, _bufferPiped, _Piped.Length);
                    Array.Copy(_Cilinder, _bufferCilinder, _Cilinder.Length);
                    Perspective(Data.D, Data.Teta, Data.F, Data.Ro);
                    break;
            };
        }

        public void Profile()
        {
            for (var i = 0; i < Aprox; i++)
            {
                _bufferCilinder[i, 0] = _Cilinder[i, 0].ProfileProjection();
                _bufferCilinder[i, 1] = _Cilinder[i, 1].ProfileProjection();
            }
            for (var i = 0; i < 4; i++)
            {
                _bufferPiped[i, 0] = _Piped[i, 0].ProfileProjection();
                _bufferPiped[i, 1] = _Piped[i, 1].ProfileProjection();
            }
            _lightpoint = _lightpoint.ProfileProjection();
            CreatePolygon();
            Painting();
            RobertsAlgorithm();
          
        }
        public void Horizontal()
        {
            for (var i = 0; i < Aprox; i++)
            {
                _bufferCilinder[i, 0] = _Cilinder[i, 0].HorizontalProjection();
                _bufferCilinder[i, 1] = _Cilinder[i, 1].HorizontalProjection();
            }
            for (var i = 0; i < 4; i++)
            {
                _bufferPiped[i, 0] = _Piped[i, 0].HorizontalProjection();
                _bufferPiped[i, 1] = _Piped[i, 1].HorizontalProjection();
            }

            _lightpoint = _lightpoint.HorizontalProjection();
            CreatePolygon();
            Painting();
            RobertsAlgorithm();
          
        }
        public void Axonometric(double psi, double fi)
        {
            for (var i = 0; i < Aprox; i++)
            {
                _bufferCilinder[i, 0] = _Cilinder[i, 0].AxonometricProjection(psi,fi);
                _bufferCilinder[i, 1] = _Cilinder[i, 1].AxonometricProjection(psi,fi);
            }
            for (var i = 0; i < 4; i++)
            {
                _bufferPiped[i, 0] = _Piped[i, 0].AxonometricProjection(psi,fi);
                _bufferPiped[i, 1] = _Piped[i, 1].AxonometricProjection(psi,fi);
            }
            CreatePolygon();
            RobertsAlgorithm();
            Painting();
        }
        public void Oblique(double l, double alpha)
        {
            for (var i = 0; i < Aprox; i++)
            {
                _bufferCilinder[i, 0] = _Cilinder[i, 0].ObliqueProjection(l, alpha);
                _bufferCilinder[i, 1] = _Cilinder[i, 1].ObliqueProjection(l, alpha);
            }
            for (var i = 0; i < 4; i++)
            {
                _bufferPiped[i, 0] = _Piped[i, 0].ObliqueProjection(l, alpha);
                _bufferPiped[i, 1] = _Piped[i, 1].ObliqueProjection(l, alpha);
            }
            CreatePolygon();
            RobertsAlgorithm();
            Painting();
        }
        public void Perspective(double d, double teta, double fi, double ro)
        {



            //for (var i = 0; i < polygonList.Count; i++)
            //{
            //    polygonList[i][0] = polygonList[i][0].SpeciesTransformation(teta, fi, ro);
            //    polygonList[i][1] = polygonList[i][1].SpeciesTransformation(teta, fi, ro);
            //    polygonList[i][2] = polygonList[i][2].SpeciesTransformation(teta, fi, ro);
            //    polygonList[i][3] = polygonList[i][3].SpeciesTransformation(teta, fi, ro);
            //}








            //for (var i = 0; i < polygonList.Count; i++)
            //{
            //    polygonList[i][0] = polygonList[i][0].PerspectiveProjection(d);
            //    polygonList[i][1] = polygonList[i][1].PerspectiveProjection(d);
            //    polygonList[i][2] = polygonList[i][2].PerspectiveProjection(d);
            //    polygonList[i][3] = polygonList[i][3].PerspectiveProjection(d);
            //}

            Species(teta, fi, ro);
            for (var i = 0; i < Aprox; i++)
            {
                _bufferCilinder[i, 0] = _bufferCilinder[i, 0].PerspectiveProjection(d);
                _bufferCilinder[i, 1] = _bufferCilinder[i, 1].PerspectiveProjection(d);
            }
            for (var i = 0; i < 4; i++)
            {
                _bufferPiped[i, 0] = _bufferPiped[i, 0].PerspectiveProjection(d);
                _bufferPiped[i, 1] = _bufferPiped[i, 1].PerspectiveProjection(d);
            }
            CreatePolygon();
            RobertsAlgorithm();
            Painting();



        }
        public void Species(double teta, double fi, double ro)
        {
            for (var i = 0; i < Aprox; i++)
            {
                _bufferCilinder[i, 0] = _Cilinder[i, 0].SpeciesTransformation(teta, fi,ro);
                _bufferCilinder[i, 1] = _Cilinder[i, 1].SpeciesTransformation(teta, fi,ro);
            }
            for (var i = 0; i < 4; i++)
            {
                _bufferPiped[i, 0] = _Piped[i, 0].SpeciesTransformation(teta, fi,ro);
                _bufferPiped[i, 1] = _Piped[i, 1].SpeciesTransformation(teta, fi,ro);
            }
        }
        public void RobertsAlgorithm()
        {
            double cosView;
            for (var i = polygonList.Count - 1; i >= 0; i--)
            {
                cosView = Tools.Angle(polygonList[i][0], polygonList[i][1],polygonList[i][2],polygonList[i][3] ,_viewpoint);
                if (RadtoDeg(cosView) > 90 || RadtoDeg(cosView) < 0)
                {
                    polygonList.RemoveAt(i);
                    //colorList.RemoveAt(i);
                }
            }
        }
        public void Painting()
        {
            for (var i = 0; i < polygonList.Count; i++)
            {
                var cosLight = Math.Cos(Tools.Angle(polygonList[i][0], polygonList[i][1], polygonList[i][2],polygonList[i][3],_lightpoint));
                var intensity = ((float)(_ia * Ka + (_il * _kd * cosLight) / 255));
                colorList.Add( Light(Color.Aqua, intensity));
            }
        }

        public void CreatePolygon()
        {
            colorList.Clear();
            polygonList.Clear();
            //цилиндр
            polygonList.Add(new[] { _bufferCilinder[0, 1], _bufferCilinder[Aprox - 1, 1], _bufferCilinder[Aprox - 1, 0], _bufferCilinder[0, 0] });
            for (var i = 1; i < Aprox; i++)
            {
                polygonList.Add(new[]
                { _bufferCilinder[i, 1],_bufferCilinder[i - 1, 1],_bufferCilinder[i - 1, 0],_bufferCilinder[i, 0] });
            }

            //куб
            polygonList.Add(new[] { _bufferPiped[3, 1], _bufferPiped[0, 1], _bufferPiped[0, 0], _bufferPiped[3, 0], });
            for (var i = 1; i < 4; i++)
            {
                polygonList.Add(new[]
                {
                    _bufferPiped[i - 1, 1], _bufferPiped[i, 1],_bufferPiped[i, 0], _bufferPiped[i - 1, 0],
                });
            }
           //  polygonList.Add(new[] { _bufferPiped[1, 0], _bufferPiped[2, 0], _bufferPiped[3, 0], _bufferPiped[0, 0],   });
           // polygonList.Add(new[] { _bufferPiped[3, 1], _bufferPiped[2, 1], _bufferPiped[1, 1], _bufferPiped[0, 1] });


            List<Points> foo = new List<Points>();
            a = new Points[_bufferCilinder.GetLength(0) + 4 + 1 +1];

           
            ////////////////говно1
            for (var i = 0; i < 4; i++)
            {
                foo.Add(_bufferPiped[i, 0]);
            }
            foo.Add(_bufferPiped[0, 0]);
            for (var i = 0; i < _bufferCilinder.GetLength(0); i++)
            {
                foo.Add(_bufferCilinder[i, 0]);
                if (i == _bufferCilinder.GetLength(0) - 1)
                {
                    foo.Add(_bufferCilinder[0, 0]);
                }
            }
            for (int i = 0; i < foo.Count; i++)
            {
                a[i] = foo[i];
            }
            for (var i = 1; i < 4; i++)
            {
                polygonList.AddRange(new[] { a });
            }
           

            List<Points> foo2 = new List<Points>();
            b = new Points[_bufferCilinder.GetLength(0) + 4 + 1 + 1];
           
            ////////////////говно2
            for (var i = 3; i >=0; i--)
            {
                foo2.Add(_bufferPiped[i, 1]);
                
            }
            foo2.Add(_bufferPiped[3, 1]);

            for (var i = 0; i < _bufferCilinder.GetLength(0); i++)
            {
                foo2.Add(_bufferCilinder[i, 1]);
                if (i == _bufferCilinder.GetLength(0) - 1)
                {
                    foo2.Add(_bufferCilinder[0, 1]);
                }
            }
            for (int i = 0; i < foo2.Count; i++)
            {
                b[i] = foo2[i];
            }
            for (var i = 1; i < 4; i++)
            {
                polygonList.AddRange(new[] { b });
            }
           
        }



        private static Color Light(Color color, float factor)
        {
            var r = (byte)((color.R * factor));
            var g = (byte)((color.G * factor));
            var b = (byte)((color.B * factor));
            return Color.FromArgb(255, r, g, b);
        }
        public static double RadtoDeg(double grad)
        {
            return grad * 180 / Math.PI;
        }
    }
}
