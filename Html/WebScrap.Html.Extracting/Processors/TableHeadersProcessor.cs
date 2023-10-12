using WebScrap.Common.Tags;
using WebScrap.Core.Tags;
using WebScrap.Core.Tags.Data;

namespace WebScrap.Html.Extracting;

public class TableHeadersProcessor : TagsProcessorBase
{
    private int? lastTagBeginIndex = null;
    private readonly List<Range> headerRanges = [];
    public Range[] HeaderRanges => [..headerRanges];
    protected override void Process(OpeningTag tag, TagsHistoryRecord tagsHistoryRecord)
    {
        base.Process(tag, tagsHistoryRecord);

        if (tag.Name == "th")
        {
            lastTagBeginIndex = tagsHistoryRecord.TagOffset;
        }
    }

    protected override void Process(
        OpeningTag openingTag, 
        ClosingTag closingTag, 
        TagRanges tagRanges)
    {
        base.Process(openingTag, closingTag, tagRanges);

        if (closingTag.Name == "th" && lastTagBeginIndex.HasValue)
        {
            headerRanges.Add(lastTagBeginIndex.Value..tagRanges.Range.End.Value);
            lastTagBeginIndex = null;
        }
    }
}