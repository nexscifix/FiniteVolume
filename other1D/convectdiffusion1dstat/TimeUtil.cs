using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;


namespace fluidedotnet
{


public class TimeUtil
{
  public NanoTimer[] timers=new NanoTimer[100];

  private static TimeUtil instance;

  private TimeUtil()
  {
      ResetTimers();
  }

  public static TimeUtil Instance()
  {
    if (instance == null)
      instance = new TimeUtil();
    return instance;

  }
  public void ResetTimers()
  {
    timers=new NanoTimer[100];
    for (int n = 0; n < 100; n++)
    {
      timers[n] = new NanoTimer(n.ToString());
      timers[n].duration = 0;
    }
  }
    /*
  public NanoTimer[] GetTimers()
  {
    NanoTimer[]  t = new NanoTimer[100];
    for (int n = 0; n < 100; n++)
    {
        t[n] = timers[n].Clone();
//        t[n]=new NanoTimer(timers[n].name);
//        t[n].duration = timers[n].duration;
    }
    return t;
  }
*/     
  public void StartTimer(int index)
  {
//    timers[index]=new NanoTimer(name);
    timers[index].Start();
  }
  public void StopTimer(int index)
  {
    timers[index].End();
  }
  

}


}