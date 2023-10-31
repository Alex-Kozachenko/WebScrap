using WebScrap.Core.Tags;
using WebScrap.Core.Tags.Data;

namespace WebScrap.Modules.Extracting.Html.Tables;

internal class TableHeadersProcessor : IObserver<ProcessedTag>
{
    private readonly List<Range> headerRanges = [];
    private TagsProvider tagsProvider = new();

    internal Range[] ProcessHeaders(ReadOnlySpan<char> html)
    {
        headerRanges.Clear();
        tagsProvider = new TagsProvider();
        tagsProvider.Subscribe(this);
        tagsProvider.Process(html);
        return [.. headerRanges];
    }

    public void OnNext(ProcessedTag tag)
    {
        if (tag.TagInfo.Name == "th")
        {
            headerRanges.Add(tag.InnerTextRange);
        }
    }

    public void OnCompleted() { }

    public void OnError(Exception error) { }
}