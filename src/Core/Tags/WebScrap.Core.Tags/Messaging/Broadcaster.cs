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
        return new Unsubscriber<TagsProviderMessage>(observers, [tagObserver]);
    }

    internal IDisposable Subscribe(IObserver<TagsProviderMessage> observer, params string[] tagNames)
    {
        if (tagNames == null)
        {
            return Subscribe(observer);
        }

        var tagObservers = new List<TagObserver>();
        foreach (var tagName in tagNames)
        {
            var tagObserver = new TagObserver(observer, tagName);
            observers.Add(tagObserver);
            tagObservers.Add(tagObserver);
        }
        return new Unsubscriber<TagsProviderMessage>(observers, tagObservers);
    }

    private sealed class Unsubscriber<T>(
        ICollection<TagObserver> observers, 
        List<TagObserver> targets)
        : IDisposable
    {
        private readonly ICollection<TagObserver> observers = observers;
        private readonly List<TagObserver> targets = targets;

        public void Dispose()
        {
            foreach (var target in targets)
            {
                observers.Remove(target);
            }
        }
    }
}
