using WebScrap.Core.Tags;

namespace WebScrap.Html.Extracting;

public class TableValuesProcessor : TagsProcessorBase
{
    private readonly List<Range> currentRowRanges = [];
    private readonly List<Range[]> valuesRanges = [];
    
    public Range[][] ProcessValues(ReadOnlySpan<char> html)
    {
        valuesRanges.Clear();
        Process(html);
        return [..valuesRanges];
    }

    protected override void Process(
        UnprocessedTag[] openedTags, 
        ProcessedTag tag)
    {
        _ = TryProcessCell(tag) 
            || TryProcessRow(tag.TagInfo);
    }

    private bool TryProcessCell(ProcessedTag tag)
    {
        if (tag.TagInfo.Name != "td")
        {
            return false;
        }

        currentRowRanges.Add(tag.InnerTextRange);
        return true;
    }

    private bool TryProcessRow(TagInfo tagInfo)
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