using System;
using System.IO;
//using REAL = System.Double;
using EVector = EgenLib.EMath.Vector3;
using Vector2 = EgenLib.EMath.Vector2;
using FiniteVolume2D;

namespace FiniteVolume2D
{
    /*
     * элемент : грани вершины узлы  объем центроид искомые
     * грань : вершины центроид площадь нормаль(с owner) интерпол€цвес
     * 
     */

    public class MainSolver
    {

        int elementcount;
        public Element[] elements;
//        public double GlobCoeff;
//        public double GlobRo;
//        public double GlobU;
//        public double FaceArea;

        double delt;
//        double xlength;
        int count;

//        string Inputfile;
//        string[] lines;
//        string[] geolines;

//        double[,] F;
//        double Re;


        public MainSolver()
        {
        }
        public double GetStepSize()
        {
            return delt;
        }
        /*
        public void GetCellSize(ref double cellsizex, ref double cellsizey)
        {
            double dh = (xlength / (elements.Length - 2));
            cellsizex = dh;
            cellsizey = dh;
        }
        public void GetDomainSize(ref int x, ref int y)
        {
            x = elementcount;
            y = 1;
        }
        public void GetCellCoord(int i, int j, ref double cellx1, ref double celly1, ref double cellx2, ref double celly2)
        {
            double dh = (xlength / (elementcount - 2));
            cellx1 = i * dh;
            cellx2 = cellx1 + dh;
            celly1 = 0 * dh;
            celly2 = celly1 + dh+dh;
        }
        public double GetU(int e)
        {
            return elements[e].u;
        }
         */

        // -------------------- COMPUTE ---------------------------------
        // -------------------- COMPUTE ---------------------------------


