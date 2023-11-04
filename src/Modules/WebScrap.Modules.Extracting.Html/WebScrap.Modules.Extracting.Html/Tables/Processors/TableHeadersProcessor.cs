using WebScrap.Core.Tags;
using WebScrap.Core.Tags.Data;

namespace WebScrap.Modules.Extracting.Html.Tables;

internal class TableHeadersProcessor : IObserver<TagsProviderMessage>
{
    private readonly List<Range> headerRanges = [];
    private IDisposable? unsubscriber;

    internal Range[] ProcessHeaders(ReadOnlySpan<char> html)
    {
        headerRanges.Clear();
        var tagsProvider = new TagsProvider();
        unsubscriber = tagsProvider.Subscribe(this);
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

    public void OnCompleted() 
    {
        unsubscriber?.Dispose();
    }

    public void OnError(Exception error) { }
}