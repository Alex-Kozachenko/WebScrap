using WebScrap.Html.Extracting;

namespace WebScrap.API;

public static class Parse
{
    public static string[][] Table(
        ReadOnlySpan<char> html)
    {
        return new HtmlTableExtractor().ExtractTable(html);
    }
}