        public void Calc()
        {
           
            double bc,ac,ae,aw,an,aso;
            double ce, cw, cn, cso;
            double fluxVe, fluxVw, fluxVn, fluxVs;
            double fluxV;
            int e;
//                       int iter = 0;
//                       for (iter = 0; iter < 1066; iter++)
                       {
                           //                int nf;
                           //                for (nf = 0; nf < Element.FACEMAX; nf++)
                           //                {
                           for (e = 0; e < elementcount; e++)
                           {
                               Element elem = elements[e];
                               Face facee = elem.faces[Side.EAST];
                               Face facew = elem.faces[Side.WEST];
                               Face facen = elem.faces[Side.NORTH];
                               Face faces = elem.faces[Side.SOUTH];

                               Element ne = elem.nbs[Side.EAST];
                               Element nw = elem.nbs[Side.WEST];
                               Element nn = elem.nbs[Side.NORTH];
                               Element ns = elem.nbs[Side.SOUTH];
                               //                    int nw = elem.facenormals[FaceType.WEST];
                               //                    int ne = elem.facenormals[FaceType.EAST];
                               fluxV = 0;
                               bc = 0;
                               ac = 0; ae = 0; aw = 0; an = 0; aso = 0;
                               ce = 0; cw = 0; cn = 0; cso = 0;
                               if (facee.isboundary)
                               {
                                   if (facee.boundaryType == BoundaryType.BND_CONST)
                                   {
                                       ce = facee.k * (facee.area / elem.nodedistances[Side.EAST]);
                                       fluxVe = -ce * facee.bndu;
                                       fluxV += fluxVe;
                                   }
                                   else if (facee.boundaryType == BoundaryType.BND_FLUX)
                                   {
                                       double sf = -facee.area;
                                       fluxVe = facee.bndflux * sf;
                                       fluxV += fluxVe;
                                   }
                               }
                               else
                               {
                                   ae = -facee.k * (facee.area / elem.nodedistances[Side.EAST]);
                                   ce = -ae;
                               }
                               if (facen.isboundary)
                               {
                                   if (facen.boundaryType == BoundaryType.BND_CONST)
                                   {
                                       cn = facen.k * (facen.area / elem.nodedistances[Side.NORTH]);
                                       fluxVn = -cn * facen.bndu;
                                       fluxV += fluxVn;
                                   }
                                   else if (facen.boundaryType == BoundaryType.BND_FLUX)
                                   {
                                       double sf = -facen.area;
                                       fluxVn = facen.bndflux * sf;
                                       fluxV += fluxVn;
                                   }
                                   else if (facen.boundaryType == BoundaryType.BND_SURROUNDCONVECT)
                                   {
                                       double h = facen.htranscoeff;
                                       double T = facen.bndu;

                                       cn = ((h * (facen.k / elem.nodedistances[Side.NORTH])) / (h + (facen.k / elem.nodedistances[Side.NORTH]))) * facen.area;
                                       fluxVn = -cn * T;
                                       fluxV += fluxVn;
                                   }
                               }
                               else
                               {
                                   an = -facen.k * (facen.area / elem.nodedistances[Side.NORTH]);
                                   cn = -an;
                               }
                               if (facew.isboundary)
                               {
                                   if (facew.boundaryType == BoundaryType.BND_CONST)
                                   {
                                       cw = facew.k * (facew.area / elem.nodedistances[Side.WEST]);
                                       fluxVw = -cw * facew.bndu;
                                       fluxV += fluxVw;
                                   }
                                   else if (facew.boundaryType == BoundaryType.BND_FLUX)
                                   {
                                       double sf = -facew.area;
                                       fluxVw = facew.bndflux * sf;
                                       fluxV += fluxVw;
                                   }
                               }
                               else
                               {
                                   aw = -facew.k * (facew.area / elem.nodedistances[Side.WEST]);
                                   cw = -aw;
                               }
                               if (faces.isboundary)
                               {
                                   if (faces.boundaryType == BoundaryType.BND_CONST)
                                   {
                                       cso = faces.k * (faces.area / elem.nodedistances[Side.SOUTH]);
                                       fluxVs = -cso * faces.bndu;
                                       fluxV += fluxVs;
                                   }
                                   else if (faces.boundaryType == BoundaryType.BND_FLUX)
                                   {
                                       //flux=qflux*surfacenormal
                                       double sf = -faces.area;
                                       fluxVs = faces.bndflux * sf;
                                       fluxV += fluxVs;
                                   }
                               }
                               else
                               {
                                   aso = -faces.k * (faces.area / elem.nodedistances[Side.SOUTH]);
                                   cso = -aso;
                               }

                               double Ue = 0;
                               double Uw = 0;
                               double Un = 0;
                               double Us = 0;
                               if (ne != null)
                                   Ue = ne.u;
                               if (nw != null)
                                   Uw = nw.u;
                               if (nn != null)
                                   Un = nn.u;
                               if (ns != null)
                                   Us = ns.u;

                               ac = ce + cw + cn + cso;
                               bc = -fluxV;
                               elem.u = (bc - ae * Ue - an * Un - aw * Uw - aso * Us) / ac;
                               //                    ac*elem.id+ae*ne.id+an*nn.id=bc;


                           }

                       }

        }


        // -------------------- MAIN ---------------------------------
        // -------------------- MAIN ---------------------------------


