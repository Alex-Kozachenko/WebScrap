using System.Collections.Immutable;
using WebScrap.Core.Tags;
using WebScrap.Core.Tags.Data;
using WebScrap.Core.Tags.Messaging;

namespace WebScrap.Modules.Extracting.Html.Tables;

internal class TableHeadersProcessor : IObserver<TagsProviderMessage>
{
    private readonly List<Range> headerRanges = [];
    private IDisposable? unsubscriber;

    public ImmutableArray<Range> HeaderRanges => [.. headerRanges];

    public void Subscribe(ITagObservable tagObservable)
    {
        unsubscriber = tagObservable.Subscribe(this, "th");
    }

    public void OnNext(TagsProviderMessage message)
    {
        var th = message.CurrentTag;
        headerRanges.Add(th.InnerTextRange);
    }

    public void OnCompleted() 
    {
        unsubscriber?.Dispose();
    }

    public void OnError(Exception error) { }
}