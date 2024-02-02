using System.Collections.Generic;
using UnityEngine;

public class GameEventOnPlayerRoleSelected : GameEventSubject
{
    private List<GameEventObserver> m_observers = new List<GameEventObserver>();

    public void OnPlayerRoleSelected()
    {
        Debug.Log("OnPlayerRoleSelected()");
        foreach (var observer in m_observers)
        {
            observer.OnPlayerRoleSelected();
        }
    }

    override public void AddObserver(GameEventObserver observer)
    {
        Debug.Log("AddObserver");
        m_observers.Add(observer);
    }

    override public void RemoveObserver(GameEventObserver observer)
    {
        m_observers.Remove(observer);
    }
}