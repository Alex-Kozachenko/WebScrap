using System.Collections.Immutable;
using WebScrap.Tags.Processors;
using WebScrap.Css;

namespace WebScrap;

public static class API
{
    /// <summary>
    /// Detect tags suitable for css parameter.
    /// </summary>
    /// <param name="html"></param>
    /// <param name="css"></param>
    /// <returns></returns>
    public static ImmutableArray<string> ExtractHtml(
        string html, 
        string css)
        => CssProcessor.CalculateTagIndexes(html, css)
            // Extract the ranges of detected tags.
            .Select(tagIndex => new Range(
                tagIndex,
                tagIndex + TagsProcessor.GetEntireTagLength(html.Substring(tagIndex))))
            // Return the actual strings from html.
            .Select(range => html[range])
            .Select(x => x.ToString())
            .ToImmutableArray();
}