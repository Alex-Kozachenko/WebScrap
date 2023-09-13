namespace Core.Internal.HtmlProcessing;

internal static class TagNavigator
{
    internal static ReadOnlySpan<char> GoToTagByCss(
            this ReadOnlySpan<char> html,
            ReadOnlyMemory<char> css)
    {
        var cssTokens = CssTokenizer.Default.TokenizeCss(css);
        while (true)
        {
            html = GoToNextTag(html);
            var tag = cssTokens.Dequeue().Css.Span;  
            AssertCurrentTag(html[1..], tag, css);
            if (cssTokens.Count is 0)
            {
                return html;
            }
        }
    }

    internal static ReadOnlySpan<char> GoToNextTag(this ReadOnlySpan<char> html)
    {
        const int tagCharOffset = 1; // in case the cursor is standing on <
        var nextTagIndex = html[tagCharOffset..].IndexOf('<');
        return nextTagIndex switch
        {
            -1 => html[..^1],
            _ => html[tagCharOffset..][nextTagIndex..],
        };
    }

    private static void AssertCurrentTag(
        this ReadOnlySpan<char> html,
        ReadOnlySpan<char> currentTag,
        ReadOnlyMemory<char> css)
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

    private static string ToCss(IEnumerable<CssToken> cssTokens)
        => string.Concat(cssTokens.Select(x => x.ToString()));
}