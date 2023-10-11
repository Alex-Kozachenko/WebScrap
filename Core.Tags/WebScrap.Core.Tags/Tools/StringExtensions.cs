namespace WebScrap.Core.Tags.Tools;

internal static class StringExtensions
{
    internal static ReadOnlySpan<char> Clip(
        this ReadOnlySpan<char> html,
        ReadOnlySpan<char> beginAny,
        ReadOnlySpan<char> endAny)
    {
        var begin = html.IndexOfAny(beginAny) + 1;
        var end = begin + html[begin..].IndexOfAny(endAny);
        return html[begin..end];
    }
}