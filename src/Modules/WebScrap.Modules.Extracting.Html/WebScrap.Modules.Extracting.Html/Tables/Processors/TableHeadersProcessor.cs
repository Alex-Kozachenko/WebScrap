using WebScrap.Core.Tags;
using WebScrap.Core.Tags.Providing;

namespace WebScrap.Modules.Extracting.Html.Tables;

internal class TableHeadersProcessor : IObserver<TagsProviderMessage>
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

    public void OnNext(TagsProviderMessage message)
    {
        var tag = message.CurrentTag;

        if (tag.TagInfo.Name == "th")
        {
            headerRanges.Add(tag.InnerTextRange);
        }
    }

    public void OnCompleted() { }

    public void OnError(Exception error) { }
}