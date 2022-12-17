using static System.Math;

namespace KarbonHolding
{
    static class Tools
    {
        //Масштабирование 
        public static Points Scale(this Points point, double sx, double sy, double sz)
        {
            double[,] scal = { { sx, 0, 0, 0 },
                               { 0, sy, 0, 0 },
                               { 0, 0, sz, 0 },
                              { 0, 0, 0, 1 } };
            return Points.MultiplicationMatrix(point.p, scal);
        }
        //Сдвиг
        public static Points Move(this Points point, double dx, double dy, double dz)
        {
            double[,] mov = { { 1, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 1, 0 },
                { dx, dy, dz, 1 } };

            return Points.MultiplicationMatrix(point.p, mov);
        }
        //поворот
        public static Points Rotate(this Points point, double alpha, double beta, double gama)
        {
            alpha = DegtoRad(alpha);
            beta = DegtoRad(beta);
            gama = DegtoRad(gama);
            double[,] rot = { { Cos(beta) * Cos(gama),
                                Cos(beta) * Sin(gama),
                               -Sin(beta), 0 },

                              { Sin(alpha) * Sin(beta) * Cos(gama) - Cos(alpha) * Sin(gama),
                                Sin(alpha) * Sin(beta) * Sin(gama) + Cos(alpha) * Cos(gama),
                                Sin(alpha) * Cos(beta), 0 },

                              { Cos(alpha) * Sin(beta) * Cos(gama) + Sin(alpha) * Sin(gama),
                                Cos(alpha) * Sin(beta) * Sin(gama) - Sin(alpha)* Cos(gama),
                                Cos(alpha)* Cos(beta),0 },

                              { 0,0,0,1 } };

            return Points.MultiplicationMatrix(point.p, rot);
        }


        //профильная 
        public static Points ProfileProjection(this Points point)
        {
            return new Points(point.Z, point.Y, point.X);
        }
        //горизонтальная
        public static Points HorizontalProjection(this Points point)
        {
            return new Points(point.X, point.Z, point.Y);
        }
        //аксонометрическая
        public static Points AxonometricProjection(this Points point, double psi, double fi)
        {
            psi = DegtoRad(psi);
            fi = DegtoRad(fi);
            double[,] matrix = { { Cos(psi), Sin(psi)* Sin(fi), 0, 0 },
                { 0, Cos(fi), 0, 0 },
                { Sin(psi), -Sin(fi)* Cos(psi), 1, 0 },
                { 0, 0, 0, 1 } };

            return Points.MultiplicationMatrix(point.p, matrix);
        }
        //косоугольная 
        public static Points ObliqueProjection(this Points point, double l, double alpha)
        {
            alpha = DegtoRad(alpha);
            double[,] oblique ={ { 1,0, 0, 0 },
                               { 0, 1, 0, 0 },
                               { l*Cos(alpha), l*Sin(alpha), 1, 0 },
                               { 0, 0, 0, 1 } };

            return Points.MultiplicationMatrix(point.p, oblique); ;
        }
        //перспективная 
        public static Points PerspectiveProjection(this Points point, double d)
        {
            if ((0 <= point.Z) && (point.Z < 0.1))
            {
                point.Z = 0.1;
            }
            else if (point.Z < 0 && point.Z > -0.1)
            {
                point.Z = -0.1;
            }

            return new Points(point.X / (point.p[0, 2] / d), point.Y / (point.p[0, 2] / d), d);
        }
        //видовое преобразование
        public static Points SpeciesTransformation(this Points point, double teta, double fi, double ro)
        {
            teta = DegtoRad(teta);
            fi = DegtoRad(fi);
            double[,] spe ={ { -Sin(teta),-Cos(fi)*Cos(teta),-Sin(fi)*Cos(teta), 0 },
                               { Cos(teta), -Cos(fi)*Sin(teta), -Sin(fi)*Sin(teta), 0 },
                               { 0, Sin(fi), -Cos(fi), 0 },
                               { 0, 0, ro, 1 } };

            return Points.MultiplicationMatrix(point.p, spe); ;
        }


        //пока не нужно
        public static double Angle(Points p1, Points p2, Points p3,Points p4, Points lightpoints)
        {
            Points avector = Avector(p1, p2, p3);
            Points bvector = Bvector(p1, p2, p3, p4,lightpoints);
            double angle = Acos(VectorMultiplication(avector, bvector) / (VectorLenght(avector) * VectorLenght(bvector)));

            return angle;
        }
        public static Points Avector(Points p1, Points p2, Points p3)
        {
            return new Points(p1.Y * p2.Z + p2.Y * p3.Z + p3.Y * p1.Z - p2.Y * p1.Z - p3.Y * p2.Z - p1.Y * p3.Z,
                              p1.Z * p2.X + p2.Z * p3.X + p3.Z * p1.X - p2.Z * p1.X - p3.Z * p2.X - p1.Z * p3.X,
                              p1.X * p2.Y + p2.X * p3.Y + p3.X * p1.Y - p2.X * p1.Y - p3.X * p2.Y - p1.X * p3.Y);
        }
        public static Points Bvector(Points p1, Points p2, Points p3,Points p4, Points lightpoint)
        {
            return new Points(lightpoint.X - CenterPoint(p1, p2, p3,p4).X,
                              lightpoint.Y - CenterPoint(p1, p2, p3,p4).Y,
                              lightpoint.Z - CenterPoint(p1, p2, p3,p4).Z);
        }
        public static Points CenterPoint(Points p1, Points p2, Points p3, Points p4)
        {
            return new Points((p1.X + p2.X + p3.X + p4.X) / 4, (p1.Y + p2.Y + p3.Y + p4.Y) / 4, (p1.Z + p2.Z + p3.Z + p4.Z) / 4);
        }
        public static double VectorMultiplication(Points vectora, Points vectorb)
        {
            return vectora.X * vectorb.X + vectora.Y * vectorb.Y + vectora.Z * vectorb.Z;
        }
        public static double VectorLenght(Points vector)
        {
            return Sqrt(Pow(vector.X, 2) + Pow(vector.Y, 2) + Pow(vector.Z, 2));
        }

        public static double DegtoRad(double grad)
        {
            grad = grad * PI / 180;
            return grad;
        }
        public static double RadtoDeg(double grad)
        {
            return grad*180/PI;
        }
    }
}
