using System.Collections.Immutable;
using WebScrap.Modules.Export.Json;

namespace WebScrap.API.Data;

public readonly ref struct ScrapResult(
    ImmutableArray<Range> tagRanges, 
    ReadOnlySpan<char> html)
{
    private readonly ImmutableArray<Range> tagRanges = tagRanges;
    private readonly ReadOnlySpan<char> html = html;

    public ImmutableArray<string> AsJson()
    {
        var tagStrings = ExtractStrings(html.ToString(), tagRanges);
        return JsonApi.Export(tagStrings)
            .Select(x => x!.ToJsonString())
            .ToImmutableArray();
    }

    public ImmutableArray<string> AsHtml()
        => ExtractStrings(html.ToString(), tagRanges)
            .Select(x => x.ToString())
            .ToImmutableArray();

    private static ImmutableArray<ReadOnlyMemory<char>> ExtractStrings(
        string html,
        IEnumerable<Range> tagRanges)
        => tagRanges
            .Select(range => html[range])
            .Select(x => x.ToString())
            .Select(x => x.Trim())
            .Select(x => x.AsMemory())
            .ToImmutableArray();
}