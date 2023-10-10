using System.Collections.Immutable;
using WebScrap.Tags;
using WebScrap.Css;
using WebScrap.Common.Contracts;
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
        var cssTokens = PreprocessingAPI.Process(css);
        var cssProcessor = new CssProcessor(cssTokens);
        cssProcessor.Run(html);
        var tagIndexes = cssProcessor.TagIndexes;
        var tagRanges = ExtractTagRanges(cssProcessor, html.ToString(), tagIndexes);
        var tagStrings = ExtractStrings(html.ToString(), tagRanges);
        return tagStrings;
    }

    private static ImmutableArray<Range> ExtractTagRanges(
        ProcessorBase processor,
        string html,
        IEnumerable<int> tagIndexes)
        => tagIndexes.Select(tagIndex =>
        {
            var substring = html[tagIndex..];
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