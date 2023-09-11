using System.Text;

namespace Core.Tools;

internal static class TextExtractor
{
    internal static string Extract(ReadOnlySpan<char> html)
    {
        var result = new StringBuilder();
        while (html.IsEmpty is not true)
        {
            var innerText = GetNextInnerText(html);
            result.Append(innerText);
            html = html[innerText.Length..];
            html = SkipTag(html);
        }
        return result.ToString();
    }

    internal static ReadOnlySpan<char> GetNextInnerText(
            ReadOnlySpan<char> html)
        => html[..html.IndexOf('<')];
    
    private static ReadOnlySpan<char> SkipTag(ReadOnlySpan<char> html)
        => html[(html.IndexOf('>') + 1)..];
}