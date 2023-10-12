using WebScrap.Core.Tags;

namespace WebScrap.Html.Extracting;

public class TableValuesProcessor : TagsProcessorBase
{
    private int? lastTdTagBeginIndex = null;
    private readonly List<Range> currentRowRanges = [];
    private readonly List<Range[]> valuesRanges = [];
    public Range[][] ValuesRanges => [..valuesRanges];

    protected override void Process(
        UnprocessedTag[] openedTags, 
        UnprocessedTag unprocessedTag)
    {
        if (unprocessedTag.TagInfo.Name == "td")
        {
            lastTdTagBeginIndex = unprocessedTag.TagOffset;
        }
    }

    protected override void Process(
        UnprocessedTag[] openedTags, 
        ProcessedTag tag)
    {
        if (tag.TagInfo.Name == "td" 
            && lastTdTagBeginIndex.HasValue)
        {
            currentRowRanges.Add(tag.InnerTextRange);
            lastTdTagBeginIndex = null;
        }

        if (tag.TagInfo.Name == "tr" && currentRowRanges.Count != 0)
        {
            valuesRanges.Add([.. currentRowRanges]);
            currentRowRanges.Clear();
        }
    }
}