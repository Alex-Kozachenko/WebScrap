using System.Collections.Immutable;
using WebScrap.Tags;
using WebScrap.Tags.Processors;
using WebScrap.Css;

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
        string html, 
        string css)
    {
        var tagFactory = new TagFactory();
        var tagIndexes = CssProcessor.CalculateTagIndexes(tagFactory, html, css);
        var tagRanges = ExtractTagRanges(tagFactory, html, tagIndexes);
        var tagStrings = ExtractStrings(html, tagRanges);
        return tagStrings;
    }

    private static ImmutableArray<Range> ExtractTagRanges(
        TagFactory tagFactory,
        string html,
        IEnumerable<int> tagIndexes) 
        => tagIndexes.Select(tagIndex => 
        {
            var substring = html.Substring(tagIndex);
            var offset = TagsProcessor.GetEntireTagLength(tagFactory, substring);
            return tagIndex..(tagIndex + offset);
        }).ToImmutableArray();

    private static ImmutableArray<string> ExtractStrings(
        string html, 
        IEnumerable<Range> tagRanges) 
        => tagRanges.Select(range => html[range])
            .Select(x => x.ToString())
            .ToImmutableArray();
}