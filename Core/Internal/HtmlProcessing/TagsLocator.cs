using Core.Internal.HtmlProcessing.Extractors;

namespace Core.Internal.HtmlProcessing;

internal static class TagsLocator
{
    public static List<ArraySegment<char>> LocateTagsByCss(
        ReadOnlySpan<char> container,
        ReadOnlySpan<char> css)
    {
        var result = new List<ArraySegment<char>>();
        container = container.TrimStart();
        var cssTokens = CssTokenizer.Default.TokenizeCss(css);
        while (true)
        {
            var tag = cssTokens.Dequeue().Css.Span;  
            AssertCurrentTag(container[1..], tag, css);
            if (cssTokens.Count is 0)
            {
                // HACK:
                var body = container.ReadBody().ToArray();
                var arraySegment = new ArraySegment<char>(body);
                result.Add(arraySegment);
                return result;
            }
            
            var nextTagIndex = GetNextTagIndex(container[1..]) + 1;
            container = container[nextTagIndex..];
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
        ReadOnlySpan<char> currentTag,
        ReadOnlySpan<char> css)
    {
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