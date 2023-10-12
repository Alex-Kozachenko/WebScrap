using System.Collections.Immutable;
using WebScrap.Css.Preprocessing;
using WebScrap.Css.Matching;

namespace WebScrap.Css.API;

public static class CssAPI
{
    public static ImmutableArray<int> GetTagIndexes(
        ReadOnlySpan<char> css, 
        ReadOnlySpan<char> html)
    {
        var builder = new TokensBuilder();
        var comparer = new CssComparer();
        return new CssProcessor(comparer, builder, css)
            .ProcessCss(html);
    }
}
