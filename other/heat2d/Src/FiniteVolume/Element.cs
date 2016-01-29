using System;
using System.IO;
using EVector = EgenLib.EMath.Vector3;
using Vector2 = EgenLib.EMath.Vector2;
using EMath = EgenLib.EMath;


namespace FiniteVolume2D
{
    /*
    public enum BoundaryType
    {
        BND_NO = 0,  // не граница
        BND_CONST =1,  // задана температура на границе
        BND_INSULATED =2  // задан тепловой поток на границе 
    };
     */ 


public class Element
{
    public const int FACEMAX = 4;

//    public Face[] faces = new Face[FACEMAX];
//    public Element[] nbs = new Element[FACEMAX];
//    public double[] cfdist = new double[FACEMAX];
    //    public Vector2[] vertex = new Vector2[FACEMAX];
//    public Vector2 centroid = new Vector2();
//    public double volume;
    public int id;
    public double u;
    public double cfdist;
    //    public double SourceCoeff;
    //    public BoundaryType boundaryType;


    public void Calc()
    {
//      CreateFaces();
//      centroid=EMath.Geometry2D.PolygonCentroid(vertex);
//      volume = EMath.Geometry2D.TriangleArea(vertex[0], vertex[1], vertex[2]);

    }
    /*
    void CreateFaces()
    {
        int nf = 0;

        for(nf=0;nf<vertex.Length-1;nf++)
        {
            faces[nf]=new Face(vertex[nf],vertex[nf+1]);
            faces[nf].Calc();
        }

    }
*/

};




}

