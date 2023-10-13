using System.Collections.Immutable;
using WebScrap.Core.Tags;
using WebScrap.Css.API;

namespace WebScrap.API;

public static class Extract
{
    /// <summary>
    /// Processes the html and returns css-compliant tags.
    /// </summary>
    public static ImmutableArray<string> Html(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> css)
    {
        html = html.TrimStart(' ');
        var tagRanges = CssAPI.GetTagRanges(css, html);
        var tagStrings = ExtractStrings(html.ToString(), tagRanges);
        return tagStrings;
    }

    private static ImmutableArray<string> ExtractStrings(
        string html,
        IEnumerable<Range> tagRanges)
        => tagRanges
            .Select(range => html[range])
            .Select(x => x.ToString())
            .Select(x => x.Trim())
            .ToImmutableArray();
}