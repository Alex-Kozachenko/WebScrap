using System.Collections.Immutable;
using WebScrap.Css.Preprocessing;
using WebScrap.Css.Matching;
using WebScrap.Core.Tags;

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
        var tagsProvider = new TagsProvider();
        var cssProcessor = new CssProcessor(
            new CssComparer(), 
            new TokensBuilder().Build(css))
            .Subscribe(tagsProvider);
        tagsProvider.Process(html);
        return cssProcessor.CssCompliantTagRanges;
    }
}
