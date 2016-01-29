using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using REAL = System.Double;
using System.Drawing.Drawing2D;
using EgenLib.EMath;





//extern HighResolutionTimer timer6;
//extern HighResolutionTimer timer7;

namespace fluidedotnet
{
  public class World
  {
    public int[,] fluidcell;
    public int xnum;
    public int ynum;
    public int pointx = -1;
    public int pointy = -1;
    public NanoTimer[] timers;

    public World()
    {
        timers = new NanoTimer[2];
        timers[0] = new NanoTimer("0");
        timers[1] = new NanoTimer("0");
    }
  }

  public class CellSelected
  {
    public int i;
    public int j;
  }

  public class Scene
  {
    public bool created = false;

    public double zoom;
    public double localzoom;
    public double vectorscale;
    public double uvscale;
    //  public bool   byonestep;
    // public bool   readytostep;    

    public string filename;
    public bool drawfast = false;
    public bool drawparticles = false;
    public bool drawId = false;
    public bool drawvelocity = false;
    public bool drawpressure = false;
    public bool drawchild = false;
    public MainSolver fluid = new MainSolver();
    //  WaterPipe       pipe=new WaterPipe();
    //  ForceRegion force = new ForceRegion();
    //  ForceRegion forceact = new ForceRegion();


    public int startx;
    public int starty;

    int count = 0;

    Font font = new Font("Arial", 8);
    SolidBrush textBrush = new SolidBrush(Color.Black);
    SolidBrush waterb = new SolidBrush(Color.FromArgb(239, 239, 255));
    SolidBrush emptyb = new SolidBrush(Color.FromArgb(255, 255, 230));


    public bool showneed = false;
    public bool showsurf = false;
    public bool pause = false;

    CellSelected selectedCell = null;

    public Scene()
    {
      drawparticles = true;
      drawchild = true;
      zoom = 1;
      localzoom = 20;
      //dh = 13.0;
      //dh = 30.0;
      //dh = 45.0;
      //dh = 60.0;
      startx = 200;
      starty = 900;
      uvscale = 100;
      //  vectorscale=0.5;
      // byonestep=false;
      // readytostep=true;    
      filename = "";
      created = false;
      vectorscale = 0.7;
    }
    public void DrawAxes(Graphics gdi, Pen b, int w, int h)
    {
      gdi.DrawLine(b, startx, 0, startx, h);
      gdi.DrawLine(b, 0, starty, w, starty);
    }
    public void DrawString(Graphics gdi, string str, Font font, Brush b, int x, int y)
    {
      gdi.DrawString(str, font, b, startx + x, starty - y - font.Size * 2);
    }

    public void FillEllipse(Graphics gdi, Brush b, int cellx1, int celly1, int w, int h)
    {
      gdi.FillEllipse(b, startx + cellx1 - w / 2, starty - celly1 - h / 2, w, h);
    }

