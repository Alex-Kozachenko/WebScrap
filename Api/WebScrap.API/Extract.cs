using System.Collections.Immutable;
using WebScrap.API.Contracts;
using WebScrap.Css.API;
using WebScrap.Modules.Export.Json;

namespace WebScrap.API;

public class Extract(Config config = default)
{
    /// <summary>
    /// Processes the html and returns css-compliant tags.
    /// </summary>
    public ImmutableArray<string> Html(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> css)
    {
        html = html.TrimStart(' ');
        var tagRanges = CssAPI.GetTagRanges(css, html);
        var tagStrings = ExtractStrings(html.ToString(), tagRanges);
        if (config.OutputFormatType == OutputFormatType.Json)
        {
            tagStrings = JsonApi.Export(tagStrings)
                .Select(x => x!.ToJsonString())
                .Select(x => x.AsMemory())
                .ToImmutableArray();
        }
        return tagStrings
            .Select(x => x.ToString())
            .ToImmutableArray();
    }

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