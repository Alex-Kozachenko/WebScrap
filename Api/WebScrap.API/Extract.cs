using System.Collections.Immutable;
using WebScrap.Core.Tags;
using WebScrap.Css.API;

namespace WebScrap.API;

public static class Extract
{
    /// <summary>
    /// Detect tags suitable for css parameter.
    /// </summary>
    /// <param name="html"></param>
    /// <param name="css"></param>
    /// <returns></returns>
    public static ImmutableArray<string> Html(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> css)
    {
        html = html.TrimStart(' ');
        var tagIndexes = CssAPI.GetTagIndexes(css, html);
        var tagRanges = ExtractTagRanges(html.ToString(), tagIndexes);
        var tagStrings = ExtractStrings(html.ToString(), tagRanges);
        return tagStrings;
    }

    private static ImmutableArray<Range> ExtractTagRanges(
        string html,
        IEnumerable<int> tagIndexes)
        => tagIndexes.Select(tagIndex =>
        {
            var substring = html[tagIndex..];
            var processor = new TagsProcessorBase();
            processor.Run(substring);
            var offset = processor.CharsProcessed;
            return tagIndex..(tagIndex + offset);
        }).ToImmutableArray();

    private static ImmutableArray<string> ExtractStrings(
        string html,
        IEnumerable<Range> tagRanges)
        => tagRanges
            .Select(range => html[range])
            .Select(x => x.ToString())
            .Select(x => x.Trim())
            .ToImmutableArray();
}