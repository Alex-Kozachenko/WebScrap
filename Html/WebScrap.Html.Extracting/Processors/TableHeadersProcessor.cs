using WebScrap.Common.Tags;
using WebScrap.Core.Tags;

namespace WebScrap.Html.Extracting;

public class TableHeadersProcessor : TagsProcessorBase
{
    private int? lastTagBeginIndex = null;
    private readonly List<Range> headerRanges = [];
    public Range[] HeaderRanges => [..headerRanges];
    protected override void Process(OpeningTag tag)
    {
        base.Process(tag);

        if (tag.Name == "th")
        {
            lastTagBeginIndex = CharsProcessed;
        }
    }

    protected override void Process(ClosingTag tag)
    {
        base.Process(tag);

        if (tag.Name == "th" && lastTagBeginIndex.HasValue)
        {
            headerRanges.Add(lastTagBeginIndex.Value..CharsProcessed);
            lastTagBeginIndex = null;
        }
    }
}