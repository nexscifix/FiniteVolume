using System;
using System.IO;
using EVector = EgenLib.EMath.Vector3;
using Vector2 = EgenLib.EMath.Vector2;
using EMath = EgenLib.EMath;


namespace FiniteVolume2D
{

    public enum BoundaryType
    {
        BND_NO = 0,  // не граница
        BND_CONST = 1,  // задана температура на границе
        BND_INSULATED = 2,  // поток=0 через грань
        //        BND_FLUX = 2,  // задан тепловой поток на границе ( либо значение flux либо INSULATED flux=0)
//        BND_SURROUNDCONVECT = 3  // задан коэффициент конвекции и значение T окружающего воздуха; Hinfinity,Tinfinity 
    };


    public class Face
    {
        public int id;
        public int pointid1;
        public int pointid2;
        public Element owner;
        public Element nbelem;
        public Vector2[] vertex = new Vector2[2];
        public Vector2 centroid = new Vector2();
        // unit normal vector to face (outward to owner element)
        public Vector2 normal = new Vector2();
        public Vector2 sf = new Vector2();
        public double faceinterpolcoeff;
        public double area;
        public double k;
        public double u;
        //    public double nodesdst;
        public bool isboundary;
        public BoundaryType boundaryType;
        public int bnddomain ;
        public int bndgroup;
        public double bndu;
        public double bndflux;
        public double htranscoeff;

        public Face(Vector2 v1, Vector2 v2)
        {
            vertex[0] = v1;
            vertex[1] = v2;
            isboundary = false;
            bnddomain = -1;
            bndgroup = -1;
        }
        public static string GetFaceStrId(int p1index, int p2index)
        {
            if (p1index > p2index)
            {
                int t = p1index;
                p1index = p2index;
                p2index = t;
            }
            string facestr = p1index.ToString() + "_" + p2index.ToString();
            return facestr;
        }
        public void SetBoundary(BoundaryType boundarytype, double value, double hcoeff = 0)
        {
            boundaryType = boundarytype;
            isboundary = true;
            htranscoeff = hcoeff;
            if (boundaryType == BoundaryType.BND_CONST)// || boundaryType == BoundaryType.BND_SURROUNDCONVECT)
            {
                bndu = value;
            }
//            else if (boundaryType == BoundaryType.BND_FLUX)
//            {
//                bndflux = value;
//            }
        }
        public void Calc()
        {
            area = (vertex[0] - vertex[1]).Length();
            centroid = (vertex[0] + vertex[1]) / 2;
            normal = EMath.Geometry2D.LineNormal(vertex[0], vertex[1]);
            sf = EMath.Geometry2D.SurfaceNormal(vertex[0], vertex[1]);

            if (nbelem != null)
            {
                faceinterpolcoeff = owner.volume / (owner.volume + nbelem.volume);
                k = (owner.k * nbelem.k) / ((1 - faceinterpolcoeff) * owner.k + faceinterpolcoeff * nbelem.k);
            }
            else
            {
                k = owner.k;
            }
            //        sf = normal * area; //(vertex[0]-vertex[1]).Length();
        }
    }


}

