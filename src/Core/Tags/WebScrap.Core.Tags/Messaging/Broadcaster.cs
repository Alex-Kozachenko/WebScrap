using WebScrap.Core.Tags.Data;

namespace WebScrap.Core.Tags.Messaging;

internal class Broadcaster
{
    readonly HashSet<TagObserver> observers = [];

    internal void OnNext(TagsProviderMessage message, string tagName)
    {
        var candidates = observers.Where(x => x.TagName == tagName || x.TagName == null);
        foreach (var observer in candidates)
        {
            observer.Observer.OnNext(message);
        }
    }

    internal void OnCompleted()
    {
        foreach (var observer in observers)
        {
            observer.Observer.OnCompleted();
        }
    }

    internal IDisposable Subscribe(IObserver<TagsProviderMessage> observer)
    {
        var tagObserver = new TagObserver(observer, null);
        observers.Add(tagObserver);
        return new Unsubscriber<TagsProviderMessage>(observers, tagObserver);
    }

    internal IDisposable Subscribe(IObserver<TagsProviderMessage> observer, string tagName)
    {
        var tagObserver = new TagObserver(observer, tagName);
        observers.Add(tagObserver);
        return new Unsubscriber<TagsProviderMessage>(observers, tagObserver);
    }

    private sealed class Unsubscriber<T>(
        ICollection<TagObserver> observers, 
        TagObserver target)
        : IDisposable
    {
        private readonly ICollection<TagObserver> observers = observers;
        private readonly TagObserver target = target;

        public void Dispose()
        {
            observers.Remove(target);
        }
    }
}