        public void Destroy()
        {

        }
/* 
  //example1
        public void SetInitialBoundary()
        {
            int e;
            Element elem;
            for (e = 0; e < elementcount; e++)
            {
                elem = elements[e];
                elem.u = 0;
            }
            elem = elements[0]; elem.SetBoundary(Side.SOUTH, BoundaryType.BND_CONST,150);
            elem = elements[1]; elem.SetBoundary(Side.SOUTH, BoundaryType.BND_CONST, 150);
            elem = elements[2]; elem.SetBoundary(Side.SOUTH, BoundaryType.BND_CONST, 150);

            elem = elements[0]; elem.SetBoundary(Side.WEST, BoundaryType.BND_CONST, 240);
            elem = elements[3]; elem.SetBoundary(Side.WEST, BoundaryType.BND_CONST, 240);
            elem = elements[6]; elem.SetBoundary(Side.WEST, BoundaryType.BND_CONST, 240);

            elem = elements[2]; elem.SetBoundary(Side.EAST, BoundaryType.BND_CONST, 0);
            elem = elements[5]; elem.SetBoundary(Side.EAST, BoundaryType.BND_CONST, 0);
            elem = elements[8]; elem.SetBoundary(Side.EAST, BoundaryType.BND_CONST, 0);

            elem = elements[6]; elem.SetBoundary(Side.NORTH, BoundaryType.BND_CONST, 150);
            elem = elements[7]; elem.SetBoundary(Side.NORTH, BoundaryType.BND_CONST, 150);
            elem = elements[8]; elem.SetBoundary(Side.NORTH, BoundaryType.BND_CONST, 150);
                
        }
        public void SetParameters()
        {
            elementcount = 9;
            delt = 2;
        }
        public bool Create(string filename_)
        {
//            Inputfile = filename_;
//            if (LoadFromFile() != 0)
//                return false;

            count = 0;
            SetParameters();

            elements = new Element[elementcount];
            int e = 0;
            for (e = 0; e < elementcount ; e++)
            {
                elements[e]=new Element();
            }

            e = 0;
            Element elem = elements[e];elem.id = e+1;elem.k = 1;
            elem.SetRectangle(0, 0, 0.1, 0.1); elem.PreCalc();
            e++;elem = elements[e];elem.id = e+1;elem.k = 1;
            elem.SetRectangle(0.1, 0, 0.2, 0.1); elem.PreCalc();
            e++;elem = elements[e];elem.id = e+1;elem.k = 1;
            elem.SetRectangle(0.3, 0, 0.3, 0.1); elem.PreCalc();

            e++;elem = elements[e];elem.id = e+1;elem.k = 1;
            elem.SetRectangle(0, 0.1, 0.1, 0.1); elem.PreCalc();
            e++;elem = elements[e];elem.id = e+1;elem.k = 1;
            elem.SetRectangle(0.1, 0.1, 0.2, 0.1); elem.PreCalc();
            e++;elem = elements[e];elem.id = e+1;elem.k = 1;
            elem.SetRectangle(0.3, 0.1, 0.3, 0.1); elem.PreCalc();

            e++;elem = elements[e];elem.id = e+1;elem.k = 1;
            elem.SetRectangle(0, 0.2, 0.1, 0.1); elem.PreCalc();
            e++;elem = elements[e];elem.id = e+1;elem.k = 1;
            elem.SetRectangle(0.1, 0.2, 0.2, 0.1); elem.PreCalc();
            e++;elem = elements[e];elem.id = e+1;elem.k = 1;
            elem.SetRectangle(0.3, 0.2, 0.3, 0.1); elem.PreCalc();
//            double gf1 = FVMath2D.FaceWeightFactorSimple(elements[0], elements[1], elements[0].faces[1]);
//            double gf2 = FVMath2D.FaceWeightFactor(elements[0], elements[1], elements[0].faces[1]);

            e=0;elem = elements[e];
            elem.nbs[Side.EAST] = elements[1];elem.nbs[Side.WEST] = null;
            elem.nbs[Side.NORTH] = elements[3];elem.nbs[Side.SOUTH] = null;
            e++; elem = elements[e];
            elem.nbs[Side.EAST] = elements[2]; elem.nbs[Side.WEST] = elements[0];
            elem.nbs[Side.NORTH] = elements[4]; elem.nbs[Side.SOUTH] = null;
            e++; elem = elements[e];
            elem.nbs[Side.EAST] =null; elem.nbs[Side.WEST] = elements[1];
            elem.nbs[Side.NORTH] = elements[5]; elem.nbs[Side.SOUTH] = null;

            e++; elem = elements[e];
            elem.nbs[Side.EAST] = elements[4]; elem.nbs[Side.WEST] = null;
            elem.nbs[Side.NORTH] = elements[6]; elem.nbs[Side.SOUTH] = elements[0];
            e++; elem = elements[e];
            elem.nbs[Side.EAST] = elements[5]; elem.nbs[Side.WEST] = elements[3];
            elem.nbs[Side.NORTH] = elements[7]; elem.nbs[Side.SOUTH] = elements[1];
            e++; elem = elements[e];
            elem.nbs[Side.EAST] = null; elem.nbs[Side.WEST] = elements[4];
            elem.nbs[Side.NORTH] = elements[8]; elem.nbs[Side.SOUTH] = elements[2];

            e++; elem = elements[e];
            elem.nbs[Side.EAST] = elements[7]; elem.nbs[Side.WEST] = null;
            elem.nbs[Side.NORTH] = null; elem.nbs[Side.SOUTH] = elements[3];
            e++; elem = elements[e];
            elem.nbs[Side.EAST] = elements[8]; elem.nbs[Side.WEST] = elements[6];
            elem.nbs[Side.NORTH] = null; elem.nbs[Side.SOUTH] = elements[4];
            e++; elem = elements[e];
            elem.nbs[Side.EAST] = null; elem.nbs[Side.WEST] = elements[7];
            elem.nbs[Side.NORTH] = null; elem.nbs[Side.SOUTH] = elements[5];


            for (e = 0; e < elementcount; e++)
            {
                elements[e].Calc();
            }

            SetInitialBoundary();
            return true;
        }
 */ 

// example2
        public void SetInitialBoundary()
        {
            int e;
            Element elem;
            for (e = 0; e < elementcount; e++)
            {
                elem = elements[e];
                elem.u = 0;
            }
            elem = elements[0]; elem.SetBoundary(Side.WEST, BoundaryType.BND_CONST,240);
            elem = elements[elements.Length-1]; elem.SetBoundary(Side.EAST, BoundaryType.BND_CONST, 0);

            for (e = 0; e < elementcount; e++)
            {
                elem = elements[e]; elem.SetBoundary(Side.SOUTH, BoundaryType.BND_FLUX, 0);
                elem = elements[e]; elem.SetBoundary(Side.NORTH, BoundaryType.BND_FLUX, 0);
            }
                
        }
        public void SetParameters()
        {
            elementcount = 24;
            delt = 2;
        }
        public bool Create(string filename_)
        {
//            Inputfile = filename_;
//            if (LoadFromFile() != 0)
//                return false;

            count = 0;
            SetParameters();

            elements = new Element[elementcount];
            int e = 0;
            for (e = 0; e < elementcount ; e++)
            {
                elements[e]=new Element();
            }
            Element elem;
            e = 0;
            for (e = 0; e < elementcount; e++)
            {
                elem = elements[e]; elem.id = e + 1; elem.k = 1;
                elem.SetRectangle(0.1*e, 0, 0.1, 0.1); elem.PreCalc();
                /*
                            e++;elem = elements[e];elem.id = e+1;elem.k = 1;
                            elem.SetRectangle(0.1, 0, 0.1, 0.1); elem.PreCalc();
                            e++;elem = elements[e];elem.id = e+1;elem.k = 1;
                            elem.SetRectangle(0.2, 0, 0.1, 0.1); elem.PreCalc();
                            e++;elem = elements[e];elem.id = e+1;elem.k = 1;
                            elem.SetRectangle(0.3, 0, 0.1, 0.1); elem.PreCalc();
                */
                elem.nbs[Side.NORTH] = null; elem.nbs[Side.SOUTH] = null;
            }

            elem = elements[0];
            elem.nbs[Side.EAST] = elements[1];elem.nbs[Side.WEST] = null;

            elem = elements[elementcount-1];
            elem.nbs[Side.EAST] = null; elem.nbs[Side.WEST] = elements[elementcount-2];

            for (e = 1; e < elementcount-1; e++)
            {
                elem = elements[e];
                elem.nbs[Side.EAST] = elements[e+1]; elem.nbs[Side.WEST] = elements[e-1];
            }

            

            for (e = 0; e < elementcount; e++)
            {
                elements[e].Calc();
            }

            SetInitialBoundary();
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

          //  SetPeriodicBoundary();

            Calc();
        }



    }




}

