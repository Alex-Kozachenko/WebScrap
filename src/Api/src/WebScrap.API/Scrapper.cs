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
        string css)
    {
        html = html.TrimStart(' ');
        var tagRanges = CssAPI.GetTagRanges(css.AsSpan(), html);
        var item = (css.AsMemory(), tagRanges.ToArray());
        return new ScrapResult(html, item);
    }

    /// <summary>
    /// Processes the html and returns groups of css-compliant tags.
    /// </summary>
    public ScrapResult Scrap(ReadOnlySpan<char> html, string[] css)
    {
        var items = new List<(ReadOnlyMemory<char>, Range[])>();
        foreach (var cssItem in css)
        {
            var tagRanges = CssAPI.GetTagRanges(cssItem.AsSpan(), html).ToArray();
            items.Add((cssItem.AsMemory(), tagRanges));
        }

        return new(html, [.. items]);
    }
}