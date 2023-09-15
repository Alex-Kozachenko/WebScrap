using System.Text;

namespace Core.Internal.HtmlProcessing.Extractors;

internal static class TextExtractor
{
    public static ReadOnlySpan<char> ReadBody(this ReadOnlySpan<char> html)
    {
        AssertHtmlStart(html);  
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
            (0, var close) 
                => SkipOpeningTag(html, builder, close),
            (var open, var close)
                when open > close 
                => SkipOpeningTag(html, builder, close),
            (var open, _) 
                => ReadBody(html, builder, open),
        };

    private static (int Begin, int End) GetTagRange(this ReadOnlySpan<char> html)
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

    private static void AssertHtmlStart(ReadOnlySpan<char> html)
    {
        if (html.GetTagRange().Begin is not 0
            || char.IsLetter(html[1]) is not true)
        {
            var message = "Html should start with opening html-tag.";
            throw new InvalidOperationException(message);
        }
    }
}