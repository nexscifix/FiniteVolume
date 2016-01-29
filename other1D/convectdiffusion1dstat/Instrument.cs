using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace fluidedotnet
{

  public  class Instrument
  {
    public int mousex, mousey;
    public int  zoomx;
    public bool mouseDown = false;
    public bool changeinstrument = false;

    public bool none = true;
    public bool zoom = false;
    public bool save = false;
    public bool drop = false;
    public bool push = false;
    public bool pipe = false;
    public bool setter = false;
    public bool remover = false;
    public bool forceup = false;
    public bool forcedown = false;
    public bool forcelift = false;
    public bool forcemove = false;

    public bool showneed = false;
    public bool showsurf = false;
    public bool pause = false;
    public bool step = false;
  }

}
