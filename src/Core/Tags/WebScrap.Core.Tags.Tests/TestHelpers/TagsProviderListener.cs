using WebScrap.Core.Tags.Providing;

namespace WebScrap.Core.Tags.Tests.TestHelpers;

internal class TagsProviderListener : IObserver<TagsProviderMessage>
{
    IDisposable? unsubscriber;
    // HACK: TagsProvider pushes the message from deepest tag.
    Stack<TagsProviderMessage> messages = new();
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
        messages.Push(value);
    }
}