    public void FillRectangle(Graphics gdi, Brush b, int cellx1, int celly1, int w, int h)
    {
        gdi.DrawRectangle(new Pen(b), startx + cellx1, starty - celly1 - h, w, h);
        gdi.FillRectangle(b, startx + cellx1, starty - celly1 - h, w, h);
    }
    public void DrawRectangle(Graphics gdi, Pen b, int cellx1, int celly1, int w, int h)
    {
      gdi.DrawRectangle(b, startx + cellx1, starty - celly1 - h, w, h);
    }
    public void DrawLine(Graphics gdi, Pen b, int cellx1, int celly1, int cellx2, int celly2)
    {
      gdi.DrawLine(b, startx + cellx1, starty - celly1, startx + cellx2, starty - celly2);
    }
    public void DrawVectorReal(Graphics gdi, Pen pen, int x1, int y1, double vx, double vy, double lenscale)
    {
      Vector3 uv = new Vector3(vx, vy, 0);
      double len = uv.Length();
      uv.Normalize();
      uv = uv * lenscale;
      Pen myPen = (Pen)pen.Clone();
      myPen.Width = (float)(len/4);
      if (myPen.Width > 6)
          myPen.Width = 6;

      DrawLine(gdi, myPen, x1, y1, x1 + (int)(uv.x / 1.8), y1 + (int)(uv.y/ 1.8));
//      DrawLine(gdi, myPen, x1, y1, x1 + (int)(uv.x), y1 + (int)(uv.y ));

      if (Math.Abs(vy) < 0.0000001)
        vy = 0.001;
      {
        double x2 = 1;
        double y2 = -(vx * x2) / vy;
        Vector3 p = new Vector3(x2, y2, 0);
        p.Normalize();
        p = p * (lenscale / 2);
        Vector3 av = uv / 1.8;
        int ax = x1 + (int)av.x;
        int ay = y1 + (int)av.y;
        Vector3 p1 = p / 1.5;
        Vector3 p2 = -p1;
        //        gdi.DrawLine(pen, ax, ay, ax + (int)p1.x, ay + (int)p1.y);
        //        gdi.DrawLine(pen, ax, ay, ax + (int)p2.x, ay + (int)p2.y);
        int sx1 = ax + (int)p1.x;
        int sy1 = ay + (int)p1.y;
        int sx2 = ax + (int)p2.x;
        int sy2 = ay + (int)p2.y;
        //gdi.DrawLine(pen, sx1, sy1, x1 + (int)uv.x, y1 + (int)uv.y);
        //gdi.DrawLine(pen, sx2, sy2, x1 + (int)uv.x, y1 + (int)uv.y);
        //gdi.DrawLine(pen, sx1, sy1, sx2, sy2);

        GraphicsPath path = new GraphicsPath();
        path.AddLines(new Point[] { new Point(startx + sx1, starty - sy1), new Point(startx + sx2, starty - sy2), new Point(startx + x1 + (int)uv.x, starty - (y1 + (int)uv.y)), });
        path.CloseFigure();
//        gdi.DrawPath(Brushes.Red, path);
        gdi.DrawPath(Pens.Red, path);

      }
    }
    // H - цвет, S - насыщенность, V - €ркость
    public void HsvToRgb(double h, double S, double V, out int r, out int g, out int b)
    {
        /*
        // use
        int r1, g1, b1;
        int step1 = 5;
        for (k = 0; k < 320; k += step1)
        {
            HsvToRgb(k, 1, 1, out r1, out g1, out b1);
            b = new SolidBrush(Color.FromArgb(r1, g1, b1));
            FillRectangle(gdi, b, 100 + k, 200, step1, 40);
        }
*/

        double H = h;
        while (H < 0) { H += 360; };
        while (H >= 360) { H -= 360; };
        double R, G, B;
        if (V <= 0)
        { R = G = B = 0; }
        else if (S <= 0)
        {
            R = G = B = V;
        }
        else
        {
            double hf = H / 60.0;
            int i = (int)Math.Floor(hf);
            double f = hf - i;
            double pv = V * (1 - S);
            double qv = V * (1 - S * f);
            double tv = V * (1 - S * (1 - f));
            switch (i)
            {



                case 0:
                    R = V;
                    G = tv;
                    B = pv;
                    break;



                case 1:
                    R = qv;
                    G = V;
                    B = pv;
                    break;
                case 2:
                    R = pv;
                    G = V;
                    B = tv;
                    break;



                case 3:
                    R = pv;
                    G = qv;
                    B = V;
                    break;
                case 4:
                    R = tv;
                    G = pv;
                    B = V;
                    break;



                case 5:
                    R = V;
                    G = pv;
                    B = qv;
                    break;



                case 6:
                    R = V;
                    G = tv;
                    B = pv;
                    break;
                case -1:
                    R = V;
                    G = pv;
                    B = qv;
                    break;



                default:

                    R = G = B = V;
                    break;
            }
        }
        r = Clamp((int)(R * 255.0));
        g = Clamp((int)(G * 255.0));
        b = Clamp((int)(B * 255.0));
    }

