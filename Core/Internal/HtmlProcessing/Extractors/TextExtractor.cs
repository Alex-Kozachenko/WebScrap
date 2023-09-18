using System.Text;

namespace Core.Internal.HtmlProcessing.Extractors;

internal static class TextExtractor
{
    public static ReadOnlySpan<char> ReadBody(this ReadOnlySpan<char> html)
    {
        html = HtmlValidator.ToValidHtml(html);
        html = HtmlTagExtractor.ExtractTag(html);    
        var sb = new StringBuilder();
        ReadBody(html, sb);
        return sb.ToString();
    }   

    private static ReadOnlySpan<char> ReadBody(
        ReadOnlySpan<char> html, 
        StringBuilder builder)
        => html.GetTagRange() switch
        {
            (_, 0) 
                => ReadBody(html[1..], builder),
            (-1, _) 
                => ReadEnd(html, builder),
            (0, var end) 
                => SkipOpeningTag(html, builder, end),
            (var begin, var end)
                when begin > end 
                => SkipOpeningTag(html, builder, end),
            (var begin, _) 
                => ReadBody(html, builder, begin),
        };

    private static (int, int) GetTagRange(this ReadOnlySpan<char> html)
        => (html.IndexOf('<'), html.IndexOf('>'));

    private static ReadOnlySpan<char> SkipOpeningTag(
        ReadOnlySpan<char> html,
        StringBuilder stringBuilder,
        int closingTagIndex)
        => ReadBody(html[closingTagIndex..], stringBuilder);

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