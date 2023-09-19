using System.Text;
using static Core.Html.Tools.HtmlValidator;
using static Core.Html.Tools.TagsNavigator;
using static Core.Html.Reading.Tags.HtmlTagExtractor;

namespace Core.Html.Reading.Text;

internal static class HtmlTextProcessor
{
    public static ReadOnlySpan<char> Process(ReadOnlySpan<char> html)
    {
        html = ToValidHtml(html);
        html = ExtractEntireTag(html);
        var sb = new StringBuilder();
        Process(html, sb);
        return sb.ToString();
    }   

    private static ReadOnlySpan<char> Process(
        ReadOnlySpan<char> html, 
        StringBuilder builder)
        => GetTagRange(html) switch
        {
            (_, 0) 
                => Process(html[1..], builder),
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

    private static ReadOnlySpan<char> SkipOpeningTag(
        ReadOnlySpan<char> html,
        StringBuilder stringBuilder,
        int closingTagIndex)
        => Process(html[closingTagIndex..], stringBuilder);

    private static ReadOnlySpan<char> ReadBody(
        ReadOnlySpan<char> html,
        StringBuilder stringBuilder, 
        int bodyEndIndex)
    {
        stringBuilder.Append(html[..bodyEndIndex]);
        return Process(html[bodyEndIndex..], stringBuilder);
    }

    private static ReadOnlySpan<char> ReadEnd(
        ReadOnlySpan<char> html, 
        StringBuilder stringBuilder)
    {
        stringBuilder.Append(html);
        return html;
    }    
}