using System.Collections.Immutable;
using WebScrap.Css.Preprocessing;
using WebScrap.Tags;

namespace WebScrap.Css.Tests.Helpers;

public static class CssProcessorHelper
{
    internal static ImmutableArray<int> CalculateTagIndexes(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> css)
        {
            var cssTokens = PreprocessingAPI.Process(css);
            var factory = TagsAPI.CreateTagFactory();
            var processor = new CssProcessor(factory, cssTokens);
            processor.Run(html);
            return processor.TagIndexes.ToImmutableArray();
        }
}