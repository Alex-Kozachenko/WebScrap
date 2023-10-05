using WebScrap.Common.Tags;
using WebScrap.Css.Preprocessing;
using WebScrap.Css.Common.Tokens;
using System.Collections.Immutable;

namespace WebScrap.Css.Listeners.Helpers;

internal class CssTracker(ReadOnlySpan<char> css)
{
    private readonly ImmutableArray<CssTokenBase> expectedTags
        = CssTokenizer.TokenizeCss(css);

    internal CssTokenBase GetCurrentExpectedTag(int processedTagsCount)
    {
        var index = processedTagsCount switch
        {
            < 0 => throw new ArgumentOutOfRangeException(
                $"{nameof(processedTagsCount)} = {processedTagsCount}"),
            var i when i < expectedTags.Length
               => i,
            _ => expectedTags.Length - 1,
        };

        return expectedTags[index];
    }

    internal bool IsCompletedCssMet(int cssTagsLength)
        => cssTagsLength == expectedTags.Length;
}