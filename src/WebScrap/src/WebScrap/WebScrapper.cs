using DevOvercome.WebScrap.Data;
using WebScrap.Css.API;

namespace DevOvercome.WebScrap;

public class WebScrapper(string html) : IWebScrapper
{
    private readonly string html = html.TrimStart(' ');

    public IScrapResult Run(string css)
    {
        var tagRanges = CssAPI.GetTagRanges(css, html);
        var cssTagRanges = new CssTagRanges(css, [..tagRanges]);
        return new ScrapResult(html, cssTagRanges);
    }

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