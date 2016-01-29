using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;


namespace fluidedotnet
{

public class NanoTimer
{
  public long duration ;
  public long ticks;
  public String name = "";
  Stopwatch watch ;
  long frequency;
  long nanosecPerTick ;
  long timesum ;
  long count ;
  
  public NanoTimer(String name_)
  {
    frequency = Stopwatch.Frequency;
    nanosecPerTick = (1000L * 1000L * 1000L) / frequency;
    name = name_;
    timesum = 0;
    count = 0;
  }
  public NanoTimer Clone()
    {
      NanoTimer t=new NanoTimer(name);
      t.duration = duration;
      t.ticks =ticks ;
      t.frequency =frequency ;
      t.nanosecPerTick =nanosecPerTick ;
      t.timesum =timesum ;
      t.count = count;

      return t;
    }
  public NanoTimer Copy(NanoTimer t)
  {
      t.duration = duration;
      t.ticks = ticks;
      t.frequency = frequency;
      t.nanosecPerTick = nanosecPerTick;
      t.timesum = timesum;
      t.count = count;
      return t;
  }
  public void Start()
  {
    watch = Stopwatch.StartNew();
  }
  public long End()
  {
    watch.Stop();
//    duration= watch.ElapsedTicks;
    duration =(int)( watch.ElapsedTicks * (nanosecPerTick/1000.0));
    ticks = watch.ElapsedTicks;

    timesum += duration;
    count++;

    return duration;
  }
  public long GetDurationMilli()
  {
      return duration/(long)1000.0;
  }
  public long GetDurationMicro()
  {
      return duration ;
  }
  public long CalcAverageMilli()
  {
      if (count == 0)
          return 0;
      long avg = (timesum / count) / (long)1000.0;
    return avg;
  }
  public long CalcAverageMicro()
  {
      if (count == 0)
          return 0;
      long avg = timesum / count;
      return avg;
  }

}


}