using WebScrap.API.Data;
using WebScrap.Css.API;

namespace WebScrap.API;

public class Scrapper
{
    /// <summary>
    /// Processes the html and returns css-compliant tags.
    /// </summary>
    public IScrapResult Scrap(
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
    public IScrapResult Scrap(string html, string[] css)
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