    int Clamp(int i)
    {
        if (i < 0) return 0;
        if (i > 255) return 255;
        return i;
    }


    public void Create()
    {
    }
    public void init(string filename_)
    {
      filename = filename_;
      if (created)
        fluid.Destroy();
      if (!fluid.Create(filename))
        throw new Exception("error init");

      created = true;
    }

    public void SetInstrument(Instrument i)
    {
      if (i.showneed)
      {
        showneed = true;
        showsurf = false;
      }
      else if (i.showsurf)
      {
        showsurf = true;
        showneed = false;
      }
      else if (i.pause)
      {
        pause = !pause;
      }
      else if (i.zoom)
      {
          zoom = i.zoomx;
      }

    }
    /*
  // make fluid drop 
  public void AddFluidToPoint(int x,int y,int size)
  {
    int i=x;
    int j=y;
  
    PointToCell(x,y,ref i,ref j);

    fluid.SETBCOND();
    int color=100;

    for(int n=i;n<i+size;n++)
      for(int m=j;m<j+size;m++)
      {
        if (fluid.GetCellType(n,m) == Fluid2D.CellType.CELL_EMPTY)
          fluid.MakeParticlesInCell(n,m,color);
      }
  }
    */
    // process physic step of fluid
    public void RunPhysicStep(bool onestep)
    {
      if (pause)
      {
        if (onestep == false)
          return;
      }

      //  static bool added=false;
      int xnum = 0, ynum = 0;
     // fluid.GetDomainSize(ref xnum, ref ynum);
      TimeUtil tu = TimeUtil.Instance();

//      for (int n = 0; n < 3; n++)
      {
          tu.StartTimer(0);
          fluid.RunPhysic();
          tu.StopTimer(0);
      }


      /*
      ToFile("u.txt", 1);
      ToFile("v.txt", 2);
      ToFile("f.txt", 3);
      ToFile("g.txt", 4);
      ToFile("p.txt", 5);
      */

    }

    public World GetScene()
    {
      World world = new World();
      fluid.GetDomainSize(ref world.xnum, ref world.ynum);
      TimeUtil tu = TimeUtil.Instance();
      tu.timers[0].Copy(world.timers[0]);
      return world;
    }
    /*
    public void DrawCellInfo(Graphics gdi, World world)
    {
      //int xnum = 0;
      //int ynum = 0;
      //fluid.GetDomainSize(ref xnum, ref ynum);

      if (selectedCell == null)
        return;

      int i = selectedCell.navi;
      int j = selectedCell.navj;
      if (selectedCell.IsChild())
      {
        i = fluid.Parent(selectedCell).navi;
        j = fluid.Parent(selectedCell).navj;
      }

      int cellx1 = (int)(dh * i);
      int cellx2 = (int)dh;
      int celly1 = (int)(dh * j);
      int celly2 = (int)dh;
      if (selectedCell.IsChild())
      {
        i = selectedCell.navi;
        j = selectedCell.navj;
        double dhsub = dh / selectedCell.subcount;
        cellx1 += (int)(dhsub * i);
        celly1 += (int)(dhsub * j);
        cellx2 = (int)(dhsub);
        celly2 = (int)(dhsub);
      }
      Pen p = new Pen(Color.DarkOrange, 3);
      DrawRectangle(gdi, p, cellx1, celly1, cellx2, celly2);


      string dformat = "0.000000";
      int n = 0;
      int x = -startx + 5;
      int y = starty / 2 - 20;
      int colw = 100;
      //gdi.DrawString("i j=", font, textBrush, x, y + n * 20);
      //gdi.DrawString("[ " + i.ToString() + " ] [ " + j.ToString() + " ]", font, textBrush, x + 150, y + n * 20);

      n--;
      DrawString(gdi, "FLAG", font, textBrush, x, y + n * 20);
      DrawString(gdi, selectedCell.FLAG.ToString(), font, textBrush, x + colw, y + n * 20);
      n--;
      DrawString(gdi, "FLAGSURF", font, textBrush, x, y + n * 20);
      DrawString(gdi, selectedCell.FLAGSURF.ToString(), font, textBrush, x + colw, y + n * 20);
      n--;
      DrawString(gdi, "U=", font, textBrush, x, y + n * 20);
      DrawString(gdi, selectedCell.U.ToString(dformat), font, textBrush, x + colw, y + n * 20);
      n--;
      DrawString(gdi, "V=", font, textBrush, x, y + n * 20);
      DrawString(gdi, selectedCell.V.ToString(dformat), font, textBrush, x + colw, y + n * 20);
      n--;
      DrawString(gdi, "F=", font, textBrush, x, y + n * 20);
      DrawString(gdi, selectedCell.F.ToString(dformat), font, textBrush, x + colw, y + n * 20);
      n--;
      DrawString(gdi, "G=", font, textBrush, x, y + n * 20);
      DrawString(gdi, selectedCell.G.ToString(dformat), font, textBrush, x + colw, y + n * 20);
      n--;
      DrawString(gdi, "P=", font, textBrush, x, y + n * 20);
      DrawString(gdi, selectedCell.P.ToString(dformat), font, textBrush, x + colw, y + n * 20);

      string schild = "";
      for (int k = 0; k < selectedCell.nbr.Length; k++)
      {
        schild += selectedCell.nbr[k].ToString();
        schild += " , ";
      }
      n--;
      DrawString(gdi, "NEIBRS=", font, textBrush, x, y + n * 20);
      DrawString(gdi, schild, font, textBrush, x + colw, y + n * 20);
      n--;
      DrawString(gdi, "Parent=", font, textBrush, x, y + n * 20);
      DrawString(gdi, selectedCell.pid.ToString(), font, textBrush, x + colw, y + n * 20);


    }
       */
      
