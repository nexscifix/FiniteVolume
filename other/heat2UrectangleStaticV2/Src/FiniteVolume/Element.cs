using System;
using System.IO;
using EVector = EgenLib.EMath.Vector3;
using Vector2 = EgenLib.EMath.Vector2;
using EMath = EgenLib.EMath;


namespace FiniteVolume2D
{


    public class Side
    {
        public const int EAST = 0;
        public const int WEST = 1;
        public const int NORTH = 2;
        public const int SOUTH = 3;
    }


    public class Element
    {
        public const int FACEMAX = 4;

        public Face[] faces = new Face[FACEMAX];
        public Element[] nbs = new Element[FACEMAX];
        public Vector2[] vertex = new Vector2[FACEMAX]; // от левый нижний против часовой
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
        public void SetRectangle(double x1, double y1, double w, double h)
        {
            vertex[0] = new Vector2(x1, y1);
            vertex[1] = new Vector2(x1 + w, y1);
            vertex[2] = new Vector2(x1 + w, y1 + h);
            vertex[3] = new Vector2(x1, y1 + h);
        }
        public void SetBoundary(int side, BoundaryType boundarytype, double value, double hcoeff = 0)
        {
            faces[side].SetBoundary(side, boundarytype, value, hcoeff);

        }
        public void PreCalc()
        {
            centroid = EMath.Geometry2D.PolygonCentroid(vertex);
            volume = (vertex[1].x - vertex[0].x) * (vertex[2].y - vertex[1].y); //poligonarea//EMath.Geometry2D.TriangleArea(vertex[0], vertex[1], vertex[2]);
        }
        public void Calc()
        {
            CreateFaces();
            int nf = 0;
            for (nf = 0; nf < nbs.Length; nf++)
            {
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


        // only one face between two nodes, first node is face owner 
        void CreateFaces()
        {
            //            int nf = 0;
            //            for (nf = 0; nf < vertex.Length - 1; nf++)
            //            {
            //                faces[nf] = new Face(vertex[nf], vertex[nf + 1]);
            //                faces[nf].Calc();
            //            }

            if (faces[Side.EAST] == null)
            {
                faces[Side.EAST] = new Face(vertex[1], vertex[2]);
                faces[Side.EAST].owner = this;
                if (nbs[Side.EAST] != null)
                {
                    faces[Side.EAST].nbelem = nbs[Side.EAST];
                    nbs[Side.EAST].faces[Side.WEST] = faces[Side.EAST];
                }

                faces[Side.EAST].Calc();
            }
            if (faces[Side.WEST] == null)
            {
                faces[Side.WEST] = new Face(vertex[3], vertex[0]);
                faces[Side.WEST].owner = this;
                if (nbs[Side.WEST] != null)
                {
                    faces[Side.WEST].nbelem = nbs[Side.WEST];
                    nbs[Side.WEST].faces[Side.EAST] = faces[Side.WEST];
                }

                faces[Side.WEST].Calc();
            }
            if (faces[Side.NORTH] == null)
            {
                faces[Side.NORTH] = new Face(vertex[2], vertex[3]);
                faces[Side.NORTH].owner = this;
                if (nbs[Side.NORTH] != null)
                {
                    faces[Side.NORTH].nbelem = nbs[Side.NORTH];
                    nbs[Side.NORTH].faces[Side.SOUTH] = faces[Side.NORTH];
                }

                faces[Side.NORTH].Calc();
            }
            if (faces[Side.SOUTH] == null)
            {
                faces[Side.SOUTH] = new Face(vertex[0], vertex[1]);
                faces[Side.SOUTH].owner = this;
                if (nbs[Side.SOUTH] != null)
                {
                    faces[Side.SOUTH].nbelem = nbs[Side.SOUTH];
                    nbs[Side.SOUTH].faces[Side.NORTH] = faces[Side.SOUTH];
                }

                faces[Side.SOUTH].Calc();
            }

            /*
            faces[Side.WEST] = new Face(vertex[3], vertex[0]);
            faces[Side.WEST].Calc();

            faces[Side.NORTH] = new Face(vertex[2], vertex[3]);
            faces[Side.NORTH].Calc();

            faces[Side.SOUTH] = new Face(vertex[0], vertex[1]);
            faces[Side.SOUTH].Calc();
*/
        }


    }




}

