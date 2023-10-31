namespace WebScrap.Core.Tags;

internal sealed class Unsubscriber<T>(
        ICollection<IObserver<T>> observers, 
        IObserver<T> target)
    : IDisposable
{
    private readonly ICollection<IObserver<T>> observers = observers;
    private readonly IObserver<T> target = target;

    public void Dispose()
    {
        observers.Remove(target);
    }
}