using WebScrap.Core.Tags;
using WebScrap.Core.Tags.Data;

namespace WebScrap.Modules.Extracting.Html.Tables;

internal class TableValuesProcessor : IObserver<ProcessedTag>
{
    private readonly List<Range> currentRowRanges = [];
    private readonly List<Range[]> valuesRanges = [];
    private TagsProvider tagsProvider = new();
    
    internal Range[][] ProcessValues(ReadOnlySpan<char> html)
    {
        valuesRanges.Clear();
        tagsProvider = new TagsProvider();
        tagsProvider.Subscribe(this);
        tagsProvider.Process(html);
        return [..valuesRanges];
    }

    public void OnNext(ProcessedTag tag)
    {
        _ = TryProcessCell(tag) 
            || TryProcessRow(tag.TagInfo);
    }

    public void OnCompleted() { }

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