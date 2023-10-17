using WebScrap.Html.Extracting;

namespace WebScrap.API;

public static class Parser
{
    [Obsolete("Will be integrated into Json output.")]
    public static string[][] ParseTable(
        ReadOnlySpan<char> html)
    {
        return new HtmlTableExtractor().ExtractTable(html);
    }
}