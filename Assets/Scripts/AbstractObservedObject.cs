using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractObservedObject : NetworkBehaviour, IObservedObject
{
    
    protected List<IObserver> observers = new List<IObserver>();

    public void NotifyObservers(IEvent e)
    {
        Debug.Log("Notifying observers");
        foreach (IObserver observer in observers)
        {
            observer.OnNotify(e);
        }
    }

    public void RegisterObserver(IObserver observer)
    {
        if(!observers.Contains(observer))
        {
            Debug.Log("Registering observer");
            observers.Add(observer);
        }
    }

}
