using static Core.Html.Tools.HtmlValidator;
using System.Collections.Immutable;
using Core.Processors;

namespace Core;

public static class TagsLocator
{
    /// <summary>
    /// Locates the html-tags by css-like string.
    /// </summary>
    /// <returns>
    /// Ranges for tags.
    /// </returns>
    public static ImmutableArray<Range> LocateTagRanges(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> css)
    {
        html = ToValidHtml(html);
        var processor = new CssProcessor(css, html.Length);
        processor.Run(html);
        return [.. processor.Ranges];
    }
}