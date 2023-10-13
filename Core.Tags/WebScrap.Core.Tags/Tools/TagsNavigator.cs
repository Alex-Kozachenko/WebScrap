namespace WebScrap.Core.Tags.Tools;

internal static class TagsNavigator
{
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
}