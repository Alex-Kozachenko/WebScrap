namespace Core.Html.Tools;

internal static class HtmlValidator
{
    // TODO: make a dedicated object like `ValidHtml`, so this check wont be nessesary every time.
    public static ReadOnlySpan<char> ToValidHtml(ReadOnlySpan<char> html)
    {
        html = html.TrimStart();
        AssertHtmlStart(html);
        return html;
    }

    private static void AssertHtmlStart(ReadOnlySpan<char> html)
    {
        var openingTagIndex = html.IndexOf('<');
        if (openingTagIndex is not 0)
        {
            var message = "Html should start with <";
            throw new InvalidOperationException(message);
        }
    }
}