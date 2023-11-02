using WebScrap.Core.Tags.Data;

namespace WebScrap.Core.Tags.Tests.TestHelpers;

internal class TagsProviderListener : IObserver<TagsProviderMessage>
{
    IDisposable? unsubscriber;
    HashSet<TagsProviderMessage> messages = new();
    
    public ProcessedTag[] ProcessedTags => 
        [.. messages
            .Select(x => x.CurrentTag)];

    public TagsProviderMessage[] Messages => [.. messages];

    public void Subscribe(IObservable<TagsProviderMessage> source)
    {
        unsubscriber = source.Subscribe(this);
    }

    public void OnCompleted()
    {
        unsubscriber?.Dispose();
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(TagsProviderMessage value)
    {
        messages.Add(value);
    }
}