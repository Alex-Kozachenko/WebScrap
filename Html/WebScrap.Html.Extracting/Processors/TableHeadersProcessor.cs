using WebScrap.Core.Tags;

namespace WebScrap.Html.Extracting;

public class TableHeadersProcessor : TagsProcessorBase
{
    private int? lastTagBeginIndex = null;
    private readonly List<Range> headerRanges = [];
    public Range[] HeaderRanges => [..headerRanges];

    protected override void Process(
        UnprocessedTag[] openedTags, 
        UnprocessedTag unprocessedTag)
    {
        if (unprocessedTag.TagInfo.Name == "th")
        {
            lastTagBeginIndex = unprocessedTag.TagOffset;
        }
    }

    protected override void Process(
        UnprocessedTag[] openedTags, 
        ProcessedTag tag) 
    { 
        if (tag.TagInfo.Name == "th" && lastTagBeginIndex.HasValue)
        {
            headerRanges.Add(tag.InnerTextRange);
            lastTagBeginIndex = null;
        }
    }
}