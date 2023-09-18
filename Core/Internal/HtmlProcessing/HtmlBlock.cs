namespace Core.Internal.HtmlProcessing;

internal static class HtmlBlockValidator
{
    public static ReadOnlySpan<char> ToValidHtml(ReadOnlySpan<char> html)
    {
        html = html.TrimStart();
        AssertHtmlStart(html);
        return html;
    }

    private static void AssertHtmlStart(ReadOnlySpan<char> html)
    {
        var openingTagIndex = html.IndexOf('<');
        if (openingTagIndex is not 0
            || char.IsLetter(html[1]) is not true)
        {
            var message = "Html should start with opening html-tag.";
            throw new InvalidOperationException(message);
        }
    }
}