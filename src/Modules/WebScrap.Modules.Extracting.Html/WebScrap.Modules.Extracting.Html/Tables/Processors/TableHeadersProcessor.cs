using WebScrap.Core.Tags;
using WebScrap.Core.Tags.Data;

namespace WebScrap.Modules.Extracting.Html.Tables;

internal class TableHeadersProcessor : TagsProcessorBase
{
    private readonly List<Range> headerRanges = [];

    internal Range[] ProcessHeaders(ReadOnlySpan<char> html)
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