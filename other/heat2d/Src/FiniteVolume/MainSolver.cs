using System;
using System.IO;
//using REAL = System.Double;
using EVector = EgenLib.EMath.Vector3;
using Vector2 = EgenLib.EMath.Vector2;
using FiniteVolume2D;

namespace FiniteVolume2D
{

    public class MainSolver
    {

//        int elementcount;
        public Element[,] elements;
        public double GlobCoeff;
//        public double GlobRo;
//        public double GlobU;
        public double FaceArea;
        public double ElementVolume;

        double delt;
        double dh;
        int imax;
        int jmax;
        //        double xlength;
        int count;

        string Inputfile;
        string[] lines;
        string[] geolines;

//        double[,] F;
//        double Re;


        public MainSolver()
        {
        }
        public double GetStepSize()
        {
            return delt;
        }
        public void GetCellSize(ref double cellsizex, ref double cellsizey)
        {
            cellsizex = dh;
            cellsizey = dh;
        }
        public void GetDomainSize(ref int x, ref int y)
        {
            x = imax;
            y = jmax;
        }
        public void GetCellCoord(int i, int j, ref double cellx1, ref double celly1, ref double cellx2, ref double celly2)
        {
            cellx1 = i * dh;
            cellx2 = cellx1 + dh;
            celly1 = j * dh;
            celly2 = celly1 + dh;
        }
        public double GetU(int x, int y)
        {
            return elements[x,y].u;
        }

        // -------------------- COMPUTE ---------------------------------
        // -------------------- COMPUTE ---------------------------------


        public void Calc()
        {
            //            gradu += GlobCoeff * ((nb.u - elem.u) / nb.dh);
            //           gradu += -g * (SF) * (df / dx,df / dy);  graduCartesian+=-g*sf*df/dx
            //                                                       graduCartesian+=-g*faceWidth*((Un - Uc) / dxCN) 
            //            int iter = 0;
            //            for (iter = 0; iter < 8; iter++)
            int i, j;

            for (i = 1; i < imax-1; i++)
                for (j = 1; j < jmax-1; j++)
                {
                    Element c = elements[i,j];
                    Element e = elements[i+1, j];
                    Element w = elements[i-1, j];
                    Element n = elements[i, j+1];
                    Element s = elements[i, j-1];

                    double gradu = 0;
                    // calc gradient
                    gradu += GlobCoeff *FaceArea*((e.u - c.u) / e.cfdist);
                    gradu += GlobCoeff * FaceArea * ((w.u - c.u) / w.cfdist);
                    gradu += GlobCoeff * FaceArea * ((n.u - c.u) / n.cfdist);
                    gradu += GlobCoeff * FaceArea * ((s.u - c.u) / s.cfdist);
                    double coeff = delt;
                    c.u = c.u + (gradu) *  coeff;
                }

        }


        // -------------------- MAIN ---------------------------------
        // -------------------- MAIN ---------------------------------


        public void Destroy()
        {

        }
        public void SetBoundary()
        {
            int i, j;

            for (i = 0; i < imax; i++)
                for (j = 0; j < jmax; j++)
                {
                    if(i==0 || i==imax-1 || j==0 || j==jmax-1)
                      elements[i, j].cfdist=dh/2;
                }


            for (i = 0; i < imax; i++)
                {
                    Element e = elements[i, 0];
                    e.u = 250;
                }
        }
        public void SetParameters()
        {
            dh = 0.1;
            ElementVolume = dh * dh;
            FaceArea = dh;
            imax = 27;
            jmax = 27;
            GlobCoeff = 10;
            delt = 0.01;
        }
        public bool Create(string filename_)
        {
            Inputfile = filename_;
            if (LoadFromFile() != 0)
                return false;

            SetParameters();
            count = 0;

            elements = new Element[imax, jmax];
            int i, j;

            for (i = 0; i < imax; i++)
                for (j = 0; j < jmax; j++)
                {
                    Element elem = new Element();
                    elem.id = i*j+j;
                    elem.u = 0;
                    elem.cfdist = dh;
                    elements[i,j] = elem;
                }

            SetBoundary();
            return true;
        }
        public int LoadFromFile()
        {
            return 0;
        }
        public void RunPhysic()
        {
//            if (count>59)
//                return;
            count++;

//            SetBoundary();

            Calc();
        }



    }




}

/*
                    Element nbw = elem.nbs[0];
                    Element nbe = elem.nbs[1];

                    double gradw = 0;
                    double grade = 0;
                    if (nbw.boundaryType != Element.BoundaryType.BND_INSULATED)
                        gradw = ((elem.u - nbw.u) / nbw.dh);
                    if (nbe.boundaryType != Element.BoundaryType.BND_INSULATED)
                        grade = ((nbe.u - elem.u) / nbe.dh);
                    int nw = elem.facenormals[FaceType.WEST];
                    int ne = elem.facenormals[FaceType.EAST];

                    gradu+=(GlobCoeff * ne * grade + GlobCoeff * nw * gradw);
                }
                double coeff = (GlobRo * elem.dh) / delt;
                elem.u = elem.u + (gradu) / coeff;

*/