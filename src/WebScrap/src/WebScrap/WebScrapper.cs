using DevOvercome.WebScrap.Data;
using WebScrap.Css.API;

namespace DevOvercome.WebScrap;

/// <summary>
/// Main engine, which extracts queried text from html.
/// </summary>
public class WebScrapper
{
    /// <summary>
    /// Processes the html and returns css-compliant tags.
    /// </summary>
    public IScrapResult Run(
        string html,
        string css)
    {
        html = html.TrimStart(' ');
        var tagRanges = CssAPI.GetTagRanges(css, html);
        var cssTagRanges = new CssTagRanges(css, [..tagRanges]);
        return new ScrapResult(html, cssTagRanges);
    }

    /// <summary>
    /// Processes the html and returns groups of css-compliant tags.
    /// </summary>
    public IScrapResult Run(string html, string[] css)
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