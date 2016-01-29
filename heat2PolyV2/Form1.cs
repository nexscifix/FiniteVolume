using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace fluidedotnet
{
  public partial class Form1 : Form
  {
    Scene scene=new Scene();
    bool run = true;
    bool isdraw = false;
    Graphics g =null;
    Graphics gform = null;
    World world = null;
    NanoTimer[] timers = null;
    FormParam formparam;
    MyCallbackDelegate reinitcallback;

    SceneParam param=new SceneParam();
    bool needinit = true;
    bool needclear = false;

    Instrument instrument = new Instrument();
    //bool needpipe = false;
    //int pipex, pipey;
    //bool drop = false;
    bool stopped = false;
    int curcycl = 0;
    public int pointx=-1;
    public int pointy=-1;

    //bool mouseDown = false;
    Bitmap bm ;
    Graphics bmg;

    public Form1()
    {
      InitializeComponent();
    }
    private void timer1_Tick(object sender, EventArgs e)
    {
    }
    public void ReInit(SceneParam param_)
    {
      param = param_;
      if (param.instrument.changeinstrument)
      {
        instrument.changeinstrument = param.instrument.changeinstrument;
        instrument.zoom = param.instrument.zoom;
        instrument.zoomx = param.instrument.zoomx;
        instrument.save = param.instrument.save;
        instrument.none = param.instrument.none;
        instrument.drop = param.instrument.drop;
        instrument.push = param.instrument.push;
        instrument.forceup = param.instrument.forceup;
        instrument.forcedown = param.instrument.forcedown;
        instrument.pipe = param.instrument.pipe;
        instrument.remover = param.instrument.remover;
        instrument.setter = param.instrument.setter;
        instrument.forcelift = param.instrument.forcelift;
        instrument.forcemove = param.instrument.forcemove;

        instrument.showneed = param.instrument.showneed;
        instrument.showsurf = param.instrument.showsurf;
        instrument.pause = param.instrument.pause;
        instrument.step = param.instrument.step;
      }
      else
        needinit = true;
    }
    private void Form1_Shown(object sender, EventArgs e)
    {
        bm = new Bitmap(Width, Height);
        //param.file = "test1";
      param.file = "cunami2";
      param.gx = -0.01;
      param.reinit = true;
      reinitcallback = new MyCallbackDelegate(ReInit);
      formparam = new FormParam();
      formparam.Show();
      formparam.InitParam("Demos", reinitcallback);
      //formparam.Left = 500;
      gform = Graphics.FromHwnd(this.Handle);
      g = Graphics.FromImage(bm);

     // instrument.zoom = true;
     // instrument.zoomx = formparam.sc instrument.zoomx;

      timer1.Enabled = false;
      ReceiveWorkerThread.WorkerReportsProgress = true;
      ReceiveWorkerThread.RunWorkerAsync();
    }

    private void ReceiveWorkerThread_DoWork(object sender, DoWorkEventArgs e)
    {
      for (int i = 0; ; i++)
      {
        if (!run)
        {
          Thread.Sleep(15);
          continue;
        }
          /*
          if (!run)
          {
            //Thread.CurrentThread.Abort();
            return;
          }
           */
        if (needinit && param.reinit)
        {
          //      scene.init("Demos\\cunami");
          scene.SetInstrument(instrument);
          scene.init("Demos\\" + param.file);
          needclear = true;
        }

        if (needinit)
        {
          needinit = false;
        }
        if (instrument.changeinstrument)
        {
          instrument.changeinstrument = false;
          scene.SetInstrument(instrument);
        }
        if (instrument.step)
        {
          instrument.step = false;
          scene.RunPhysicStep(true);
          ThreadParam p = new ThreadParam();
          p.world = scene.GetScene();
         // p.timers = TimeUtil.Instance().GetTimers();
          ReceiveWorkerThread.ReportProgress(0, p);
        }

        if (instrument.save)
        {
          instrument.save = false;
          instrument.none = true;
          instrument.changeinstrument = false;
        }
        if (instrument.mouseDown)
        {
//          scene.ClosePipe();
          if (instrument.drop)
          {
            //scene.AddFluidToPoint(instrument.mousex,instrument.mousey,10);
          }
        }

        if(!stopped)
          scene.RunPhysicStep(false);

        if (ReceiveWorkerThread.CancellationPending == true)
        {
          break;
        }
        if (i % 10 == 0 && !stopped)
        {
          //isdraw = true;
//          World world2 = scene.GetScene();
//          timers
          ThreadParam p = new ThreadParam();
          p.world = scene.GetScene();
         // p.timers = TimeUtil.Instance().GetTimers();
          ReceiveWorkerThread.ReportProgress(0,p);
          //while (isdraw) ;
          Thread.Sleep(125);
        }
//        if (i % 3 == 0 && !stopped)
//          Thread.Sleep(4);
        //if (i % 7 == 0)
       //  Thread.Sleep(362);
      }

    }
    private void ReceiveWorkerThread_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      if (!run)
        return;
      ThreadParam p = (ThreadParam)e.UserState;
      world=p.world;
//      timers = p.timers;
//      world.timers[0] = timers;
      Draw(g);
      //g.Clear(Color.White);
      //Invalidate();
      //panel1.Invalidate();
      //      scene.RenderScene(g, world, this.ClientRectangle.Width, this.ClientRectangle.Height);
      isdraw = false;
      //e.Graphics.Clear(Color.White);
      //    scene.Render(e.Graphics, this.ClientRectangle.Width, this.ClientRectangle.Height);
    }

    private void Draw(Graphics gr)
    {
      if (g != null && world != null)
      {
        world.pointx=pointx;
        world.pointy=pointy;
        gr.Clear(Color.White);
        //scene.RenderSceneSmooth(gr, world, this.ClientRectangle.Width, this.ClientRectangle.Height,1);
        //scene.RenderSceneParticles(gr, world, this.ClientRectangle.Width, this.ClientRectangle.Height);
        scene.RenderScene(gr, world, this.ClientRectangle.Width, this.ClientRectangle.Height);
        //scene.RenderSceneFlag(gr, world, this.ClientRectangle.Width, this.ClientRectangle.Height);
        
        //scene.dh = 10;

       //scene.RenderSmoothParticles(gr, world, this.ClientRectangle.Width, this.ClientRectangle.Height);
        //if (curcycl > 10)
        //{
        //  curcycl = 0;
        //scene.RenderTimers(gr, world, timers, this.ClientRectangle.Width, this.ClientRectangle.Height);
        //}
        //curcycl++;

        gform.DrawImage(bm, 0, 0);
      }
    }
    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      run = false;
      ReceiveWorkerThread.Dispose();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      //world = scene.GetScene();
      //scene.CreateSurface(world);
      //Invalidate();
      //panel1.Invalidate();
      //run = false;
      //ReceiveWorkerThread.CancelAsync();
    }

    private void button2_Click(object sender, EventArgs e)
    {
     // scene.Render(g, this.ClientRectangle.Width, this.ClientRectangle.Height);
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
      return;
    }

    private void Form1_KeyDown(object sender, KeyEventArgs e)
    {
      //instrument.drop = true;
    }

    private void Form1_MouseDown(object sender, MouseEventArgs e)
    {
      //instrument.drop = true;
      //instrument.drop = true;
      //instrument.pipe = true;
      instrument.mousex = e.X;
      instrument.mousey = e.Y;
      instrument.mouseDown = true;
      pointx=e.X;
      pointy=e.Y;
    }

    private void Form1_MouseMove(object sender, MouseEventArgs e)
    {
      instrument.mousex = e.X;
      instrument.mousey = e.Y;
    }

    private void Form1_MouseUp(object sender, MouseEventArgs e)
    {
      instrument.mouseDown = false;
    }

  }
}
