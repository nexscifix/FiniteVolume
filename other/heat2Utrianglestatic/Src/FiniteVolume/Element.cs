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
        public const int FACEMAX = 3;

        public Face[] faces = new Face[FACEMAX];
        public Element[] nbs = new Element[FACEMAX];
        public Vector2[] vertex = new Vector2[FACEMAX]; // от левый нижний против часовой
        public Node[] nodes = new Node[FACEMAX];
        public int nodecount;
        public Vector2[] cndistance = new Vector2[FACEMAX];
        public double[] nodedistances = new double[FACEMAX];
        public Vector2 centroid = new Vector2();
        public int id;
        public double volume;
        public double u;
        public double k;
        //    public double SourceCoeff;

        public Element()
        {
            u = 0;
            k = 0;
            id = -1;
        }
        /*
        public void SetRectangle(double x1, double y1, double w, double h)
        {
            vertex[0] = new Vector2(x1, y1);
            vertex[1] = new Vector2(x1 + w, y1);
            vertex[2] = new Vector2(x1 + w, y1 + h);
            vertex[3] = new Vector2(x1, y1 + h);
        }
         */ 
        public void SetTriangle(double x1, double y1,double x2, double y2,double x3, double y3)
        {
            vertex[0] = new Vector2(x1, y1);
            vertex[1] = new Vector2(x2, y2);
            vertex[2] = new Vector2(x3, y3);
        }
        public void SetBoundary(int side, BoundaryType boundarytype, double value, double hcoeff = 0)
        {
            faces[side].SetBoundary(side, boundarytype, value, hcoeff);

        }
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
        public void PreCalc()
        {
            centroid = EMath.Geometry2D.PolygonCentroid(vertex);
            volume =EMath.Geometry2D.TriangleArea(vertex[0], vertex[1], vertex[2]);
        }
        public void Calc()
        {
            CreateFaces();
        }


        // only one face between two nodes, first node is face owner 
        void CreateFaces()
        {
            int nf = 0;
            for (nf = 0; nf < vertex.Length; nf++)
            {

                if (faces[nf] == null)
                {
                    if(nf==vertex.Length-1)
                        faces[nf] = new Face(vertex[nf], vertex[0]);
                    else
                      faces[nf] = new Face(vertex[nf], vertex[nf+1]);
                    faces[nf].owner = this;
                    if (nbs[nf] != null)
                        faces[nf].nbelem = nbs[nf];
                }
            }

        }
        public void CalcFaces()
        {
            int nf = 0;
            for (nf = 0; nf < vertex.Length; nf++)
            {
                if (faces[nf].owner == this)
                {
                    if (nbs[nf] != null && faces[nf].owner == this)
                    {
                        Face f = nbs[nf].FindFaceByVertex(faces[nf].vertex[0], faces[nf].vertex[1]);
                        f = faces[nf];
                    }
                    faces[nf].Calc();
                }
                Element nb = nbs[nf];
                if (nb != null)
                {
                    cndistance[nf] = nb.centroid - centroid;
                    nodedistances[nf] = cndistance[nf].Length();
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

