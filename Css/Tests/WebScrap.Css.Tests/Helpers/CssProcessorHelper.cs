using System.Collections.Immutable;
using WebScrap.Css.Preprocessing;
using WebScrap.Css.Matching;

namespace WebScrap.Css.Tests.Helpers;

public static class CssProcessorHelper
{
    internal static ImmutableArray<int> CalculateTagIndexes(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> css)
        {
            var builder = new TokensBuilder();
            var comparer = new CssComparer();
            var cssProcessor = new CssProcessor(comparer, builder, css);
            cssProcessor.Run(html);
            return [.. cssProcessor.TagIndexes];
        }
}