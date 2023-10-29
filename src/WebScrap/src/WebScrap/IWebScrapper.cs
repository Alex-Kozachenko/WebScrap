using DevOvercome.WebScrap.Data;

namespace DevOvercome.WebScrap;

/// <summary>
/// Main engine, which extracts queried text from html.
/// </summary>
public interface IWebScrapper 
{
    /// <summary>
    /// Processes the html and returns css-compliant tags.
    /// </summary>
    IScrapResult Run(string css);

    /// <summary>
    /// Processes the html and returns groups of css-compliant tags.
    /// </summary>
    IScrapResult Run(IEnumerable<string> css);
}
