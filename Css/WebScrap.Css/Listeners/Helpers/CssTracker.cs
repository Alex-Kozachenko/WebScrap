using WebScrap.Css.Tokens;
using System.Collections.Immutable;
using WebScrap.Common.Tags;

namespace WebScrap.Css.Listeners.Helpers;

internal class CssTracker(ReadOnlySpan<char> css)
{
    private readonly ImmutableArray<CssToken> expectedTags
        = CssTokenizer.TokenizeCss(css);

    internal CssToken GetCurrentExpectedTag(Stack<OpeningTag> cssTags)
    {
        var processedTagsCount = cssTags.Count;
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

    internal bool IsCompletedCssMet(Stack<OpeningTag> cssTags)
        => cssTags.Count == expectedTags.Length;
}