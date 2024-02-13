using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractEvent : IEvent
{

    protected string name;

    protected double time;

    public AbstractEvent(string name, double time)
    {
        this.name = name;
        this.time = time;
    }

    public AbstractEvent(double time)
    {
        this.name = this.GetType().ToString();
        this.time = time;
    }

    public double GetTime()
    {
        return time;
    }

    public string GetName()
    {
        return name;
    }

}
