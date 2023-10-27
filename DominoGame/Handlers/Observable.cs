namespace DominoGame.Handlers;

public class Observable
{
    private readonly IList<IUpdate> _observers;

    public Observable()
    {
        _observers = new List<IUpdate>();
    }

    protected void Notify()
    {
        foreach (IUpdate observer in _observers)
        {
            observer.Update(this);
        }
    }

    public void Attach(IUpdate observer)
    {
        _observers.Add(observer);
    }
}