using WebScrap.Common.Tags;
using WebScrap.Core.Tags;
using WebScrap.Core.Tags.Data;

namespace WebScrap.Html.Extracting;

public class TableValuesProcessor : TagsProcessorBase
{
    private int? lastTdTagBeginIndex = null;
    private readonly List<Range> currentRowRanges = [];
    private readonly List<Range[]> valuesRanges = [];
    public Range[][] ValuesRanges => [..valuesRanges];
    protected override void Process(OpeningTag tag, TagsHistoryRecord tagsHistoryRecord)
    {
        base.Process(tag, tagsHistoryRecord);

        if (tag.Name == "td")
        {
            lastTdTagBeginIndex = tagsHistoryRecord.TagOffset;
        }
    }

    protected override void Process(
        OpeningTag openingTag, 
        ClosingTag closingTag, 
        TagRanges tagRanges)
    {
        base.Process(openingTag, closingTag, tagRanges);

        if (closingTag.Name == "td" 
            && lastTdTagBeginIndex.HasValue)
        {
            currentRowRanges.Add(tagRanges.InnerTextRange);
            lastTdTagBeginIndex = null;
        }

        if (closingTag.Name == "tr" && currentRowRanges.Count != 0)
        {
            valuesRanges.Add([.. currentRowRanges]);
            currentRowRanges.Clear();
        }
    }
}