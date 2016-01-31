using System;
using System.IO;
using EVector = EgenLib.EMath.Vector3;
using Vector2 = EgenLib.EMath.Vector2;
using EMath = EgenLib.EMath;


namespace FiniteVolume2D
{
    public class Node
    {
        public int id;
        public Vector2 vertex;
        public Element[] elements = new Element[10];
        public int elemcount;
        public double u;
        public int count;
    }

    public class Element
    {
        public Face[] faces;
        public Element[] nbs ;
        public Vector2[] vertex ; // от левый нижний против часовой
        public Node[] nodes;
        public int nodecount;
        // вектор до центра объема соседей
        public Vector2[] cndistance ;
        public double[] nodedistances ;
        // вектор от центра до центра граней
        public Vector2[] cfdistance;
        // вектор от центра соседа до центра граней
        public Vector2[] nfdistance ;
        // для интерполяции значений на грани между элементами по значениям в элементах
        public double[] geomInterpolateFactor ;
        public Vector2 centroid = new Vector2();
//        public double[] flux;
//        public double[] fluxT;
        public double u;
        public double k;
        public int id;
        public double volume;
        public Vector2 gradient = new Vector2();
        public bool isboundary;

        //    public double SourceCoeff;

        public Element(int vcount)
        {
            u = 0;
            k = 0;
            id = -1;
            isboundary = false;

            faces = new Face[vcount];
            nbs = new Element[vcount];
            vertex = new Vector2[vcount]; // от левый нижний против часовой
            nodes = new Node[vcount];
            cndistance = new Vector2[vcount];
            nodedistances = new double[vcount];
            cfdistance = new Vector2[vcount];
            nfdistance = new Vector2[vcount];
            geomInterpolateFactor = new double[vcount];
//            flux = new double[vcount];
         //   fluxT = new double[vcount];


        }
        /*
        public void SetTriangle(double x1, double y1,double x2, double y2,double x3, double y3)
        {
            vertex[0] = new Vector2(x1, y1);
            vertex[1] = new Vector2(x2, y2);
            vertex[2] = new Vector2(x3, y3);
        }
         */
        public void SetPolygon4(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
        {
            vertex[0] = new Vector2(x1, y1);
            vertex[1] = new Vector2(x2, y2);
            vertex[2] = new Vector2(x3, y3);
            vertex[3] = new Vector2(x4, y4);
        }
        public void SetPolygon(Vector2[] vertexs)
        {
            vertex = vertexs; // от левый нижний против часовой
        }
        /*
        public void SetBoundary(int side, BoundaryType boundarytype, double value, double hcoeff = 0)
        {
            faces[side].SetBoundary(boundarytype, value, hcoeff);

        }
         */ 
        public int FaceLocalId(Face face)
        {
            for (int n = 0; n < faces.Length; n++)
            {
                if (faces[n] == face)
                    return n;
            }
            return -1;
        }
        /*
        public Face FindFaceByVertex(Vector2 v1,Vector2 v2)
        {
            Face f = null;

            for (int n = 0; n < faces.Length; n++)
            {
                Face nf = faces[n];
                if (v1.Equal(nf.vertex[0]) || v2.Equal(nf.vertex[0]))
                {
                    if (v1.Equal(nf.vertex[1]) || v2.Equal(nf.vertex[1]))
                    {
                        f = nf;
                        break;
                    }
                }
            }

            return f;
        }
         */ 
        public void PreCalc()
        {
            centroid = EMath.Geometry2D.PolygonCentroid(vertex);
            //volume =EMath.Geometry2D.TriangleArea(vertex[0], vertex[1], vertex[2]);
            volume = EMath.Geometry2D.PolygonArea(vertex);
        }
        /*
        public void CalcFluxes()
        {
            int nf = 0;

            Vector2 gradC = CalcGradient();

            for (nf = 0; nf < vertex.Length; nf++)
            {
                Face face = faces[nf];
                Element nb = nbs[nf];
                if (face.isboundary)
                {
                    if (face.boundaryType == BoundaryType.BND_CONST)
                    {
                        double flux1nf;
                        flux1nf = k * (face.area / nodedistances[nf]);
                        Vector2 Sf = face.sf.Clone();
                        Vector2 dCf = cfdistance[nf].Clone();
                        if (Sf * dCf < 0)
                            Sf = -Sf;
                        //1) minimum correction
                        //Vector2 DCF = elem.cndistance[nf].Clone();
                        Vector2 e1 = dCf.GetNormalize();
                        Vector2 EF = (e1 * Sf) * e1;
                        flux[nf] = k * (EF.Length() / dCf.Length());

                    }
                    else if (face.boundaryType == BoundaryType.BND_INSULATED)
                    {
                        flux[nf] = 0;
                    }
                }
                else
                {
                    double flux1nf;
                    flux1nf = -k * (face.area / nodedistances[nf]);
                    Vector2 Sf = face.sf.Clone();
                    Vector2 dCf = cfdistance[nf].Clone();
                    if (Sf * dCf < 0)
                        Sf = -Sf;
                    //1) minimum correction
                    Vector2 DCF = cndistance[nf].Clone();
                    Vector2 e1 = DCF.GetNormalize();
                    //corrected flux
                    Vector2 EF = (e1 * Sf) * e1;

                    flux[nf] = -k * (EF.Length() / DCF.Length());

                    Vector2 gradnb = new Vector2();

                    Vector2 gradface = geomInterpolateFactor[nf] * gradnb + (1 - geomInterpolateFactor[nf]) * gradC;
                    Vector2 TF = Sf - EF;
                    double fluxT = -(gradface * TF);

                    flux[nf] += fluxT;

                }

            }
        }
        */

        public void CalcGradient()
        {
//            return;
            int nf = 0;
            gradient = new Vector2();
            for (nf = 0; nf < vertex.Length; nf++)
            {
                if (faces[nf].isboundary)
                {
                    faces[nf].u = faces[nf].bndu;
                }
                else
                {
                    //faces[nf].u = (u + nbs[nf].u) / 2.0;
                    double gInterpolateFactor = cfdistance[nf].Length() / cndistance[nf].Length();
                    faces[nf].u = gInterpolateFactor * nbs[nf].u + (1 - gInterpolateFactor) * u;
                }
                Vector2 Sf = faces[nf].sf.Clone();
                Vector2 dCf = cfdistance[nf].Clone();
                if (Sf * dCf < 0)
                    Sf = -Sf;
                gradient+=faces[nf].u * Sf;
            }
            gradient = gradient / volume;

        }
        public void Calc()
        {
            int nf = 0;
            for (nf = 0; nf < vertex.Length; nf++)
            {
                if (faces[nf].owner == this)
                    faces[nf].Calc();
                Element nb = nbs[nf];
                cfdistance[nf] = faces[nf].centroid - centroid;
                if (nb != null)
                {
                    cndistance[nf] = nb.centroid - centroid;
                    nodedistances[nf] = cndistance[nf].Length();
                    nfdistance[nf] = nb.centroid - faces[nf].centroid;

                    geomInterpolateFactor[nf] = cfdistance[nf].Length() / (cfdistance[nf].Length() + nfdistance[nf].Length());

                }
                else
                {
                    cndistance[nf] = faces[nf].centroid - centroid;
                    nodedistances[nf] = cndistance[nf].Length();
                }

            }

        }


    }




}