    public void GetCellCoord(int x, int y, ref int cellx1, ref int celly1, ref int cellx2, ref int celly2)
    {
        double dcellx1 = 0, dcelly1 = 0, dcellx2 = 0, dcelly2 = 0;
        fluid.GetCellCoord(x, y, ref  dcellx1, ref  dcelly1, ref  dcellx2, ref  dcelly2);
        cellx1 = (int)(dcellx1 * zoom*localzoom); cellx2 = (int)(dcellx2 * zoom*localzoom); celly1 = (int)(dcelly1 * zoom*localzoom); celly2 = (int)(dcelly2 * zoom*localzoom);
    }
       
    public void GetCellRect(int x, int y, ref int cellx1, ref int celly1, ref int w, ref int h)
    {
        double hx = 0;
        double hy = 0;
        fluid.GetCellSize(ref hx, ref hy);
        w = (int)(hx * zoom*localzoom);
        h = (int)(hy * zoom*localzoom);

        double dcellx1 = 0, dcelly1 = 0, dcellx2 = 0, dcelly2 = 0;
        fluid.GetCellCoord(x, y, ref  dcellx1, ref  dcelly1, ref  dcellx2, ref  dcelly2);
        cellx1 = (int)(dcellx1 * zoom*localzoom); celly1 = (int)(dcelly1 * zoom*localzoom); 
    }
      /*
    public void RenderGrid(Graphics gdi, World world, int w, int h)
    {
      int xnum = 0;
      int ynum = 0;
      int cellx1 = 0, celly1 = 0, cellx2 = 0, celly2 = 0;
      fluid.GetDomainSize(ref xnum, ref ynum);

      for (int y = 0; y < world.ynum; y++)
        for (int x = 0; x < world.xnum; x++)
        {
            GetCellRect(x, y, ref  cellx1, ref  celly1, ref  cellx2, ref  celly2);
       //   if (fluid.IsObstacle(x, y))
       //       FillRectangle(gdi, Brushes.LightGreen, cellx1, celly1, cellx2, celly2);
//          else
            FillRectangle(gdi, Brushes.LightBlue, cellx1, celly1, cellx2, celly2);
          DrawRectangle(gdi, Pens.Black, cellx1, celly1, cellx2, celly2);
        }

    }
       */ 
    public void RenderParentGrid(Graphics gdi, int x, int y)
    {
        int cellx1 = 0, celly1 = 0, cellx2 = 0, celly2 = 0;

        GetCellRect(x, y, ref  cellx1, ref  celly1, ref  cellx2, ref  celly2);
        DrawRectangle(gdi, Pens.LightGray, cellx1, celly1, cellx2, celly2);
    }
    public void RenderParent(Graphics gdi, int x, int y, int areacoeff, double tmax)
    {
        int cellx1 = 0, celly1 = 0, cellx2 = 0, celly2 = 0;
        //        int cx1 = 0, cy1 = 0, cx2 = 0, cy2 = 0;

        //GetCellCoord(x, y, ref  cx1, ref  cy1, ref  cx2, ref  cy2);

        GetCellRect(x, y, ref  cellx1, ref  celly1, ref  cellx2, ref  celly2);

        //cellx1=(int)(x*zoom*localzoom);    celly1=(int)(y*zoom*localzoom);    cellx2=cellx1+(int)zoom*localzoom;   celly2 = celly1 + (int)zoom*localzoom;

        double coeff = 1.0;
        double t = fluid.GetU(x, y);
        t = Math.Abs(t) * coeff;

        int r1, g1, b1;
        HsvToRgb(240 - t, 1, 1, out r1, out g1, out b1);

        Brush b = new SolidBrush(Color.FromArgb(r1, g1, b1));
        FillRectangle(gdi, b, cellx1, celly1, cellx2 * areacoeff, celly2 * areacoeff);
    }
    public void RenderTimings(Graphics gdi, World world, int w, int h)
    {
        //TimeUtil tu = TimeUtil.Instance();

        string doubleformat = "0.0";

        int x =0;
        int y = -50;
        int rownum = 0;
        int fonth = 20;
        int valueoffset = 100;
        double allcalc = world.timers[0].CalcAverageMicro()/1000.0;
        
        DrawString(gdi, "Calc Time =", font, textBrush, x, y + rownum * fonth);
        DrawString(gdi, allcalc.ToString(doubleformat), font, textBrush, x + valueoffset, y + rownum * fonth);
    }
    public void RenderScene(Graphics gdi, World world, int w, int h)
    {
        //RenderTimings(gdi, world, w, h);
        //return;
        int i, j;
        int x, y;
        selectedCell = null;
        if (world.pointx != -1)
        {
            //selectedCell = PointToCell(world.pointx, world.pointy);
        }

        DrawAxes(gdi, Pens.Black, w, h);

        double tmax = 0;
        y = 0;
//        for (y = 0; y < world.ynum; y++)
            for (x = 0; x < world.xnum; x++)
            {
                RenderParent(gdi, x, y, 1, tmax);
            }

        RenderTimings(gdi, world, w, h);


        //DrawCellInfo(gdi, world);

    }
    /*
    //return fluid cell by coordinates of screen pixel 
    public Cell2D PointToCell(int x, int y)
    {
      //int xnum = 0;
      //int ynum = 0;
      //fluid.GetDomainSize(ref xnum, ref ynum);
      double cellsizex = 0, cellsizey = 0;
      fluid.GetCellSize(ref cellsizex, ref cellsizey);
      double pmult = cellsizex / dh;
      double xx = x - startx;
      double yy = starty - y;
      xx *= pmult;
      yy *= pmult;

      int xnum = 0;
      int ynum = 0;
      fluid.GetDomainSize(ref xnum, ref ynum);
      double xend = xnum * cellsizex;
      double yend = ynum * cellsizey;

      if (xx > xend || x < 0)
        return null;
      if (yy > yend || y < 0)
        return null;

      return fluid.CellByParticle(xx, yy);
    }
    */




  }



}






