namespace WebScrap.Html.Extracting;

public interface IHtmlTableExtractor
{
    string[][] ExtractTable(ReadOnlySpan<char> html);
}