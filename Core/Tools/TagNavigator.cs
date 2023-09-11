namespace Core.Tools;

internal static class TagNavigator
{
    private const int tagCharOffset = 1;

    internal static ReadOnlySpan<char> GoToDeepestTag(
            this ReadOnlySpan<char> html,
            Queue<CssToken> cssTokens)
    {
        var cssLine = string.Concat(cssTokens.Select(x => x.ToString()));
        html = GoToNextTag(html);
        while (true)
        {
            var tag = cssTokens.Dequeue().Css.Span;            
            AssertCurrentTag(html, tag, cssLine);
            if (cssTokens.Count is 0)
            {
                return html;
            }
            html = GoToNextTag(html);
        }
    }

    internal static ReadOnlySpan<char> GrabInnerText(this ReadOnlySpan<char> html)
    {
        html = html[tagCharOffset..];
        var nextTagIndex = html.IndexOf('<') switch
        {
            -1 => html.Length,
            var i => i
        };

        return html[html.IndexOf('>')..nextTagIndex];
    }
        

    private static void AssertCurrentTag(
        this ReadOnlySpan<char> html,
        ReadOnlySpan<char> currentTag,
        ReadOnlySpan<char> cssQuery)
    {
        if (html.StartsWith(currentTag) is not true)
        {
            throw new InvalidOperationException(
                $"Unable to locate a htmltag under current css: {cssQuery}");
        }
    }

    internal static ReadOnlySpan<char> GoToNextTag(this ReadOnlySpan<char> html)
    {
        var nextTagIndex = html.IndexOf('<') switch
        {
            -1 => html.Length - 1,
            var i => i
        };

        return html[tagCharOffset..][nextTagIndex..];
    }
        
}