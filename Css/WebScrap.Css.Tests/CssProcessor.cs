using System.Collections.Immutable;
using WebScrap.Tags;

namespace WebScrap.Css.Tests;

public static class CssProcessor
{
    internal static ImmutableArray<int> CalculateTagIndexes(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> css)
        => WebScrap.Css.CssProcessor.CalculateTagIndexes(new TagFactory(), html, css);
}