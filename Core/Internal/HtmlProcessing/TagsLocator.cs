using Core.Internal.HtmlProcessing.Extractors;
using static Core.Internal.HtmlProcessing.HtmlBlockValidator;
using static Core.Internal.HtmlProcessing.CssTokenizer;

namespace Core.Internal.HtmlProcessing;

internal static class TagsLocator
{
    public static List<ArraySegment<char>> LocateTagsByCss(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> css)
    {
        html = ToValidHtml(html);
        var result = new List<ArraySegment<char>>();
        var cssTokens = TokenizeCss(css);
        while (true)
        {
            var tag = cssTokens.Dequeue();  
            AssertCurrentTag(html[1..], tag, css);
            if (cssTokens.Count is 0)
            {
                // HACK: redundant copying invoked.
                var body = html.ReadBody().ToArray();
                var arraySegment = new ArraySegment<char>(body);
                result.Add(arraySegment);
                return result;
            }
            
            var nextTagIndex = GetNextTagIndex(html[1..]) + 1;
            html = html[nextTagIndex..];
        }
    }

    public static int GetNextTagIndex(ReadOnlySpan<char> html)
        => html.IndexOf('<') switch
        {
            -1 => html.Length,
            var nextTagIndex => nextTagIndex
        };

    private static void AssertCurrentTag(
        ReadOnlySpan<char> html,
        CssToken cssToken,
        ReadOnlySpan<char> css)
    {
        var currentTag = cssToken.Css.Span;
        if (html.StartsWith(currentTag) is not true)
        {
            throw new InvalidOperationException(
                $"""
                    Unable to locate a tag:"{currentTag}"
                    under for css: "{css}"
                    html: {html}
                """);
        }
    }
}