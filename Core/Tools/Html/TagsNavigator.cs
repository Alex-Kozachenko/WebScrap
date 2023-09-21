namespace Core.Tools.Html;

internal static class TagsNavigator
{
    internal static int GetNextTagIndexSkipCurrent(ReadOnlySpan<char> html)
        => html.IndexOf('<') switch
        {
            0 => GetNextTagIndex(html[1..]) +1,
            _ => GetNextTagIndex(html)
        };

    internal static int GetNextTagIndex(ReadOnlySpan<char> html)
        => html.IndexOf('<') switch
        {
            -1 => html.Length,
            var nextTagIndex => nextTagIndex
        };

    internal static int GetInnerTextIndex(ReadOnlySpan<char> html)
        => html.IndexOf('<') switch
        {
            0 => GetInnerTextIndex(html[1..]) + 1,
            _ => html.IndexOf('>') + 1
        };

    internal static (int Begin, int End) GetTagRange(ReadOnlySpan<char> html)
        => (html.IndexOf('<'), html.IndexOf('>'));
}