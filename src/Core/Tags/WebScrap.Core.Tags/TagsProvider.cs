using WebScrap.Core.Tags.Data;
using WebScrap.Core.Tags.Tools;

namespace WebScrap.Core.Tags;

public class TagsProvider : 
    IObservable<TagsProviderMessage>,
    IObserver<ProcessedTag>
{
    private readonly TagsDispatcher tagsDispatcher = new();
    private readonly HashSet<IObserver<TagsProviderMessage>> observers = [];
    private readonly IDisposable? tagDispatcherUnsubscriber;

    public TagsProvider()
    {
        tagDispatcherUnsubscriber = tagsDispatcher.Subscribe(this);
    }

    public void Process(ReadOnlySpan<char> html)
    {
        var charsProcessed = 0;
        do
        {
            charsProcessed += tagsDispatcher.Process(html[charsProcessed..], charsProcessed);
        } while (charsProcessed < html.Length);

        tagDispatcherUnsubscriber?.Dispose();
        ForEach(observers, o => o.OnCompleted());
    }

    public IDisposable Subscribe(IObserver<TagsProviderMessage> observer)
    {
        observers.Add(observer);
        return new Unsubscriber<TagsProviderMessage>(observers, observer);
    }

    public void OnCompleted()
    {
        tagDispatcherUnsubscriber?.Dispose();
    }

    public void OnError(Exception error) { }

    public void OnNext(ProcessedTag value)
    {
        var history = tagsDispatcher.OpenedTags
            .Reverse()
            .Select(x => x.TagInfo);
        var message = new TagsProviderMessage(
            [..history], 
            value);
        ForEach(observers, o => o.OnNext(message));
    }

    static void ForEach<T>(IEnumerable<T> set, Action<T> action)
    {
        foreach (var item in set)
        {
            action(item);
        }
    }
}
