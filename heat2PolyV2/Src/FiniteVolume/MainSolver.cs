using System;
using System.Collections.Generic;
using System.IO;
//using REAL = System.Double;
using EVector = EgenLib.EMath.Vector3;
using Vector2 = EgenLib.EMath.Vector2;
using FiniteVolume2D;

namespace FiniteVolume2D
{

    public class MainSolver
    {
        int elementcount;
        public Element[] elements;
        public Face[] faces;
        public Face[] bndfaces;
        //        public Node[] nodes;
        public int nodecount;
        double delt;
        int count;


        public MainSolver()
        {
        }
        public double GetStepSize()
        {
            return delt;
        }

        // -------------------- COMPUTE ---------------------------------
        // -------------------- COMPUTE ---------------------------------


        public void Calc()
        {

            double bc, ac;
            double sumflux;
            double[] aa = new double[6];
            double[] bb = new double[6];

            int e;
            for (e = 0; e < elementcount; e++)
            {
                Element elem = elements[e];
                int nf;
                bc = 0;
                ac = 0;
                sumflux = 0;
                for (int nn = 0; nn <6; nn++)
                {
                    aa[nn] = 0;
                    bb[nn] = 0;
                }
                for (nf = 0; nf < elem.vertex.Length; nf++)
                {
                    Face face = elem.faces[nf];
                    Element nb = elem.nbs[nf];
                    if (face.isboundary)
                    {
                        if (face.boundaryType == BoundaryType.BND_CONST)
                        {
                            ac += elem.flux[nf];
                            bc += elem.flux[nf] * face.bndu;
                            bb[nf] = elem.flux[nf];
                        }
                        else if (face.boundaryType == BoundaryType.BND_INSULATED)
                        {
                            ac += elem.flux[nf];
                            bc += elem.flux[nf] * face.bndu;
                            bb[nf] = elem.flux[nf];

                        }
                    }
                    else
                    {
                        sumflux += elem.flux[nf] * nb.u;
                        ac += -elem.flux[nf];
                        aa[nf] = -elem.flux[nf];
                    }

                }
                elem.u = elem.u + delt * (bc - sumflux - ac * elem.u);


            }

        }


        // -------------------- MAIN ---------------------------------
        // -------------------- MAIN ---------------------------------


