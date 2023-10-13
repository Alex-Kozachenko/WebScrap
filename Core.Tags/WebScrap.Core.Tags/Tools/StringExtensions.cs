namespace WebScrap.Core.Tags.Tools;

internal static class StringExtensions
{
    internal static ReadOnlySpan<char> Clip(
        this ReadOnlySpan<char> html,
        ReadOnlySpan<char> beginAny,
        ReadOnlySpan<char> endAny,
        bool strip = false)
    {
        var begin = html.IndexOfAny(beginAny);
        var end = begin + html[begin..][1..].IndexOfAny(endAny) + 2;

        var result = html[begin..end];
        return strip ? result[1..^1] : result;
    }
}