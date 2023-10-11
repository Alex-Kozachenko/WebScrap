using WebScrap.Common.Tags;
using WebScrap.Core.Tags;

namespace WebScrap.Html.Extracting;

public class TableValuesProcessor : TagsProcessorBase
{
    private int? lastTdTagBeginIndex = null;
    private readonly List<Range> currentRowRanges = [];
    private readonly List<Range[]> valuesRanges = [];
    public Range[][] ValuesRanges => [..valuesRanges];
    protected override void Process(OpeningTag tag)
    {
        base.Process(tag);

        if (tag.Name == "td")
        {
            lastTdTagBeginIndex = CharsProcessed;
        }
    }

    protected override void Process(ClosingTag tag)
    {
        base.Process(tag);

        if (tag.Name == "td" 
            && lastTdTagBeginIndex.HasValue)
        {
            currentRowRanges.Add(lastTdTagBeginIndex.Value..CharsProcessed);
            lastTdTagBeginIndex = null;
        }

        if (tag.Name == "tr" && currentRowRanges.Count != 0)
        {
            valuesRanges.Add([.. currentRowRanges]);
            currentRowRanges.Clear();
        }
    }
}