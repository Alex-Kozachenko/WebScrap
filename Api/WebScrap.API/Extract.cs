using System.Collections.Immutable;
using WebScrap.Core.Tags;
using WebScrap.Css;
using WebScrap.Css.Preprocessing;
using WebScrap.Common;

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
        var tagIndexes = GetTagIndexes(css, html);
        var tagRanges = ExtractTagRanges(html.ToString(), tagIndexes);
        var tagStrings = ExtractStrings(html.ToString(), tagRanges);
        return tagStrings;
    }

    private static List<int> GetTagIndexes(
        ReadOnlySpan<char> css, 
        ReadOnlySpan<char> html)
    {
        var cssTokens = PreprocessingAPI.Process(css);
        var cssProcessor = new CssProcessor(cssTokens);
        cssProcessor.Run(html);
        return cssProcessor.TagIndexes;
    }

    private static ImmutableArray<Range> ExtractTagRanges(
        string html,
        IEnumerable<int> tagIndexes)
        => tagIndexes.Select(tagIndex =>
        {
            var substring = html[tagIndex..];
            var processor = new TagsProcessor();
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