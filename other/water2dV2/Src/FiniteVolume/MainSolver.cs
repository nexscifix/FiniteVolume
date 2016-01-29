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
        public double Re;
        public double ElementVolume;
        double GX;
        double GY;

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
        public double GetV(int x, int y)
        {
            return elements[x, y].v;
        }
        public double GetP(int x, int y)
        {
            return elements[x, y].p;
        }
        public bool IsBoundary(int x, int y)
        {
            return elements[x, y].boundaryType!=BoundaryType.BND_NO;
        }

        // -------------------- COMPUTE ---------------------------------
        // -------------------- COMPUTE ---------------------------------


        public void Calc()
        {
            /*
             f=u+dt*( (1/Re)*diffusionU - convectionU + gx) 
             g=v+dt*( (1/Re)*diffusionV - convectionV + gy) 

             d2p/dx2+d2p/dy2=(1/dt)*(df/dx+dg/dy)
             
             u=f-dt*dp/dx 
             v=g-dt*dp/dy 
              
             d2p/dx2+d2p/dy2=(1/dt)*(df/dx+dg/dy)
             2*(FaceArea*(pe-pc)/cf + FaceArea*(pw-pc)/cf + FaceArea*(pn-pc)/cf + FaceArea*(ps-pc)/cf)= (1/dt)*(FaceArea*(FC+FE)/2+FaceArea*(FC+FW)/2+FaceArea*(GC+GN)/2+FaceArea*(GC+GS)/2 )
             2*(pe/cfe-pc/cfe+pw/cfw-pc/cfw+pn/cfn-pc/cfn+ps/cfs-pc/cfs)= (1/dt)((FC+FE)/2+(FC+FW)/2+(GC+GN)/2+(GC+GS)/2)
             -pc/cfs-pc/cfn-pc/cfw-pc/cfe =( (1/dt)((FC+FE)/2+(FC+FW)/2+(GC+GN)/2+(GC+GS)/2 )/2 - (pe/cfe+pw/cfw+pn/cfn+ps/cfs)
             -pc(1/cfs+1/cfn+1/cfw+1/cfe) =( (1/dt)((FC+FE)/2+(FC+FW)/2+(GC+GN)/2+(GC+GS)/2) )/2 - (pe/cfe+pw/cfw+pn/cfn+ps/cfs)
             pc = (  (pe/cfe+pw/cfw+pn/cfn+ps/cfs) - (  (1/dt)((FC+FE)/2+(FC+FW)/2+(GC+GN)/2+(GC+GS)/2) )/2   ) /   (1/cfs+1/cfn+1/cfw+1/cfe)
              
              
//             (pe-2pc+pw)/dx*dx+(pn-2pc+ps)/dy*dy=(1/dt)*((fc-fw)/dx+(fc-fs)/dy)
//             (PE-PW)/2dx*volume+(PN-PS)/2dy*volume=(1/dt)*((FE-FW)/2dx*volume +(FN-FS)/2dy*volume ) 
              
              
             u=f-dt*dp/dx 
             v=g-dt*dp/dy 
              
             dp/dx=FaceArea*( (PC+PE)/2+(PC+PW)/2 )
              
             */

            //            int iter = 0;
            //            for (iter = 0; iter < 8; iter++)


            int i, j;
            double gradu = 0;
            double gradv = 0;
            for (i = 0; i < imax; i++)
                for (j = 0; j < jmax; j++)
                {
                    Element c = elements[i, j];
                    c.f = c.u;
                    c.g = c.v;
                }


            for (i = 1; i < imax-1; i++)
                for (j = 1; j < jmax-1; j++)
                {
                    Element c = elements[i,j];
                    Element e = elements[i+1, j]; Element w = elements[i-1, j];
                    Element n = elements[i, j+1]; Element s = elements[i, j-1];
                    gradu= FaceArea*( (e.u - c.u) / e.cfdist + (w.u - c.u) / w.cfdist + (n.u - c.u) / n.cfdist + (s.u - c.u) / s.cfdist);
                    c.f = c.u + delt*( (1.0/Re)*gradu + GX) ;
                    gradv = FaceArea * ((e.v - c.v) / e.cfdist + (w.v - c.v) / w.cfdist + (n.v - c.v) / n.cfdist + (s.v - c.v) / s.cfdist);
                    c.g = c.v + delt*( (1.0/Re)*gradv + GY) ;
                }

            //  copy values at external boundary
            for (i = 1; i < imax - 1; i++)
            {
                elements[i, 0].p = elements[i, 1].p;
                elements[i, jmax - 1].p = elements[i,jmax - 2].p;
            }
            for (j = 1; j < jmax; j++)
            {
                elements[0, j].p = elements[1,j].p;
                elements[imax - 1, j].p = elements[imax - 2,j].p;
            }

            for (i = 1; i < imax - 1; i++)
                for (j = 1; j < jmax - 1; j++)
                {
                    Element c = elements[i, j];
                    Element e = elements[i + 1, j]; Element w = elements[i - 1, j];
                    Element n = elements[i, j + 1]; Element s = elements[i, j - 1];
//                    gradu = FaceArea * ((e.u - c.u) / e.cfdist + (w.u - c.u) / w.cfdist + (n.u - c.u) / n.cfdist + (s.u - c.u) / s.cfdist);
//                    c.f = c.u + ( (1.0/Re)gradu + GX) * delt;
                    double fe = (c.f + e.f) / 2;
                    double fw = (c.f + w.f) / 2;
                    double gn = (c.g + n.g) / 2;
                    double gs = (c.g + s.g) / 2;
                    if (i == 1)
                        fw = w.f;
                    if (i == imax-2)
                        fe = e.f;
                    if (j == 1)
                        gs = s.g;
                    if (j == jmax - 2)
                        gn = n.g;
                    /*
                    */
               //     c.p = ((1.0 / delt) * ((e.f - w.f) / 2 + (n.g - s.g) / 2) - e.p / dh + w.p / dh + n.p / dh + s.p / dh) / (-1.0 / dh - 1.0 / dh - 1.0 / dh - 1.0 / dh);

                    c.p = ((e.p / e.cfdist + w.p / w.cfdist + n.p / n.cfdist + s.p / s.cfdist) - ( (1.0 / delt) * (fe - fw + gn - gs))/2 ) / (1 / s.cfdist + 1 / n.cfdist + 1 / w.cfdist + 1 / e.cfdist);
                }
            for (i = 1; i < imax - 1; i++)
                for (j = 1; j < jmax - 1; j++)
                {
                    Element c = elements[i, j];
                    Element e = elements[i + 1, j]; Element w = elements[i - 1, j];
                    Element n = elements[i, j + 1]; Element s = elements[i, j - 1];
                    double pe = (c.p + e.p) / 2;
                    double pw = (c.p + w.p) / 2;
                    double pn = (c.p + n.p) / 2;
                    double ps = (c.p + s.p) / 2;
                    if (i == 1)
                        pw = w.p;
                    if (i == imax - 2)
                        pe = e.p;
                    if (j == 1)
                        ps = s.p;
                    if (j == jmax - 2)
                        pn = n.p;

                    c.u = c.f - delt *FaceArea*( pe - pw);
                    c.v = c.g - delt * FaceArea * (pn - ps);
                    /*                    
                                        c.u = c.f - delt * FaceArea * ((e.p - w.p) / dh);
                                        c.v = c.g - delt * FaceArea * ((n.p - s.p) / dh);
                                        */

                }

   //         p[i][j] = ((1.0 / dt) * ((fe - fw) / 2 + (gn - gs) / 2) - pe / dx + pw / dx + pn / dy + ps / dy) / (-1.0 / dx - 1.0 / dx - 1.0 / dy - 1.0 / dy);
//            u[i][j] = f[i][j] - dt * A * ((pe - pw) / dx);
//            v[i][j] = g[i][j] - dt * A * ((pn - ps) / dy);


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
                    if (i == 0 || i == imax - 1 || j == 0 || j == jmax - 1)
                    {
                        elements[i, j].cfdist = dh / 2;
                        elements[i, j].boundaryType = BoundaryType.BND_CONST;
                    }
                }


            for (i = 0; i < imax; i++)
                {
                    Element e = elements[i, 0];
                    e.u = 10;
                    //e = elements[i,jmax-1];
                    //e.u = 10;
                }
        }
        public void SetParameters()
        {
            dh = 0.1;
//            dh = 1;
            ElementVolume = dh * dh;
            FaceArea = dh;
            imax = 20;
            jmax = 20;
            GX=0;
            GY=0;
            Re = 30;
            GlobCoeff = 0;// 1 / Re;
            delt = 0.05;
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
                    elem.v = 0;
                    elem.f = 0;
                    elem.g = 0;
                    elem.p = 0;
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