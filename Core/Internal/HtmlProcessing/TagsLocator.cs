using Core.Internal.HtmlProcessing.Extractors;

namespace Core.Internal.HtmlProcessing;

internal static class TagsLocator
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
        html = HtmlValidator.ToValidHtml(html);
        List<ArraySegment<char>> result = [];
        var cssTokens = CssTokenizer.TokenizeCss(css);
        Stack<ArraySegment<char>> openedSuitableTags = [];

        while (html.Length is not 0)
        {
            // HACK: legit hell on earth.
            var cssTag = openedSuitableTags.Count < cssTokens.Length
                ? cssTokens[openedSuitableTags.Count]
                : new CssToken();

            var htmlTag = HtmlTagReader.ReadHtmlTag(html);
            if (htmlTag.Name.StartsWith(cssTag.Css.Span))
            {
                if (htmlTag.IsOpening)
                {
                    openedSuitableTags.Push(htmlTag.Name.ToArray());
                    if (openedSuitableTags.Count == cssTokens.Length)
                    {
                        // HACK: redundant copying invoked.
                        var body = html.ReadBody().ToArray();
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

    public static int GetNextTagIndex(ReadOnlySpan<char> html)
        => html.IndexOf('<') switch
        {
            -1 => html.Length,
            var nextTagIndex => nextTagIndex
        };
}