using WebScrap.Css.Preprocessing;
using WebScrap.Css.Matching;

namespace WebScrap.Css.API;

public static class CssAPI
{
    public static List<int> GetTagIndexes(
        ReadOnlySpan<char> css, 
        ReadOnlySpan<char> html)
    {
        var builder = new TokensBuilder();
        var comparer = new CssComparer();
        var cssProcessor = new CssProcessor(comparer, builder, css);
        cssProcessor.Run(html);
        return cssProcessor.TagIndexes;
    }
}
