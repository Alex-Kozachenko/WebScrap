using Core.Css.Tools;
using static Core.Css.Tools.CssTokenizer;
using static Core.Html.Tools.HtmlValidator;
using static Core.Html.Tools.TagsNavigator;
using static Core.Html.Reading.Text.HtmlTextProcessor;
using static Core.Html.Reading.Tags.HtmlTagReader;

namespace Core;

// NOTE: it seems a tiny tags locator became a center of the project
// TODO: decouple this.
public static class TagsLocator
{
    /// <summary>
    /// Locates the html-tags by css-like string.
    /// </summary>
    /// <returns>
    /// Plain html (inner text) for tags which has been found by the css-like selector.
    /// </returns>
    public static List<ArraySegment<char>> LocateTagsByCss(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> css)
    {
        html = ToValidHtml(html);
        List<ArraySegment<char>> result = [];
        var cssTokens = TokenizeCss(css);
        Stack<ArraySegment<char>> openedSuitableTags = [];

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
                        // HACK: redundant copying invoked.
                        var body = Process(html).ToArray();
                        var arraySegment = new ArraySegment<char>(body);
                        result.Add(arraySegment);
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
            html = html[nextTagIndex..];
        }

        return result;
    }
}