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
        public Node[] nodes;
        public int nodecount;
        double delt;
        int count;

//        string Inputfile;
//        string[] lines;
//        string[] geolines;

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
            double flux;
            double sumflux;
            double[] aa = new double[3];
            double[] bb = new double[3]; 

            int e;
            int iter = 0;
            for (iter = 0; iter < 225; iter++)
            {
                for (e = 0; e < elementcount; e++)
                {
                    Element elem = elements[e];
                    int nf;
                    bc = 0;
                    ac = 0;
                    sumflux = 0;
                    flux = 0;
                    aa[0] = 0;
                    aa[1] = 0;
                    aa[2] = 0;
                    bb[0] = 0;
                    bb[1] = 0;
                    bb[2] = 0;
                    for (nf = 0; nf < Element.FACEMAX; nf++)
                    {
                        Face face = elem.faces[nf];
                        Element nb = elem.nbs[nf];
                        //                    int nw = elem.facenormals[FaceType.WEST];
                        //                    int ne = elem.facenormals[FaceType.EAST];
                        //                               fluxV = 0;
                        if (face.isboundary)
                        {
                            if (face.boundaryType == BoundaryType.BND_CONST)
                            {
                                flux = face.k * (face.area / elem.nodedistances[nf]);
                                ac += flux;
                                bc += flux * face.bndu;
                                bb[nf] = flux;
                            }
                            else if (face.boundaryType == BoundaryType.BND_FLUX)
                            {
                                double sf = face.area;
                                flux = face.bndflux * sf;
                                bc += flux;
                                bb[nf] = flux;
                            }
                        }
                        else
                        {
                            flux = -face.k * (face.area / elem.nodedistances[nf]);
                            sumflux += flux * nb.u;
                            ac += -flux;
                            aa[nf] = -flux;
                        }

                    }
                    for (int ii = 0; ii < 3; ii++)
                    {
                        aa[ii] = aa[ii] / Math.Sqrt(3);
                        bb[ii] = bb[ii] / Math.Sqrt(3);
                    }
                    double acc = ac / Math.Sqrt(3);
                    double bcc = bc / Math.Sqrt(3);
                    elem.u = (bc - sumflux) / ac;

                }
            }

        }


        // -------------------- MAIN ---------------------------------
        // -------------------- MAIN ---------------------------------


        public void Destroy()
        {

        }
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
                
        }
        public void SetParameters()
        {
            elementcount = 9;
            delt = 2;
        }
        //by book
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
            double t500=500/2;
            double t200=200/2;
            double t400=400/2;
            double a = 0.02;
            double da=0.02/2.0;
            double h =(Math.Sqrt(3)/2.0)*a;
            Element elem;
            e = 0;elem = elements[e]; elem.id = e + 1; elem.k = 1;
            elem.SetTriangle(a, 0, a + da, h,da, h); 
            elem.nbs[0] = elements[1]; elem.nbs[1] = elements[7]; elem.nbs[2] = null;
            elem.PreCalc();             

            e++; elem = elements[e]; elem.id = e + 1; elem.k = 1;
            elem.SetTriangle(a, 0, 2 * a, 0, a + da, h); 
            elem.nbs[0] = null; elem.nbs[1] = elements[2]; elem.nbs[2] = elements[0];
            elem.PreCalc(); 

            e++; elem = elements[e]; elem.id = e + 1; elem.k = 1;
            elem.SetTriangle(2 * a, 0, 2*a + da, h,a+da, h);
            elem.nbs[0] = elements[3]; elem.nbs[1] = null; elem.nbs[2] = elements[1];
            elem.PreCalc(); 

            e++; elem = elements[e]; elem.id = e + 1; elem.k = 1;
            elem.SetTriangle(2 * a, 0,3*a,0 ,2*a + da, h);
            elem.nbs[0] = null; elem.nbs[1] = elements[4]; elem.nbs[2] = elements[2];
            elem.PreCalc();

            e++; elem = elements[e]; elem.id = e + 1; elem.k = 1;
            elem.SetTriangle(3 * a, 0,3*a+da,h ,2 * a + da, h );
            elem.nbs[0] = null; elem.nbs[1] = elements[5]; elem.nbs[2] = elements[3];
            elem.PreCalc(); 
            //---------------------
            e++; elem = elements[e]; elem.id = e + 1; elem.k = 1;
            elem.SetTriangle(2 * a + da, h, 3 * a + da, h, 3 * a ,2*h);
            elem.nbs[0] = elements[4]; elem.nbs[1] = elements[6]; elem.nbs[2] = null;
            elem.PreCalc();

            e++; elem = elements[e]; elem.id = e + 1; elem.k = 1;
            elem.SetTriangle(3 * a + da, h,4 * a, 2*h, 3 * a, 2 * h);
            elem.nbs[0] = null; elem.nbs[1] = null; elem.nbs[2] = elements[5];
            elem.PreCalc();

            e++; elem = elements[e]; elem.id = e + 1; elem.k = 1;
            elem.SetTriangle(da, h, a+da, h, a, 2 * h);
            elem.nbs[0] = elements[0]; elem.nbs[1] = null; elem.nbs[2] = elements[8];
            elem.PreCalc();

            e++; elem = elements[e]; elem.id = e + 1; elem.k = 1;
            elem.SetTriangle(da, h, a, 2 * h,0, 2 * h);
            elem.nbs[0] = elements[7]; elem.nbs[1] = null; elem.nbs[2] = null;
            elem.PreCalc();
            
            for (e = 0; e < elementcount; e++)
                elements[e].Calc();
            for (e = 0; e < elementcount; e++)
                elements[e].CalcFaces();
            //frombook
            e = 0; elem = elements[e]; elem.SetBoundary(2, BoundaryType.BND_CONST, t500);
            e++; elem = elements[e]; elem.SetBoundary(0, BoundaryType.BND_FLUX, 0);
            e++; elem = elements[e]; elem.SetBoundary(1, BoundaryType.BND_CONST, t400);
            e++; elem = elements[e]; elem.SetBoundary(0, BoundaryType.BND_FLUX, 0);
            e++; elem = elements[e]; elem.SetBoundary(0, BoundaryType.BND_CONST, t500);
            //--
            e++; elem = elements[e]; elem.SetBoundary(2, BoundaryType.BND_CONST, t200);
            e++; elem = elements[e]; elem.SetBoundary(0, BoundaryType.BND_CONST,t500); elem.SetBoundary(1, BoundaryType.BND_FLUX, 0);
            e++; elem = elements[e]; elem.SetBoundary(1, BoundaryType.BND_CONST, t200);
            e++; elem = elements[e]; elem.SetBoundary(1, BoundaryType.BND_FLUX, 0); elem.SetBoundary(2, BoundaryType.BND_CONST, t500);


            //1
            //e = 0; elem = elements[e]; elem.SetBoundary(2, BoundaryType.BND_CONST, t500);
            //e++; elem = elements[e]; elem.SetBoundary(0, BoundaryType.BND_CONST,t500);
            //e++; elem = elements[e]; elem.SetBoundary(1, BoundaryType.BND_CONST, t400);
            //e++; elem = elements[e]; elem.SetBoundary(0, BoundaryType.BND_CONST, t500);
            //e++; elem = elements[e]; elem.SetBoundary(0, BoundaryType.BND_CONST, t500);
            ////--
            //e++; elem = elements[e]; elem.SetBoundary(2, BoundaryType.BND_CONST, t200);
            //e++; elem = elements[e]; elem.SetBoundary(0, BoundaryType.BND_CONST, t500); elem.SetBoundary(1, BoundaryType.BND_FLUX, 0);
            //e++; elem = elements[e]; elem.SetBoundary(1, BoundaryType.BND_CONST, t200);
            //e++; elem = elements[e]; elem.SetBoundary(1, BoundaryType.BND_FLUX, 0); elem.SetBoundary(2, BoundaryType.BND_CONST, t500);

            //2
            //e = 0; elem = elements[e]; elem.SetBoundary(2, BoundaryType.BND_CONST, t500);
            //e++; elem = elements[e]; elem.SetBoundary(0, BoundaryType.BND_CONST, t500);
            //e++; elem = elements[e]; elem.SetBoundary(1, BoundaryType.BND_CONST, t200);
            //e++; elem = elements[e]; elem.SetBoundary(0, BoundaryType.BND_CONST, t200);
            //e++; elem = elements[e]; elem.SetBoundary(0, BoundaryType.BND_CONST, t200);
            ////--
            //e++; elem = elements[e]; elem.SetBoundary(2, BoundaryType.BND_CONST, t200);
            //e++; elem = elements[e]; elem.SetBoundary(0, BoundaryType.BND_CONST, t200); elem.SetBoundary(1, BoundaryType.BND_FLUX, 0);
            //e++; elem = elements[e]; elem.SetBoundary(1, BoundaryType.BND_CONST, t200);
            //e++; elem = elements[e]; elem.SetBoundary(1, BoundaryType.BND_FLUX, 0); elem.SetBoundary(2, BoundaryType.BND_CONST, t500);

            nodecount = 0;
            nodes = new Node[elementcount*3];
            int n = 0;
            int v = 0;
            for (e = 0; e < elementcount; e++)
            {
                elem = elements[e];
                for (v = 0; v < elem.vertex.Length; v++)
                {
                    bool found=false;
                    for (n = 0; n < nodecount; n++)
                    {
                        if (nodes[n].vertex.Equal(elem.vertex[v]))
                        {
                            if (n == 0)
                            {
                                int abb = 1;
                            }
                            nodes[n].elements[nodes[n].elemcount] = elem;
                            nodes[n].elemcount++;
                            found=true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        nodes[nodecount] = new Node();
                        nodes[nodecount].id = nodecount + 1;
                        nodes[nodecount].vertex = elem.vertex[v];
                        nodes[nodecount].elements[0] = elem;
                        nodes[nodecount].elemcount++;
                        nodecount++;
                    }
                }
            }

            int n2=0;
            for (n = 0; n < nodecount; n++)
            {
                for (e = 0; e < nodes[n].elemcount; e++)
                {
                    elem = nodes[n].elements[e];
                    bool found= false;
                    for (n2 = 0; n2 < Element.FACEMAX; n2++)
                    {
                        if (elem.nodes[n2] == nodes[n])
                        {
                            found = true;
                            break;
                        }
                    }
                    if(!found)
                    {
                        elem.nodes[elem.nodecount] = nodes[n];
                        elem.nodecount++;
                    }

                }
            }

//            SetInitialBoundary();
            return true;
        }
 
        public int LoadFromFile()
        {
            return 0;
        }
        public void CalcPostProcessing()
        {
            int n = 0;
            int e = 0;

            for (n = 0; n < nodecount; n++)
            {
                Node node = nodes[n];
                node.count = 0;
                node.u = 0;
                for (e = 0; e < node.elemcount; e++)
                {
                    node.u += node.elements[e].u;
                    node.count++;
                }
                node.u = node.u / node.count;
            }
        }
        public void RunPhysic()
        {
            if (count>0)
                return;
            count++;
          //  SetPeriodicBoundary();

            Calc();
            CalcPostProcessing();


        }



    }




}

