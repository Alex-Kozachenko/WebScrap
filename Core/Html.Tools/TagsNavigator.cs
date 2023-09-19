namespace Core.Html.Tools;

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
            0 => GetInnerTextIndex(html[1..]) + 1, // openingTagOffset 
            _ => html.IndexOf('>') + 1 // closingTagOffset
        };

    internal static (int Begin, int End) GetTagRange(ReadOnlySpan<char> html)
        => (html.IndexOf('<'), html.IndexOf('>'));
}