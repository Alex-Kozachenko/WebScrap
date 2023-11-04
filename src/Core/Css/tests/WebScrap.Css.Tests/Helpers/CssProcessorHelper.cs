using System.Collections.Immutable;
using WebScrap.Css.Preprocessing;
using WebScrap.Css.Matching;
using WebScrap.Core.Tags;

namespace WebScrap.Css.Tests.Helpers;

internal static class CssProcessorHelper
{
    internal static ImmutableArray<Range> CalculateTagRanges(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> css)
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