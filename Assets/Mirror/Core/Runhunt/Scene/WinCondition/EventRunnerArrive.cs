using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRunnerArrive : AbstractEvent
{

    public string playerName { get; private set; }

    public EventRunnerArrive(double time, string playerName) : base(time)
    {
        this.playerName = playerName;
    }

}
