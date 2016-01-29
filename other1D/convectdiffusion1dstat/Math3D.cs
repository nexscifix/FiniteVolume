using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace EgenLib.EMath
{

    public class Matrix
    {
        // расположение елементов (как в Directx)
        //  _00, _01, _02, _03
        //  _10, _11, _12, _13
        //  _20, _21, _22, _23
        //  _30, _31, _32, _33
        // m[строки][столбцы],m[i*cols+j]  
        public int rows;
        public int cols;
        public double[,] m = null;

        public void Alloc()
        {
            m = new double[rows, cols];
        }
        public Matrix Clone()
        {
            Matrix mat = new Matrix(rows, cols);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    mat.m[i, j] = m[i, j];
            return mat;
        }
        public Matrix(int maxrows_, int maxcols_)
        {
            rows = maxrows_;
            cols = maxcols_;
            Alloc();
        }
        public Matrix(int maxrows_, int maxcols_, double initvalue)
        {
            rows = maxrows_;
            cols = maxcols_;
            Alloc();
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    m[i, j] = initvalue;
        }
        /*
          public Matrix(double x, double y, double z)
          {
            rows = 3;
            cols = 1;
            Alloc();
            m[0,0] = x;
            m[1, 0] = y;
            m[2, 0] = z;
          }
         */
        public Matrix(double x, double y, double z, double k)
        {
            rows = 4;
            cols = 1;
            Alloc();
            m[0, 0] = x;
            m[1, 0] = y;
            m[2, 0] = z;
            m[3, 0] = k;
        }
        public Matrix(Vector3 v)
        {
            rows = 4;
            cols = 1;
            Alloc();
            m[0, 0] = v.x;
            m[1, 0] = v.y;
            m[2, 0] = v.z;
            m[3, 0] = v.k;
        }
        public int GetColumns()
        {
            return cols;
        }
        public int GetRows()
        {
            return rows;
        }
        public double this[int row, int col]
        {
            get
            {
                if (row >= rows)
                    throw new Exception("bad matrix index");
                if (col >= cols)
                    throw new Exception("bad matrix index");
                return m[row, col];
            }
            set
            {
                if (row >= rows)
                    throw new Exception("bad matrix index");
                if (col >= cols)
                    throw new Exception("bad matrix index");
                m[row, col] = value;
            }
        }
        public double this[int row]
        {
            get
            {
                if (row >= rows)
                    throw new Exception("bad matrix index");
                return m[row, 0];
            }
            set
            {
                if (row >= rows)
                    throw new Exception("bad matrix index");
                m[row, 0] = value;
            }
        }
        public Matrix GetDiagonal()
        {
            Matrix result = new Matrix(rows, cols);
            result.MakeZero();
            for (int i = 0; i < rows; i++)
                result[i, i] = m[i, i];
            return result;
        }
        public void CopyRowFrom(Matrix src, int srcrownum, int destrownum)
        {
            for (int i = 0; i < cols; i++)
                m[destrownum, i] = src[srcrownum, i];
        }
        public void CopyColFrom(Matrix src, int srccolnum, int destcolnum)
        {
            for (int i = 0; i < rows; i++)
                m[i, destcolnum] = src[i, srccolnum];
        }
        public Matrix GetAbs()
        {
            Matrix result = new Matrix(rows, cols);
            result.MakeZero();
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[i, j] = Math.Abs(m[i, j]);
            return result;
        }
        public void FindMaxElement(out int row, out int col, out double element)
        {
            element = -1000000;
            row = -1;
            col = -1;
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    if (m[i, j] > element)
                    {
                        element = m[i, j];
                        row = i;
                        col = j;
                    }
        }
        public Matrix GetTranspose()
        {
            Matrix result = new Matrix(cols, rows);
            result.MakeZero();
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[j, i] = m[i, j];
            return result;
        }
        public void MakeIdentity()
        {
            MakeZero();
            for (int i = 0; i < rows; i++)
                m[i, i] = 1.0;
        }
        public void MakeZero()
        {
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    m[i, j] = 0.0;
        }
        public static Matrix GetIdentity4()
        {
            Matrix result = new Matrix(4, 4);
            result.MakeZero();
            for (int i = 0; i < result.rows; i++)
                result[i, i] = 1.0;
            return result;
        }
        public static Matrix GetZero4()
        {
            Matrix result = new Matrix(4, 4);
            result.MakeZero();
            return result;
        }
        public static Matrix GetOrthogonalProjection4(double x1, double x2, double y1, double y2, double z1, double z2)
        {
            Matrix result = new Matrix(4, 4);
            result.MakeZero();
            result[0, 0] = 2.0 / (x2 - x1);
            result[1, 1] = 2.0 / (y2 - y1);
            result[2, 2] = 2.0 / (z2 - z1);
            result[3, 0] = (x2 + x1) / (x2 - x1);
            result[3, 0] = (y2 + y1) / (y2 - y1);
            result[3, 0] = (z2 + z1) / (z2 - z1);
            result[3, 3] = 1.0;
            return result;
        }
        public static Matrix GetTranslation4(double tx, double ty, double tz)
        {
            Matrix result = new Matrix(4, 4);
            result.MakeIdentity();
            result[0, 3] = tx;
            result[1, 3] = ty;
            result[2, 3] = tz;
            return result;
        }
        public static Matrix GetScale4(double sx, double sy, double sz)
        {
            Matrix result = new Matrix(4, 4);
            result.MakeIdentity();
            result[0, 0] = sx;
            result[1, 1] = sy;
            result[2, 2] = sz;
            return result;
        }
        public static Matrix GetFlipY4()
        {
            Matrix result = new Matrix(4, 4);
            result.MakeIdentity();
            result[0, 0] = -1.0;
            return result;
        }
        public static Matrix GetFlipX4()
        {
            Matrix result = new Matrix(4, 4);
            result.MakeIdentity();
            result[1, 1] = -1.0;
            return result;
        }
        public static Matrix GetRotationZ4(double angle)
        {
            Matrix result = new Matrix(4, 4);
            result.MakeIdentity();
            angle *= Math.PI / 180.0;
            result[0, 0] = Math.Cos(angle);
            result[0, 1] = Math.Sin(angle);
            result[1, 0] = -Math.Sin(angle);
            result[1, 1] = Math.Cos(angle);
            return result;
        }
        public static Matrix GetRotationY4(double angle)
        {
            Matrix result = new Matrix(4, 4);
            result.MakeIdentity();
            angle *= Math.PI / 180.0;
            result[0, 0] = Math.Cos(angle);
            result[0, 2] = -Math.Sin(angle);
            result[2, 0] = Math.Sin(angle);
            result[2, 2] = Math.Cos(angle);
            return result;
        }
        public static Matrix GetRotationX4(double angle)
        {
            Matrix result = new Matrix(4, 4);
            result.MakeIdentity();
            angle *= Math.PI / 180.0;
            result[1, 1] = Math.Cos(angle);
            result[1, 2] = Math.Sin(angle);
            result[2, 1] = -Math.Sin(angle);
            result[2, 2] = Math.Cos(angle);
            return result;
        }
        public static Matrix GetRotateZAndTranslation4(double angle, double tx, double ty, double tz)
        {
            Matrix result = new Matrix(4, 4);
            Matrix rot = Matrix.GetRotationZ4(angle);
            Matrix tran = Matrix.GetTranslation4(tx, ty, tz);
            result = tran * rot;

            return result;
        }
        public static Matrix GetRotationByAxes(Vector3 axe, double angle)
        {
            angle *= Math.PI / 180.0;
            double x = axe.x;
            double y = axe.y;
            double z = axe.z;
            double c = Math.Cos(angle);
            double s = Math.Sin(angle);
            double t = 1 - Math.Cos(angle);

            Matrix m = new Matrix(4, 4);
            m.MakeIdentity();
            m[0, 0] = t * x * x + c; m[0, 1] = t * x * y - s * z; m[0, 2] = t * x * z + s * y;          //m[0,3] =;
            m[1, 0] = t * x * y + s * z; m[1, 1] = t * y * y + c; m[1, 2] = t * y * z - s * x;          //m[1,3] =;
            m[2, 0] = t * x * z - s * y; m[2, 1] = t * y * z + s * x; m[2, 2] = t * z * z + c;   //m[2,3] =;
            //      m[3,0] =;          m[3,1] =;         m[3,2] =;          m[3,3] =;

            return m;
        }
        public static Vector3 Transform(Vector3 point, Matrix t)
        {
            Matrix p = new Matrix(point.x, point.y, point.z, point.k);
            Matrix pnew = t * p;
            return new Vector3(pnew[0], pnew[1], pnew[2], pnew[3]);
        }
        public Vector3 Transform(Vector3 point)
        {
            Matrix p = new Matrix(point.x, point.y, point.z, point.k);
            Matrix pnew = this * p;
            //      Matrix pnew= p*this;
            return new Vector3(pnew[0], pnew[1], pnew[2], pnew[3]);
        }
        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.GetColumns() != b.GetRows())
                throw new Exception("a.columns must be = b.rows");
            Matrix c = new Matrix(a.GetRows(), b.GetColumns());

            for (int i = 0; i < a.GetRows(); i++)
            {
                for (int j = 0; j < b.GetColumns(); j++)
                {
                    for (int k = 0; k < a.GetColumns(); k++)
                    {
                        c[i, j] += a[i, k] * b[k, j];
                    }
                }
            }

            return c;
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.GetColumns() != b.GetColumns()
              || a.GetRows() != b.GetRows()
              )
                throw new Exception("size a and b matrix must be equal");
            Matrix c = new Matrix(a.GetColumns(), b.GetRows());
            for (int i = 0; i < a.GetRows(); i++)
                for (int j = 0; j < a.GetColumns(); j++)
                    c[i, j] = a[i, j] + b[i, j];
            return c;
        }
        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.GetColumns() != b.GetColumns()
              || a.GetRows() != b.GetRows()
              )
                throw new Exception("size a and b matrix must be equal");
            Matrix c = new Matrix(a.GetColumns(), b.GetRows());
            for (int i = 0; i < a.GetRows(); i++)
                for (int j = 0; j < a.GetColumns(); j++)
                    c[i, j] = a[i, j] - b[i, j];
            return c;
        }
        public static implicit operator Vector3(Matrix v)
        {
            return new Vector3(v[0, 0], v[1, 0], v[2, 0], v[3, 0]);
        }
        public string ToString(bool inonestring)
        {
            string s = "";
            string strnumber;
            for (int k = 0; k < rows; k++)
            {
                for (int l = 0; l < cols; l++)
                {
                    strnumber = m[k, l].ToString();
                    strnumber = strnumber.Replace(",", ".");
                    if (strnumber.IndexOf(".") == -1)
                        strnumber += ".0";
                    while (strnumber.Length < 17)
                        strnumber += "0";
                    s += " " + strnumber;
                }
                if (k < rows - 1 && !inonestring)
                    s += "\n";
            }
            return s;
        }
        public void ToFile(bool append, bool inonestring, string filename, string before, string after)
        {
            string s = before + ToString(inonestring) + after;
            StreamWriter w = null;
            if (append)
                w = File.AppendText(filename);
            else
                w = new StreamWriter(filename);
            w.Write(s);
            w.Flush();
            w.Close();
        }

    }



    public class Vector3
    {
        public double x = 0;
        public double y = 0;
        public double z = 0;
        public double k = 1;

        public Vector3()
        {
        }
        public Vector3(double x_, double y_, double z_)
        {
            x = x_;
            y = y_;
            z = z_;
            k = 1;
        }
        public Vector3(double x_, double y_, double z_, double k_)
        {
            x = x_;
            y = y_;
            z = z_;
            k = k_;
        }
        public Vector3 Clone()
        {
            return new Vector3(x, y, z, k);
        }
        public double this[int i]
        {
            get
            {
                if (i >= 4)
                    throw new Exception("bad vector index");
                if (i == 0)
                    return x;
                else if (i == 1)
                    return y;
                else if (i == 2)
                    return z;
                return k;
            }
            set
            {
                if (i >= 4)
                    throw new Exception("bad vector index");
                if (i == 0)
                    x = value;
                else if (i == 1)
                    y = value;
                else if (i == 2)
                    z = value;
                k = value;
            }
        }
        // норма или длинна вектора  |v|   ||v|| 
        public double Length()
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }
        public double LengthSquared()
        {
            return x * x + y * y + z * z;
        }
        // нахождение единичного вектора направление которого совпадает с направлением текущего
        public Vector3 GetNormalize()
        {
            Vector3 result = new Vector3();
            double length = Length();
            double tolerance = 0.00001;
            if (length >= tolerance)
            {
                result.x = x / length;
                result.y = y / length;
                result.z = z / length;
            }
            else
            {
              x = 0;
              y = 0;
              z = 0;
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
                z = z / length;
            }
            else
            {
              x = 0;
              y = 0;
              z = 0;
            }
        }
        // векторное произведение векторов AxB
        // только для 3D векторов
        // результат - вектор(нормаль) перпендикулярный к плоскости в которой лежат перемножаемые вектора
        public static Vector3 operator %(Vector3 a, Vector3 v)
        {
            return new Vector3(
             a.y * v.z - a.z * v.y,
             a.z * v.x - a.x * v.z,
             a.x * v.y - a.y * v.x);
        }
        public static double operator *(Vector3 a, Vector3 b)
        {
            return (a.x * b.x + a.y * b.y + a.z * b.z);
        }
        /*
        // скалярное(внутреннее) перемножение векторов a*b
        // если оно равно 0 то вектора перпендикулярны (ортогональны) - угол = 90
        public double Dot(Vector3 v) 
        { 
            return (x*v.x + y*v.y + z*v.z); 
        }
                public static Vector3 operator *(Vector3 a, Vector3 b)
                {
                    return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
                }
         */
        public static Vector3 operator *(double a, Vector3 b)
        {
            return new Vector3(a * b.x, a * b.y, a * b.z);
        }
        public static Vector3 operator *(Vector3 b, double a)
        {
            return new Vector3(a * b.x, a * b.y, a * b.z);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public static Vector3 operator +(double a, Vector3 b)
        {
            return new Vector3(a + b.x, a + b.y, a + b.z);
        }
        public static Vector3 operator +(Vector3 b, double a)
        {
            return new Vector3(a + b.x, a + b.y, a + b.z);
        }

        public static Vector3 operator -(Vector3 b)
        {
            return new Vector3(-b.x, -b.y, -b.z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        public static Vector3 operator -(double a, Vector3 b)
        {
            return new Vector3(a - b.x, a - b.y, a - b.z);
        }
        public static Vector3 operator -(Vector3 b, double a)
        {
            return new Vector3(b.x - a, b.y - a, b.z - a);
        }

        public static Vector3 operator /(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }
        public static Vector3 operator /(double a, Vector3 b)
        {
            return new Vector3(a / b.x, a / b.y, a / b.z);
        }
        public static Vector3 operator /(Vector3 b, double a)
        {
            return new Vector3(b.x / a, b.y / a, b.z / a);
        }

    }

    public class Geometry
    {
        public static double PIOVER180 = 0.0174532925199432957692369076848861f;
        public static double PIUNDER180 = 57.2957795130823208767981548141052f;
        public static double DEG_TO_RAD(double angle)
        {
            return angle * PIOVER180;
        }
        public static double RAD_TO_DEG(double radians)
        {
            return radians * PIUNDER180;
        }
        // расчитывает ось вокруг которой нужно повернуть вектор src на угол angle ,чтобы получить вектор dest
        public static void CalcRotationAxe(Vector3 src, Vector3 dest,ref Vector3 axe,ref double angle)
        {
          Vector3 va = src.Clone();
          va.Normalize();
          Vector3 vb = dest.Clone();
          vb.Normalize();

          Vector3 normal = va % vb;
          axe=normal;
          double cos = va * vb;
          angle = Geometry.RAD_TO_DEG(Math.Acos(cos));
        }
      /* not working
        public static void CalcVectorAngle(Vector3 v1, ref double anglex, ref double angley)
        {
          //EMath.Vector3 vv1 = new EMath.Vector3(1.0f, 0.0f, 1.0f);
          //EMath.Vector3 vv2 = new EMath.Vector3(0.7f, 0.7f, 0.7f);
          //vv1.Normalize();
          //vv2.Normalize();
          //double co = vv1 * vv2 / (vv1.Length() * vv2.Length());
          //double ang = EMath.Geometry.RAD_TO_DEG(Math.Acos(co));

          double eps = 0.001;
          //      Vector3 v = new Vector3(0.0f, -1.0f, 0.0f);
          Vector3 v = v1;
          if (v.Length() < eps)
            return;

          Vector3 ox = new Vector3(1.0f, 0.0f, 0.0f);

          Vector3 vector2d = new Vector3(v.x, 0, v.z);
          anglex = 0;
          if (vector2d.Length() > eps)
          {
            double cosox = vector2d * ox / (vector2d.Length() * ox.Length());
            anglex = Geometry.RAD_TO_DEG(Math.Acos(cosox));
            if (vector2d.z < 0)
              anglex = -anglex;
          }
          angley = 0;
          if (vector2d.Length() > eps)
          {
            double cosv = vector2d * v / (vector2d.Length() * v.Length());
            angley = Geometry.RAD_TO_DEG(Math.Acos(cosv));
          }
          else
            angley = 90;

          if (v.y < 0)
            angley = -angley;
        }
      */
        public static bool LineLineIntersect(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, out Vector3 pa, out Vector3 pb, out double mindist)
        {
            //,
            //   double *mua, double *mub)
            /*
               Calculate the line segment PaPb that is the shortest route between
               two lines P1P2 and P3P4. Calculate also the values of mua and mub where
                  Pa = P1 + mua (P2 - P1)
                  Pb = P3 + mub (P4 - P3)
               Return FALSE if no solution exists.
            */
            mindist = 65000;
            double mua = 0;
            double mub = 0;
            double EPS = 0.001;
            Vector3 p13 = new Vector3();
            Vector3 p43 = new Vector3();
            Vector3 p21 = new Vector3();
            double d1343, d4321, d1321, d4343, d2121;
            double numer, denom;

            pa = new Vector3();
            pb = new Vector3();

            p13.x = p1.x - p3.x;
            p13.y = p1.y - p3.y;
            p13.z = p1.z - p3.z;
            p43.x = p4.x - p3.x;
            p43.y = p4.y - p3.y;
            p43.z = p4.z - p3.z;
            if (Math.Abs(p43.x) < EPS && Math.Abs(p43.y) < EPS && Math.Abs(p43.z) < EPS)
                return false;
            p21.x = p2.x - p1.x;
            p21.y = p2.y - p1.y;
            p21.z = p2.z - p1.z;
            if (Math.Abs(p21.x) < EPS && Math.Abs(p21.y) < EPS && Math.Abs(p21.z) < EPS)
                return false;

            d1343 = p13.x * p43.x + p13.y * p43.y + p13.z * p43.z;
            d4321 = p43.x * p21.x + p43.y * p21.y + p43.z * p21.z;
            d1321 = p13.x * p21.x + p13.y * p21.y + p13.z * p21.z;
            d4343 = p43.x * p43.x + p43.y * p43.y + p43.z * p43.z;
            d2121 = p21.x * p21.x + p21.y * p21.y + p21.z * p21.z;

            denom = d2121 * d4343 - d4321 * d4321;
            if (Math.Abs(denom) < EPS)
                return false;
            numer = d1343 * d4321 - d1321 * d4343;

            mua = numer / denom;
            mub = (d1343 + d4321 * (mua)) / d4343;

            pa.x = p1.x + mua * p21.x;
            pa.y = p1.y + mua * p21.y;
            pa.z = p1.z + mua * p21.z;
            pb.x = p3.x + mub * p43.x;
            pb.y = p3.y + mub * p43.y;
            pb.z = p3.z + mub * p43.z;

            Vector3 a = p1;
            Vector3 b = p2;
            Vector3 c = p3;
            Vector3 d = p4;

            double DISTEPS = 0.1;
            bool result = false;
            mindist = Math.Sqrt((pb.x - pa.x) * (pb.x - pa.x) + (pb.y - pa.y) * (pb.y - pa.y) + (pb.z - pa.z) * (pb.z - pa.z));
            if (mindist < DISTEPS)
            {
                //принадлежит ли точка этим отрезкам
                double distab = Math.Sqrt((b.x - a.x) * (b.x - a.x) + (b.y - a.y) * (b.y - a.y) + (b.z - a.z) * (b.z - a.z));
                double distcd = Math.Sqrt((d.x - c.x) * (d.x - c.x) + (d.y - c.y) * (d.y - c.y) + (d.z - c.z) * (d.z - c.z));
                double distap = Math.Sqrt((pa.x - a.x) * (pa.x - a.x) + (pa.y - a.y) * (pa.y - a.y) + (pa.z - a.z) * (pa.z - a.z));
                double distbp = Math.Sqrt((pa.x - b.x) * (pa.x - b.x) + (pa.y - b.y) * (pa.y - b.y) + (pa.z - b.z) * (pa.z - b.z));
                double distcp = Math.Sqrt((pa.x - c.x) * (pa.x - c.x) + (pa.y - c.y) * (pa.y - c.y) + (pa.z - c.z) * (pa.z - c.z));
                double distdp = Math.Sqrt((pa.x - d.x) * (pa.x - d.x) + (pa.y - d.y) * (pa.y - d.y) + (pa.z - d.z) * (pa.z - d.z));
                if ((distab - distap) > -DISTEPS && (distab - distbp) > -DISTEPS
                  && (distcd - distcp) > -DISTEPS && (distcd - distdp) > -DISTEPS
                )
                    result = true;
                //        distab=distab+0;
            }

            return result;
        }

    };

}