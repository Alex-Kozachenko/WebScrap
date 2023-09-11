namespace Core.Tools;

internal static class TagNavigator
{
    private const int tagNameOffset = 1;
    internal static ReadOnlySpan<char> GoToDeepestTag(
            ReadOnlySpan<char> html,
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

    private static void AssertCurrentTag(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> currentTag,
        ReadOnlySpan<char> cssQuery)
    {
        if (html[tagNameOffset..].StartsWith(currentTag) is not true)
        {
            throw new InvalidOperationException(
                $"Unable to locate a htmltag under current css: {cssQuery}");
        }
    }

    private static ReadOnlySpan<char> GoToNextTag(ReadOnlySpan<char> html)
        => html[tagNameOffset..][html.IndexOf('<')..];
}