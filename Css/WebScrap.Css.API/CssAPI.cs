using System.Collections.Immutable;
using WebScrap.Css.Preprocessing;
using WebScrap.Css.Matching;

namespace WebScrap.Css.API;

public static class CssAPI
{
    /// <summary>
    /// Runs the html processing.
    /// </summary>
    /// <param name="css"> The css pattern for compliance. </param>
    /// <param name="html"> The html for processing. </param>
    /// <returns> Ranges of css-compliant tags. </returns>
    public static ImmutableArray<Range> GetTagRanges(
        ReadOnlySpan<char> css, 
        ReadOnlySpan<char> html)
    {
        var builder = new TokensBuilder();
        var comparer = new CssComparer();
        return new CssProcessor(comparer, builder, css)
            .ProcessCss(html);
    }
}
