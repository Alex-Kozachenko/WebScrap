using DevOvercome.WebScrap.Data;
using WebScrap.Css.API;

namespace DevOvercome.WebScrap;

public interface IWebScrapper
{
    static IWebScrapper Create(string html) => new WebScrapper(html);
    IScrapResult Run(string css);
    IScrapResult Run(IEnumerable<string> css);
}

/// <summary>
/// Main engine, which extracts queried text from html.
/// </summary>
public class WebScrapper(string html) : IWebScrapper
{
    private readonly string html = html.TrimStart(' ');

    /// <summary>
    /// Processes the html and returns css-compliant tags.
    /// </summary>
    public IScrapResult Run(string css)
    {
        var tagRanges = CssAPI.GetTagRanges(css, html);
        var cssTagRanges = new CssTagRanges(css, [..tagRanges]);
        return new ScrapResult(html, cssTagRanges);
    }

    /// <summary>
    /// Processes the html and returns groups of css-compliant tags.
    /// </summary>
    public IScrapResult Run(IEnumerable<string> css)
    {
        var cssTagRanges = new List<CssTagRanges>();
        foreach (var cssItem in css)
        {
            var tagRanges = CssAPI.GetTagRanges(cssItem, html);
            cssTagRanges.Add(new(cssItem, [..tagRanges]));
        }

        return new ScrapResult(html, [..cssTagRanges]);
    }
}