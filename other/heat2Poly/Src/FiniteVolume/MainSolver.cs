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
        int elementcount;
        public Element[] elements;
//        public Node[] nodes;
        public int nodecount;
        double delt;
        int count;

//        string Inputfile;

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
                
        }
        public void SetPeriodicBoundary()
        {
            Element elem;
        }
        public void SetParameters()
        {
            delt = 0.01;
        }
        public bool Create(string filename_)
        {
//            Inputfile = filename_;
//            if (LoadFromFile() != 0)
//                return false;

//            TestCalcUnstructuredFlux();
//            TestCalcUnstructuredFace();

            count = 0;
            SetParameters();
            int rowcount=12;
            elementcount = rowcount * 12;
            double h = 4;

            elements = new Element[elementcount];

            int e = 0;
            for (e = 0; e < elementcount ; e++)
            {
                elements[e]=new Element();
            }
            Element elem;

            e = 0;
            int n = 0;
            int row = 0;
            int total = elementcount / rowcount;
            for (e = row * total; e < (row+1) * total; e++)
            {
                elem = elements[e]; elem.id = e + 1; elem.k = 1;
                Vector2[] vertex = new Vector2[4];
                vertex[0] = new Vector2(h * n, h * row);
                vertex[1] = new Vector2(h * (n + 1), h * row);
                vertex[2] = new Vector2(h * (n + 1), h * (row + 1));
                vertex[3] = new Vector2(h * n, h * (row + 1));
                elem.SetPolygon(vertex);
                elem.nbs[0] = null;
                elem.nbs[2] = elements[e+total];
                if (n == 0)
                    elem.nbs[3] = null;
                else
                    elem.nbs[3] = elements[e-1];
                if (n == total-1)
                    elem.nbs[1] = null;
                else
                    elem.nbs[1] = elements[e + 1];

                elem.PreCalc();
                n++;
            }

            for (int r = 0; r < rowcount-2; r++)
            {

                n = 0;
                row++;
                for (e = row * total; e < (row + 1) * total; e++)
                {
                    elem = elements[e]; elem.id = e + 1; elem.k = 1;
                    elem.SetPolygon4(h * n, h * row, h * (n + 1), h * row, h * (n + 1), h * (row + 1), h * n, h * (row + 1));
                    elem.nbs[0] = elements[e - total];
                    elem.nbs[2] = elements[e + total];
                    if (e == row * total)
                        elem.nbs[3] = null;
                    else
                        elem.nbs[3] = elements[e - 1];
                    if (e == (row + 1) * total - 1)
                        elem.nbs[1] = null;
                    else
                        elem.nbs[1] = elements[e + 1];

                    elem.PreCalc();
                    n++;
                }
            }
            n = 0;
            row++;
            for (e = row * total; e < (row + 1) * total; e++)
            {
                elem = elements[e]; elem.id = e + 1; elem.k = 1;
                elem.SetPolygon4(h * n, h * row, h * (n + 1), h * row, h * (n + 1), h * (row + 1), h * n, h * (row + 1));
                elem.nbs[0] = elements[e - total];
                elem.nbs[2] = null;
                if (e == row * total)
                    elem.nbs[3] = null;
                else
                    elem.nbs[3] = elements[e - 1];
                if (e == (row + 1) * total - 1)
                    elem.nbs[1] = null;
                else
                    elem.nbs[1] = elements[e + 1];

                elem.PreCalc();
                n++;
            }

            for (e = 0; e < elementcount; e++)
                elements[e].Calc();
            for (e = 0; e < elementcount; e++)
                elements[e].CalcFaces();
            

            double bndw = 240;
            //down
            e = 0; elem = elements[e]; elem.SetBoundary(0, BoundaryType.BND_CONST, 0); elem.SetBoundary(3, BoundaryType.BND_CONST, bndw);
            for (int el = 0; el < total - 2; el++)
            {
                e++; elem = elements[e]; elem.SetBoundary(0, BoundaryType.BND_CONST, 0);
            }
            e++; elem = elements[e]; elem.SetBoundary(0, BoundaryType.BND_CONST, 0); elem.SetBoundary(1, BoundaryType.BND_CONST, 0);
            //--
            //middle
            for (int r = 0; r < rowcount - 2; r++)
            {

                e++; elem = elements[e]; elem.SetBoundary(3, BoundaryType.BND_CONST, bndw);
                for (int el = 0; el < total - 2; el++)
                    e++;
                e++; elem = elements[e]; elem.SetBoundary(1, BoundaryType.BND_CONST, 0);
            }
            //--
            //up
            e++; elem = elements[e]; elem.SetBoundary(2, BoundaryType.BND_CONST, 0); elem.SetBoundary(3, BoundaryType.BND_CONST, bndw);
            for (int el = 0; el < total - 2; el++)
            {
                e++; elem = elements[e]; elem.SetBoundary(2, BoundaryType.BND_CONST, 0);
            }
            e++; elem = elements[e]; elem.SetBoundary(2, BoundaryType.BND_CONST, 0); elem.SetBoundary(1, BoundaryType.BND_CONST, 0);

            
            SetInitialBoundary();

            for (e = 0; e < elementcount; e++)
                elements[e].CalcFluxes();

            return true;
        }
 
        public int LoadFromFile()
        {
            return 0;
        }
        public void RunPhysic()
        {
//            if (count>0)
//                return;
            count++;

            SetPeriodicBoundary();
            Calc();


        }



    }




}

