public interface IObservedObject
{

    public void NotifyObservers(IEvent e);

    public void RegisterObserver(IObserver observer);

}