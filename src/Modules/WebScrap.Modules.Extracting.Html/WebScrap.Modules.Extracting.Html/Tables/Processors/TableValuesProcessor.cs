using WebScrap.Core.Tags;
using WebScrap.Core.Tags.Data;

namespace WebScrap.Modules.Extracting.Html.Tables;

internal class TableValuesProcessor : IObserver<TagsProviderMessage>
{
    private readonly List<Range> currentRowRanges = [];
    private readonly List<Range[]> valuesRanges = [];
    private IDisposable? unsubscriber;
    
    internal Range[][] ProcessValues(ReadOnlySpan<char> html)
    {
        valuesRanges.Clear();
        var tagsProvider = new TagsProvider();
        unsubscriber = tagsProvider.Subscribe(this);
        tagsProvider.Process(html);
        return [..valuesRanges];
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