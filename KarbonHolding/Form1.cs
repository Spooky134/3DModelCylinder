using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KarbonHolding
{
    public partial class OhhShitProduction : Form
    {
        private MyFigure _vafle;
        public OhhShitProduction() { InitializeComponent(); }
        private void Form1_Load(object sender, EventArgs e) {}
        private void Button_Click(object sender, EventArgs e)
        {
            DataUpdate();
            var button = (ButtonBase)sender;
            switch (button.Text)
            {
                case "Render":
                    _vafle = new MyFigure(Data.R, Data.N, Data.A, Data.B,Data.C);
                    _vafle.Init();
                    _vafle.RenderFigure(picture);
                    break;
                case "Rerender":
                    _vafle.RenderFigure(picture);
                    break;
                case "Rotate":
                    _vafle.Rotate(Data.Alpha, Data.Beta, Data.Gama);
                    _vafle.RenderFigure(picture);
                    break;
                case "Move":
                    _vafle.Move(Data.Dx, Data.Dy, Data.Dz);
                    _vafle.RenderFigure(picture);
                    break;
                case "Scale":
                    _vafle.Scale(Data.Sx, Data.Sy, Data.Sz);
                    _vafle.RenderFigure(picture);
                    break;
                case "Lines":
                    Data.FDraw = 1;
                    break;
                case "Polygon":
                    Data.FDraw = 2;
                    break;
                case "Usual":
                    Data.FProj = 1;
                    break;
                case "Horizontal":
                    Data.FProj = 2;
                    break;
                case "Profile":
                    Data.FProj = 3;
                    break;
                case "Axonometric":
                    Data.FProj = 4;
                    break;
                case "Oblique":
                    Data.FProj = 5;
                    break;
                case "Perspective":
                    Data.FProj = 6;
                    break;
            };
        }
        private void DataUpdate()
        {
            Data.R = double.Parse(Radius.Text);
            Data.N = int.Parse(N.Text);
            Data.A = double.Parse(A.Text);
            Data.B = int.Parse(B.Text);
            Data.C = int.Parse(C.Text);
            Data.Alpha = double.Parse(Alpha.Text);
            Data.Beta = double.Parse(Beta.Text);
            Data.Gama = double.Parse(Gama.Text);
            Data.Dx = double.Parse(Dx.Text);
            Data.Dy = double.Parse(Dy.Text);
            Data.Dz = double.Parse(Dz.Text);
            Data.Sx = double.Parse(Sx.Text);
            Data.Sy = double.Parse(Sy.Text);
            Data.Sz = double.Parse(Sz.Text);
            Data.Psi = double.Parse(psi.Text);
            Data.Fi = double.Parse(Fi.Text);
            Data.L = double.Parse(L.Text);
            Data.Alph = double.Parse(Alph.Text);
            Data.D = double.Parse(D.Text);
            Data.Teta = double.Parse(Teta.Text);
            Data.F = double.Parse(F.Text);
            Data.Ro = double.Parse(Ro.Text);
        }
    }
}
