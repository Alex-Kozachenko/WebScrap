namespace WebScrap.Modules.Extracting.Html.Contracts;

public interface ITextExtractor
{
    /// <summary>
    /// Accepts single tag and returns it's text.
    /// </summary>
    string ExtractText(ReadOnlySpan<char> tag);
}