using System;
using System.IO;
using EVector = EgenLib.EMath.Vector3;
using Vector2 = EgenLib.EMath.Vector2;
using EMath = EgenLib.EMath;


namespace FiniteVolume2D
{
    /*
    public class FaceType
    {
        public const int WEST = 0;
        public const int EAST = 1;
    }

public class Face
{
//    public Element owner;
//    public Vector2[] vertex = new Vector2[2];
    public Vector2 centroid = new Vector2();
    // unit normal vector to face (outward to owner element)
    public Vector2 normal = new Vector2();
    public Vector2 sf = new Vector2();
    public double area;
//    public double nodesdst;

    public Face(Vector2 v1, Vector2 v2)
    {
        vertex[0] = v1;
        vertex[1] = v2;
    }
    public void Calc()
    {
        centroid = (vertex[0] + vertex[1]) / 2;
        normal = EMath.Geometry2D.LineNormal(vertex[0],vertex[1]);
        sf = normal * (vertex[0]-vertex[1]).Length();
    }
}
    */

}

