using System.Collections.Generic;

public class GameEventOnPlayerRoleSelected : GameEventSubject
{
    private List<GameEventObserver> observers = new List<GameEventObserver>();

    override public void Notify()
    {
        foreach (var observer in observers)
        {
            observer.OnPlayerRoleSelected();
        }
    }

    override public void AddObserver(GameEventObserver observer)
    {
        observers.Add(observer);
    }

    override public void RemoveObserver(GameEventObserver observer)
    {
        observers.Remove(observer);
    }
}
