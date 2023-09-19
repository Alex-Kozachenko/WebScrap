using static Core.Html.Tools.HtmlValidator;

namespace Core.Html.Reading.Tags;

/// <summary>
/// Produces shallow <see cref="HtmlTag"/>= object.
/// </summary>
internal class HtmlTagReader
{
    public static HtmlTag ReadHtmlTag(
        ReadOnlySpan<char> html)
    {
        html = ToValidHtml(html);
        _ = TryProcessClosingTagName(html, out var htmlTag)
            || TryProcessOpeningTagName(html, out htmlTag);
            
        if (htmlTag.Name.Length is 0)
        {
            throw new ArgumentException($"html doesn't start with tag! {html}");
        }
        
        return htmlTag;
    } 

    private static bool TryProcessOpeningTagName(
        ReadOnlySpan<char> html,
        out HtmlTag result)
    {
        ReadOnlySpan<char> openingBrackets = "<";
        var delimeterIndex = html.IndexOfAny(' ', '>');
        if (html.StartsWith(openingBrackets) is not true)
        {
            result = new();
            return false;
        }

        result = new(
            name: html[openingBrackets.Length..delimeterIndex],
            isOpening: true);
        return true;
    }

    private static bool TryProcessClosingTagName(
        ReadOnlySpan<char> html,
        out HtmlTag htmlTag)
    {
        ReadOnlySpan<char> closingBrackets = "</";
        var delimeterIndex = html.IndexOf('>');
        if (html.StartsWith(closingBrackets) is not true)
        {
            htmlTag = new();
            return false;
        }

        htmlTag = new(
            name: html[closingBrackets.Length..delimeterIndex], 
            isOpening: false);

        return true;
    }
}