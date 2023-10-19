using WebScrap.API.Data;
using WebScrap.Css.API;

namespace WebScrap.API;

public class Scrapper
{
    /// <summary>
    /// Processes the html and returns css-compliant tags.
    /// </summary>
    public ScrapResult Scrap(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> css)
    {
        html = html.TrimStart(' ');
        var tagRanges = CssAPI.GetTagRanges(css, html);
        return new ScrapResult(html, tagRanges);
    }
}