using System.Collections.Immutable;
using WebScrap.Core.Tags.Data;
using WebScrap.Core.Tags.Messaging;

namespace WebScrap.Modules.Extracting.Html.Tables;

internal class TableValuesProcessor : IObserver<TagsProviderMessage>
{
    private readonly List<Range> currentRowRanges = [];
    private readonly List<Range[]> valuesRanges = [];
    private IDisposable? unsubscriber;

    public ImmutableArray<ImmutableArray<Range>> ValuesRanges => [..valuesRanges.Select(x => x.ToImmutableArray())];
    
    public void Subscribe(ITagObservable tagObservable)
    {
        unsubscriber = tagObservable.Subscribe(this, "tr", "td");
    }

    public void OnNext(TagsProviderMessage message)
    {
        _ = TryProcessCell(message.CurrentTag) 
            || TryProcessRow(message.CurrentTag.TagInfo);
    }

    public void OnCompleted() 
    {
        unsubscriber?.Dispose();
    }

    public void OnError(Exception error) { }

    bool TryProcessCell(ProcessedTag tag)
    {
        if (tag.TagInfo.Name != "td")
        {
            return false;
        }

        currentRowRanges.Add(tag.InnerTextRange);
        return true;
    }

    bool TryProcessRow(TagInfo tagInfo)
    {
        var isCurrentRowFilled = currentRowRanges.Count != 0;
        if (tagInfo.Name != "tr" || isCurrentRowFilled is false)
        {
            return false;
        }

        valuesRanges.Add([.. currentRowRanges]);
        currentRowRanges.Clear();
        return true;
    }
}