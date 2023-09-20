using Core.Css.Tools;
using static Core.Css.Tools.CssTokenizer;
using static Core.Html.Tools.HtmlValidator;
using static Core.Html.Tools.TagsNavigator;
using static Core.Html.Reading.Tags.HtmlTagReader;
using static Core.Html.Reading.Tags.HtmlTagExtractor;
using System.Collections.Immutable;

namespace Core;

// NOTE: it seems a tiny tags locator became a center of the project
// TODO: decouple this.
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
        var cssTokens = TokenizeCss(css);

        List<Range> result = [];

        // TODO: refactor.
        Stack<ArraySegment<char>> openedSuitableTags = [];
        // TODO: refactor.
        int processed = 0;
        var originalRange = ..html.Length;

        while (html.Length is not 0)
        {
            // HACK: legit hell on earth.
            var cssTag = openedSuitableTags.Count < cssTokens.Length
                ? cssTokens[openedSuitableTags.Count]
                : new CssToken();

            var htmlTag = ReadHtmlTag(html);
            if (htmlTag.Name.StartsWith(cssTag.Css.Span))
            {
                if (htmlTag.IsOpening)
                {
                    openedSuitableTags.Push(htmlTag.Name.ToArray());
                    if (openedSuitableTags.Count == cssTokens.Length)
                    {
                        var bodyLength = GetTagLength(html);
                        var bodyRange = processed..(processed + bodyLength);
                        result.Add(bodyRange);
                    }
                }
                else
                {
                    if (openedSuitableTags.Peek().Array!.SequenceEqual(
                        htmlTag.Name.ToArray()))
                    {
                        openedSuitableTags.Pop();
                    }
                }
            }

            var nextTagIndex = GetNextTagIndex(html[1..]) + 1;
            processed += html[..nextTagIndex].Length;
            html = html[nextTagIndex..];
        }

        return [.. result];
    }
}