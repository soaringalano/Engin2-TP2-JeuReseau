// Source : https://unity.com/how-to/create-modular-and-maintainable-code-observer-pattern
using Mirror;
using System.Collections.Generic;

public class GameEventSubject : NetworkBehaviour
{
    private List<GameEventObserver> observers = new List<GameEventObserver>();

    virtual public void Notify()
    {

    }

    virtual public void AddObserver(GameEventObserver observer)
    {
        observers.Add(observer);
    }

    virtual public void RemoveObserver(GameEventObserver observer)
    {
        observers.Remove(observer);
    }
}