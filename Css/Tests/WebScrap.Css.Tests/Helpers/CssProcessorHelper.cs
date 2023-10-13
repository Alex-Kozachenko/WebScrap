using System.Collections.Immutable;
using WebScrap.Css.Preprocessing;
using WebScrap.Css.Matching;

namespace WebScrap.Css.Tests.Helpers;

internal static class CssProcessorHelper
{
    internal static ImmutableArray<Range> CalculateTagRanges(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> css)
        {
            var builder = new TokensBuilder();
            var comparer = new CssComparer();
            var cssProcessor = new CssProcessor(comparer, builder, css);
            return cssProcessor.ProcessCss(html);
        }
}