        public void Destroy()
        {

        }
        public void SetInitialBoundary()
        {
            int e;
            Element elem;
            for (e = 0; e < elementcount; e++)
            {
                elem = elements[e];
                elem.u = 0;
            }
            SetPeriodicBoundary();
        }
        /*
        //p.out
        public void SetPeriodicBoundary()
        {
            int f;
            Face face;
            for (f = 0; f < bndfaces.Length; f++)
            {
                face=bndfaces[f];
                if (face.bnddomain == 0)
                {
                    face.SetBoundary(BoundaryType.BND_CONST, 0);
                }
                else
                    face.SetBoundary(BoundaryType.BND_CONST, 255);
            }

        }
           */
        /*
        //std.out stdref.out
        public void SetPeriodicBoundary()
        {
            int f;
            Face face;
            for (f = 0; f < bndfaces.Length; f++)
            {
                face = bndfaces[f];
                if (face.bndgroup == 3)
                {
                    face.SetBoundary(BoundaryType.BND_CONST, 255);
                }
                else
                    face.SetBoundary(BoundaryType.BND_CONST, 0);
            }

        }
          */
        //circle.out eye
        public void SetPeriodicBoundary()
        {
            int f;
            Face face;
            for (f = 0; f < bndfaces.Length; f++)
            {
                face = bndfaces[f];
                if (face.bnddomain == 0)
                {
                    face.SetBoundary(BoundaryType.BND_CONST, 295);
                }
                else
                    face.SetBoundary(BoundaryType.BND_CONST, 0);
            }

        }
        /*
        //circle.out
        public void SetPeriodicBoundary()
        {
            int f;
            Face face;
            for (f = 0; f < bndfaces.Length; f++)
            {
                face = bndfaces[f];
                if (face.bnddomain == 0 && (face.bndgroup==6 || face.bndgroup==8))
//                    if (face.bnddomain == 0)
                {
                    face.SetBoundary(BoundaryType.BND_CONST, 255);
                }
                else
                    face.SetBoundary(BoundaryType.BND_CONST, 0);
            }

        }
         */
        /*
        //triangle.out
        public void SetPeriodicBoundary()
        {
            int f;
            Face face;
            for (f = 0; f < bndfaces.Length; f++)
            {
                face = bndfaces[f];
                if (face.bndgroup == 5 || face.bndgroup == 1 || face.bndgroup == 7)
                {
                    face.SetBoundary(BoundaryType.BND_CONST, 255);
                }
                else
                    face.SetBoundary(BoundaryType.BND_CONST, 0);
            }

        }
         */ 
        public void SetParameters()
        {
            delt = 0.015;
        }
        public bool Create(string filename_)
        {
            double[,] points=null ;
            double[,] polys = null;
            double[,] bound = null;


            Dictionary<string, Face> facedic = new Dictionary<string, Face>();


            if (!LoadFromFile(filename_, ref points, ref polys, ref bound))
                return false;
            count = 0;
            SetParameters();
            elementcount = polys.GetLength(1);
            int vertexcount = polys.GetLength(0)-1;
            elements = new Element[elementcount];

            int faceid = 0;
            int e = 0;
            for (e = 0; e < elementcount; e++)
            {
                Element elem;
                elements[e] = new Element(vertexcount);
                elem = elements[e]; elem.id = e; elem.k = 1;
                Vector2[] vertex = new Vector2[vertexcount];
                for (int v = 0; v < vertexcount; v++)
                {
                    int pointindex = (int)polys[v, e];
                    vertex[v] = new Vector2(points[0, pointindex], points[1, pointindex]);
                    int p1index = pointindex;
                    int p2index = 0;
                    if (v < vertexcount - 1)
                        p2index = (int)polys[v + 1, e];
                    else
                        p2index = (int)polys[0, e];
                    int p1 = p1index;
                    int p2 = p2index;
                    string facestr =Face.GetFaceStrId(p1index,p2index);
                    if (!facedic.ContainsKey(facestr))
                    {
                        Face face = new Face(new Vector2(points[0, p1], points[1, p1]), new Vector2(points[0, p2], points[1, p2]));
                        face.id = faceid;
                        face.pointid1=p1;
                        face.pointid2=p2;
                        face.owner = elem;
                        faceid++;
                        facedic.Add(facestr, face);
                        elem.faces[v] = face;
                    }
                    else
                    {
                        Face face = facedic[facestr];
                        face.nbelem = elem;
                        elem.faces[v] = face;
                        Element owner = face.owner;
                        elem.nbs[v] = owner;
                        owner.nbs[owner.FaceLocalId(face)] = elem;
                    }

                }
                elem.SetPolygon(vertex);
                elem.PreCalc();
            }

            faces = new Face[facedic.Count];
            bndfaces = new Face[bound.GetLength(1)];

            foreach (var pair in facedic)
            {
                Face face = pair.Value;
                faces[face.id] = face;
            }

            for (e = 0; e < elementcount; e++)
                elements[e].Calc();

            int f;
            for (f = 0; f < bound.GetLength(1); f++)
            {
                string facestr = Face.GetFaceStrId((int)bound[0, f], (int)bound[1, f]);
                Face face = facedic[facestr];
                face.bnddomain = (int)bound[5, f];
                face.bndgroup = (int)bound[4, f];
                face.isboundary = true;
                bndfaces[f]=face;
            }

            SetInitialBoundary();

            for (e = 0; e < elementcount; e++)
                elements[e].CalcFluxes();


            return true;

        }
        public double[,] LoadMatrix(string[] lines,ref int row)
        {
            double[,] m ;
            int rowstart = row;

            for (; ; )
            {
                string line = lines[row];
                if (line.StartsWith("##"))
                    break;
                if (line.Trim() == "")
                    break;
                row++;
            }
            int rowend = row-1;
            int cols = lines[rowstart].Split(' ').Length;
            m = new double[rowend-rowstart +1 , cols];
            int col = 0;
            for(int r=rowstart;r<=rowend;r++)
            {
                string line = lines[r];
                var vals = line.Split(' ');
                col = 0;
                foreach (string s in vals)
                {
                    m[r - rowstart, col] = Convert.ToDouble(s.Trim().Replace(".", ","));
                    col++;
                }
            }


            return m;
        }
        public bool LoadFromFile(string Inputfile, ref double[,] points, ref double[,] polys, ref double[,] bound)
        {
            var lines = File.ReadAllText(Inputfile+".out").Split('\n');
            if (lines[0].Replace(" ", "") != "##Point")
                return false;

            int row = 1;
            points = LoadMatrix(lines,ref row);
            row++;
            polys = LoadMatrix(lines, ref row);
            row++;
            bound = LoadMatrix(lines, ref row);
            for (int r = 0; r < polys.GetLength(0); r++)
                for (int c = 0; c < polys.GetLength(1); c++)
                {
                    polys[r, c] -= 1;
                }

            for (int r = 0; r < 2; r++)
                for (int c = 0; c < bound.GetLength(1); c++)
                {
                    bound[r, c] -= 1;
                }

            return true;
        }
        public void RunPhysic()
        {
            //return;
  //          if (count>0)
  //              return;
            count++;

            SetPeriodicBoundary();
            Calc();


        }



    }




}

