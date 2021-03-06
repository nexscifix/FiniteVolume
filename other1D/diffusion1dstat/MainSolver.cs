using System;
using System.IO;
using REAL = System.Double;

/*     -------------------   
       |     |     |     |
     -----W--w--P--e--E---- 
       | i-1 |  i  | i+1 |
       ------------------- 
       cells(finite volumes)  
  
    Gcoef*Acoef*(dF/dx)e - Gcoef*Acoef*(dF/dx)w + S*V = 0
  
    Gcoef*Acoef* (FE-FP)/dx - Gcoef*Acoef* (FP-FW)/dx + S*V = 0

    Gcoef*Acoef* (FE-FP)/dx - Gcoef*Acoef* (FP-FW)/dx + S*V = 0
    C*FE-C*FP-C*FP-C*(-FW) + SV=0
    -C*FP-C*FP + C*FE-C*(-FW) + SV=0
    C*FP+C*FP = C*FE-C*(-FW) + SV
    FP= (C*FE-C*(-FW) + SV) / (C+C)
   
    Acoef - face area
    dx - distance between nodes
    S - average value of source in cell
    V - cell volume =Acoef*dx
    e,w - faces
    E,W - nodes with unknown
    P - current cell node 

*/

namespace fluidedotnet
{

public class Face
{
//    int ownerid;
//    int nbid;
    public double Area;
    public double nodesdst;
}

public class Element
{
    public enum BoundaryType
    {
        BND_NO = 0,
        BND_CONST =1
    };
    public const int FACEMAX = 2;

    public int[] nbs = new int[FACEMAX];
    public Face[] faces = new Face[FACEMAX];
//    int[] facenormals = new int[4];
    public double u;
    public double SourceCoeff;
    public double FluxCoeff;
    public double dh;
    public double volume;
    public BoundaryType boundaryType;


    //  msh_vector normal;
//  msh_vector celement;
//  int nbnodes;
//  int node[8];
};


    public class MainSolver
    {

//        int nodecount;
//        int facecount;
//        int elementcount;
        public Element[] elements;
        public double GlobCoeff;
        public double FaceArea;

//        msh_face* faces;
//        msh_face* boundaryfaces;


        /*
        public enum CellType
        {
            CELL_FLUID = 1,
        };
        public struct Particle
        {
            public REAL x;
        };
        public Particle[] particles;
        public int particlecount;
        */
        REAL delt;
        REAL xlength;
        int count;

        string Inputfile;
        string[] lines;
        string[] geolines;

        REAL[,] F;
        double Re;


        public MainSolver()
        {
        }
        public double GetStepSize()
        {
            return delt;
        }
        public void GetCellSize(ref double cellsizex, ref double cellsizey)
        {
            double dh = (xlength / (elements.Length - 2));
            cellsizex = dh;
            cellsizey = dh;
        }
        public void GetDomainSize(ref int x, ref int y)
        {
            x = elements.Length;
            y = 1;
        }
        public void GetCellCoord(int i, int j, ref double cellx1, ref double celly1, ref double cellx2, ref double celly2)
        {
            double dh = (xlength / (elements.Length-2));
            cellx1 = i * dh;
            cellx2 = cellx1 + dh;
            celly1 = 0 * dh;
            celly2 = celly1 + dh+dh;
        }
        public double GetU(int x, int y)
        {
            return elements[x].u;
        }

        // -------------------- COMPUTE ---------------------------------
        // -------------------- COMPUTE ---------------------------------




        // -------------------- MAIN ---------------------------------
        // -------------------- MAIN ---------------------------------


        public void Destroy()
        {

        }
/*
        public void SetBoundary()
        {//ex 4.1
            elements[0].u = 100;
            elements[elements.Length - 1].u = 500;
        }
        public void SetParameters()
        {
            xlength = 0.5;
            GlobCoeff = 1000;
            FaceArea = 0.01;
        }
 */
        public void SetBoundary()
        {////ex 4.2
            //uniform source 
            double q = 1000*1000;
            elements[0].u = 100;
            elements[elements.Length - 1].u = 200;

            int e;
            int elemnum = elements.Length;
            for (e = 1; e < elemnum-1; e++)
            {
                elements[e].SourceCoeff = q * elements[e].volume;
            }

        }
        public void SetParameters()
        {
            xlength = 0.02;
            GlobCoeff = 0.5;
            FaceArea = 1;
        }
        public bool Create(string filename_)
        {
            Inputfile = filename_;
            if (LoadFromFile() != 0)
                return false;
//            U = new REAL[imax ,jmax];

            SetParameters();
            count = 0;

            elements = new Element[7];
            int e;
            int elemnum = elements.Length;
            for (e = 0; e < elemnum; e++)
            {
                elements[e] = new Element();
            }

            //int elementcount = elements.Length;
            for (e = 1; e < elemnum-1; e++)
            {
                Element elem = elements[e];
                elem.dh = xlength / (elemnum-2);
                elem.volume = elem.dh * FaceArea;
                elem.u = 0;
                //                if (elem.boundaryType == Element.BoundaryType.BND_NO)

                int nf;
                for (nf = 0; nf < Element.FACEMAX; nf++)
                {
                    elem.faces[nf] = new Face();
                    elem.faces[nf].Area = FaceArea;
                }
                //if(e==0)
                //{
                //  elem.nbs[0] = -1;
                //  elem.faces[0].nodesdst=elem.dh/2;
                //}
                //else
                //{
                  elem.nbs[0] =e-1;
                  elem.faces[0].nodesdst=elem.dh;
                //}
                //if (e == elemnum-1)
                //{
                //    elem.nbs[1] = -1;
                //    elem.faces[1].nodesdst=elem.dh/2;
                //}
                //else
               //{
                    elem.nbs[1] = e + 1;
                    elem.faces[1].nodesdst=elem.dh;
                //}
                    elem.FluxCoeff = (GlobCoeff * elem.faces[0].Area) / elem.dh;
            }

            double dh2 = (xlength / (elemnum - 2)) / 2;
            elements[0].boundaryType = Element.BoundaryType.BND_CONST;
            elements[0].FluxCoeff = (GlobCoeff * FaceArea) / dh2;

            elements[elements.Length - 1].boundaryType = Element.BoundaryType.BND_CONST;
            elements[elements.Length - 1].FluxCoeff = (GlobCoeff * FaceArea) / dh2;

            SetBoundary();



            return true;
        }
        public int LoadFromFile()
        {
            return 0;
        }
        public void Calc()
        {
            //        u[i] =(aw*tw + ae*te) / (aw+ae);
            //    FP= (C*FE-C*(-FW) + SV) / (C+C)
            int e;
            int nf;
            int elemnum = elements.Length;
            int iter = 0;
            for (iter = 0; iter < 20; iter++)
                for (e = 1; e < elemnum - 1; e++)
                {
                    Element elem = elements[e];
                    double Fluxes = 0;
                    double ElemCoeff = 0;
                    for (nf = 0; nf < Element.FACEMAX; nf++)
                    {
                        //Face face = elem.faces[nf];
                        Element nb = elements[elem.nbs[nf]];
                        //                facenormal=elems[elem].normal[nb];
                        Fluxes += nb.FluxCoeff * nb.u;
                        ElemCoeff += nb.FluxCoeff;
                    }
                    elem.u = (Fluxes + elem.SourceCoeff) / ElemCoeff;
                }

        }
        public void RunPhysic()
        {
            if (count != 0)
                return;
            count++;

//            SetBoundary();

            Calc();
        }



    }




}

