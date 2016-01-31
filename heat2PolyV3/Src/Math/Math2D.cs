using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using Evector = EgenLib.EMath.Vector2;

namespace EgenLib.EMath
{

    public class Vector2
    {
        public double x = 0;
        public double y = 0;
//        public double k = 1;

        public Vector2()
        {
        }
        public Vector2(double x_, double y_)
        {
            x = x_;
            y = y_;
//            k = 1;
        }
        public Vector2 Clone()
        {
            return new Vector2(x, y);
        }
        public double this[int i]
        {
            get
            {
                if (i >= 2)
                    throw new Exception("bad vector index");
                if (i == 0)
                    return x;
                else if (i == 1)
                    return y;
                return 0;
            }
            set
            {
                if (i >= 2)
                    throw new Exception("bad vector index");
                if (i == 0)
                    x = value;
                else if (i == 1)
                    y = value;
//                k = 0;
            }
        }
        // норма или длинна вектора  |v|   ||v|| 
        public double Length()
        {
            return Math.Sqrt(x * x + y * y );
        }
        public double LengthSquared()
        {
            return x * x + y * y ;
        }
        // нахождение единичного вектора направление которого совпадает с направлением текущего
        public Vector2 GetNormalize()
        {
            Vector2 result = new Vector2();
            double length = Length();
            double tolerance = 0.00001;
            if (length >= tolerance)
            {
                result.x = x / length;
                result.y = y / length;
            }
            else
            {
              x = 0;
              y = 0;
            }
            return result;
        }
        // нахождение единичного вектора направление которого совпадает с направлением текущего
        public void Normalize()
        {
            double length = Length();
            double tolerance = 0.00001;
            if (length >= tolerance)
            {
                x = x / length;
                y = y / length;
            }
            else
            {
              x = 0;
              y = 0;
            }
        }
/*
        // векторное произведение векторов AxB
        // только для 3D векторов
        // результат - вектор(нормаль) перпендикулярный к плоскости в которой лежат перемножаемые вектора
        public static Vector2 operator %(Vector2 a, Vector2 v)
        {
            return new Vector2(
             a.y * v.z - a.z * v.y,
             a.z * v.x - a.x * v.z,
             a.x * v.y - a.y * v.x);
        }
*/
        public bool Equal(Vector2 a)
        {
            return x==a.x && y==a.y;
        }
        public static double operator *(Vector2 a, Vector2 b)
        {
            return (a.x * b.x + a.y * b.y );
        }
        public static Vector2 operator *(double a, Vector2 b)
        {
            return new Vector2(a * b.x, a * b.y);
        }
        public static Vector2 operator *(Vector2 b, double a)
        {
            return new Vector2(a * b.x, a * b.y);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }
        public static Vector2 operator +(double a, Vector2 b)
        {
            return new Vector2(a + b.x, a + b.y);
        }
        public static Vector2 operator +(Vector2 b, double a)
        {
            return new Vector2(a + b.x, a + b.y);
        }

        public static Vector2 operator -(Vector2 b)
        {
            return new Vector2(-b.x, -b.y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }
        public static Vector2 operator -(double a, Vector2 b)
        {
            return new Vector2(a - b.x, a - b.y);
        }
        public static Vector2 operator -(Vector2 b, double a)
        {
            return new Vector2(b.x - a, b.y - a);
        }

        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x / b.x, a.y / b.y);
        }
        public static Vector2 operator /(double a, Vector2 b)
        {
            return new Vector2(a / b.x, a / b.y);
        }
        public static Vector2 operator /(Vector2 b, double a)
        {
            return new Vector2(b.x / a, b.y / a);
        }

    }

    public class Geometry2D
    {
        public static double TriangleArea(Evector v1, Evector v2, Evector v3)
        {
//            double triarea = Math.Abs(((v1 - v3) % (v2 - v3)).Length()) / 2;
            double triarea = Math.Abs(v1.x * (v2.y - v3.y) + v2.x * (v3.y - v1.y) + v3.x * (v1.y - v2.y)) / 2.0;
            return triarea;
        }
        public static Evector TriangleCenter(Evector v1, Evector v2, Evector v3)
        {
            return (v1 + v2 + v3) / 3;
        }
        public static Evector LineNormal(Evector p1, Evector p2)
        {
            Evector dif=p2-p1;
            Evector n = new Evector(dif.y, -dif.x);
            n.Normalize();
            return n;
        }
        public static Evector SurfaceNormal(Evector p1, Evector p2)
        {
            Evector dif = p2 - p1;
            Evector n = new Evector(dif.y, -dif.x);
            return n;
        }
        public static Evector PolygonCentroid(Evector[] vertex)
        {
            Evector PolyGeomCentr = new Evector();
            foreach (Evector v in vertex)
            {
                PolyGeomCentr += v;
            }
            PolyGeomCentr /= vertex.Length;

            Evector PolygonCentr = new Evector();
            double polygonarea = 0;
            for (int n = 0; n < vertex.Length; n++)
            {
                Evector v1 = vertex[n];
                Evector v2 ;
                if(n==vertex.Length-1)
                    v2=vertex[0];
                else
                  v2 = vertex[n + 1];
                Evector TriangleGeomCentr = TriangleCenter(v1, v2, PolyGeomCentr);
                double triarea = TriangleArea(v1, v2, PolyGeomCentr);
                polygonarea += triarea;
                PolygonCentr += triarea * TriangleGeomCentr;
            }

            PolygonCentr /= polygonarea;
            return PolygonCentr;

        }
        public static double PolygonArea(Evector[] vertex)
        {
            Evector PolyGeomCentr = new Evector();
            foreach (Evector v in vertex)
            {
                PolyGeomCentr += v;
            }
            PolyGeomCentr /= vertex.Length;

            double polygonarea = 0;
            for (int n = 0; n < vertex.Length; n++)
            {
                Evector v1 = vertex[n];
                Evector v2;
                if (n == vertex.Length - 1)
                    v2 = vertex[0];
                else
                    v2 = vertex[n + 1];
                double triarea = TriangleArea(v1, v2, PolyGeomCentr);
                polygonarea += triarea;
            }

            return polygonarea;

        }

    };

}