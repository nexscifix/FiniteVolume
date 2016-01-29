using System;
using System.IO;
using REAL = System.Double;


namespace fluidedotnet
{
    /*
       
         -----W--w--P--e--E---- 
  
      dT/dt = D*(dT/dx)e - D*(dT/dx)w 
  
      dT/dt = D*(TE-TP) - D*(TP-TW) 
      RoC*(TPNew-TP)/dt = D*(TE-TP) - D*(TP-TW) 

      Cdt*(TPNew-TP) = D*(TE-TP) - D*(TP-TW) 
      
      
      TPNew =TP + ( D*(TE-TP) - D*(TP-TW) )/Cdt 
      
     */

    /*
    public class Face
{
//    int ownerid;
//    int nbid;
    public double Area;
    public double nodesdst;
}
*/

    public class FaceType
    {
        public const int WEST = 0;
        public const int EAST = 1;
    }


public class Element
{
    public enum BoundaryType
    {
        BND_NO = 0,  // не граница
        BND_CONST =1,  // задана температура на границе
        BND_INSULATED =2  // задан тепловой поток на границе 
    };
    public const int FACEMAX = 2;

    //    public Face[] faces = new Face[FACEMAX];
    public Element[] nbs = new Element[FACEMAX];
    public int[] facenormals = new int[FACEMAX];
    public double u;
    public double SourceCoeff;
    public double dh;
    public double volume;
    public BoundaryType boundaryType;
    //    public double FluxCoeff;
//    public double ConvectCoeff;

    //  msh_vector normal;
//  msh_vector celement;
//  int nbnodes;
//  int node[8];
};


    public class MainSolver
    {

//        int nodecount;
//        int facecount;
        int elementcount;
        public Element[] elements;
        public double GlobCoeff;
        public double GlobRo;
        public double GlobU;
        public double FaceArea;

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
        public double GetU(int x, int y)
        {
            return elements[x].u;
        }

        // -------------------- COMPUTE ---------------------------------
        // -------------------- COMPUTE ---------------------------------


        public void Calc()
        {
            int e;
            //            int iter = 0;
            //            for (iter = 0; iter < 8; iter++)
            for (e = 1; e < elementcount - 1; e++)
            {
                Element elem = elements[e];

                int nf;
                double gradu = 0;
                for (nf = 0; nf < Element.FACEMAX; nf++)
                {
                    Element nb = elem.nbs[nf];
                    // calc gradient
                    if (nb.boundaryType != Element.BoundaryType.BND_INSULATED)
                        gradu += GlobCoeff * ((nb.u - elem.u) / nb.dh);
//                    int nw = elem.facenormals[FaceType.WEST];
//                    int ne = elem.facenormals[FaceType.EAST];
//                    gradu+=(GlobCoeff * ne * grade + GlobCoeff * nw * gradw);
                }
                double coeff = (GlobRo * elem.dh) / delt;
                elem.u = elem.u + (gradu) / coeff;
            }
            //      TPNew =TP + ( D*(TE-TP) - D*(TP-TW) )/Cdt 

        }


        // -------------------- MAIN ---------------------------------
        // -------------------- MAIN ---------------------------------


        public void Destroy()
        {

        }
        public void SetBoundary()
        {////ex 8.1

            double dh2 = (xlength / (elementcount - 2)) / 2;
            elements[0].u = 200;
            elements[0].boundaryType = Element.BoundaryType.BND_INSULATED;
            elements[0].dh = dh2;

            elements[elementcount - 1].u = 0;
            elements[elementcount - 1].boundaryType = Element.BoundaryType.BND_CONST;
            elements[elementcount - 1].dh = dh2;

            int e;
            for (e = 0; e < elementcount-1; e++)
            {
                elements[e].u = 200;
            }
        }
        public void SetParameters()
        {
            xlength = 0.02;
            elementcount = 7;
            GlobRo = 10000000;
            GlobU = 1;
            GlobCoeff = 10;
            delt = 2;
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

            elements = new Element[elementcount];
            int e;
            for (e = 0; e < elementcount; e++)
            {
                elements[e] = new Element();
            }

            for (e = 1; e < elementcount - 1; e++)
            {
                Element elem = elements[e];
                elem.dh = xlength / (elementcount - 2);
                elem.volume = elem.dh * FaceArea;
                elem.u = 0;
                elem.SourceCoeff = 0;
                elem.boundaryType = Element.BoundaryType.BND_NO;
                elem.nbs[0] = elements[e - 1];
                elem.nbs[1] = elements[e + 1];
                elem.facenormals[0] = -1;
                elem.facenormals[1] = 1;
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
            if (count>59)
                return;
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