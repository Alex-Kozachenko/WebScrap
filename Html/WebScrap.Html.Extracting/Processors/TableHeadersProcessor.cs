using WebScrap.Core.Tags;

namespace WebScrap.Html.Extracting;

public class TableHeadersProcessor : TagsProcessorBase
{
    private readonly List<Range> headerRanges = [];

    public Range[] ProcessHeaders(ReadOnlySpan<char> html)
    {
        headerRanges.Clear();
        Process(html);
        return [.. headerRanges];
    }

    protected override void Process(
        UnprocessedTag[] openedTags, 
        ProcessedTag tag) 
    { 
        if (tag.TagInfo.Name == "th")
        {
            headerRanges.Add(tag.InnerTextRange);
        }
    }
}