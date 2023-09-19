namespace Core.Html.Tools;

internal static class TagsNavigator
{
    internal static int GetNextTagIndex(ReadOnlySpan<char> html)
        => html.IndexOf('<') switch
        {
            -1 => html.Length,
            var nextTagIndex => nextTagIndex
        };
}