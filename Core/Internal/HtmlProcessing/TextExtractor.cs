using System.Text;

namespace Core.Internal.HtmlProcessing;

internal static class TextExtractor
{
    internal static ReadOnlySpan<char> ReadBody(this ReadOnlySpan<char> html)
    {
        var sb = new StringBuilder();
        ReadBody(html, sb);
        return sb.ToString();
    }
    
    private static ReadOnlySpan<char> ReadBody(
        ReadOnlySpan<char> html, 
        StringBuilder builder)
        => (html.IndexOf('<'), html.IndexOf('>')) switch
        {
            (_, 0) 
                => ReadBody(html[1..], builder),
            (-1, _) 
                => ReadEnd(html, builder),
            (0, var close) 
                => SkipOpeningTag(html, builder, close),
            (var open, var close)
                when open > close 
                => SkipOpeningTag(html, builder, close),
            (var open, _) 
                => ReadBody(html, builder, open),
        };

    private static ReadOnlySpan<char> SkipOpeningTag(
        ReadOnlySpan<char> html,
        StringBuilder stringBuilder,
        int closingTagIndex)
        => html[closingTagIndex..].ReadBody(stringBuilder);

    private static ReadOnlySpan<char> ReadBody(
        ReadOnlySpan<char> html,
        StringBuilder stringBuilder, 
        int bodyEndIndex)
    {
        stringBuilder.Append(html[..bodyEndIndex]);
        return ReadBody(html[bodyEndIndex..], stringBuilder);
    }

    private static ReadOnlySpan<char> ReadEnd(
        ReadOnlySpan<char> html, 
        StringBuilder stringBuilder)
    {
        stringBuilder.Append(html);
        return html;
    }
}