using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using Vector2 = EgenLib.EMath.Vector2;


namespace FiniteVolume2D
{

    public class FVMath2D
    {
        public static double FaceWeightFactor(Element main, Element e,Face face)
        {
            double wf = 0;
            Vector2 dcf = face.centroid - main.centroid;
            Vector2 dff = e.centroid - face.centroid;
            wf = (dcf * face.normal) / ((dcf +dff) * face.normal);

            return wf;
        }
        public static double FaceWeightFactorSimple(Element main, Element e, Face face)
        {
            double wf = main.volume / (main.volume + e.volume);
            return wf;
        }

    }

}