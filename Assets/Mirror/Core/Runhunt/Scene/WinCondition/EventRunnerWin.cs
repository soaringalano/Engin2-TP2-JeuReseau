using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRunnerWin : AbstractEvent
{

    public string playerName { get; private set; }

    public EventRunnerWin(double time, string playerName) : base(time)
    {
        this.playerName = playerName;
    }

}
