namespace WebScrap.Tools.Html;

public static class TagsNavigator
{
    public static int GetNextTagIndexSkipCurrent(ReadOnlySpan<char> html)
        => html.IndexOf('<') switch
        {
            0 => GetNextTagIndex(html[1..]) +1,
            _ => GetNextTagIndex(html)
        };

    public static int GetNextTagIndex(ReadOnlySpan<char> html)
        => html.IndexOf('<') switch
        {
            -1 => html.Length,
            var nextTagIndex => nextTagIndex
        };

    public static int GetInnerTextIndex(ReadOnlySpan<char> html)
        => html.IndexOf('<') switch
        {
            0 => GetInnerTextIndex(html[1..]) + 1,
            _ => html.IndexOf('>') + 1
        };
}