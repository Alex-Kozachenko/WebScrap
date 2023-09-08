namespace Core.Tools;

internal static class TagNavigator
{
    internal static ReadOnlySpan<char> GoToDeepestTag(
            ReadOnlySpan<char> htmlSpan,
            Queue<CssToken> cssTokens)
    {
        while (cssTokens.Count is not 0)
        {
            var currentCssTag = cssTokens.Dequeue().Css.Span;
            if (!CheckHtmlStartsWithTag(htmlSpan, currentCssTag))
            {
                return string.Empty;
            }

            htmlSpan = htmlSpan[GetIndexOfNextTagName(htmlSpan)..];
        }

        return htmlSpan;
    }

    internal static int GetIndexOfNextTagName(ReadOnlySpan<char> html)
        => html.IndexOf('<') + 1;

    internal static ReadOnlySpan<char> GetNextInnerText(
            ReadOnlySpan<char> html)
        => html[(html.IndexOf('>') + 1)..html.IndexOf('<')];

    private static bool CheckHtmlStartsWithTag(
            ReadOnlySpan<char> html,
            ReadOnlySpan<char> tagName)
        => html.Slice(GetIndexOfNextTagName(html), tagName.Length)
               .SequenceEqual(tagName);
}