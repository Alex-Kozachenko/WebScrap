using System.Collections.Immutable;

namespace WebScrap.Css.Tools;

internal static class CssTokenBuilder
{
    internal static CssToken Build(
        ReadOnlyMemory<char> css,
        ReadOnlySpan<char> childSelectors)
    {
        var childSelector = css.Span[0];
        return childSelectors.Contains(childSelector)
            ? Build(css[1..], childSelector)
            : Build(css, (char?)null);
    }

    private static CssToken Build(ReadOnlyMemory<char> css, char? childSelector)
    {
        var tagName = GetTagName(css);
        return Build(css, childSelector, tagName);
    }

    private static ReadOnlyMemory<char> GetTagName(ReadOnlyMemory<char> css)
    {
        var counter = 0;
        while (counter < css.Length 
            && char.IsLetter(css.Span[counter])) 
            {
                counter++;
            }
        return css[..counter];
    }

    private static CssToken Build(
        ReadOnlyMemory<char> css, 
        char? childSelector, 
        ReadOnlyMemory<char> tagName)
    {
        var attributes = CssAttributeBase.Create(css[tagName.Length..]);
        return new(tagName, childSelector, [attributes]);
    }

    // private static ImmutableArray<CssAttributeBase> ExtractAttributes
    //     (ReadOnlyMemory<char> attributes)
    // {
    //     const int maxTokens = 32;
    //     Span<Range> ranges = new Range[maxTokens];
    //     var rangesCount = attributes.Span.SplitAny(
    //         ranges,
    //         attributeSelectors,
    //         StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    //     var result = new List<CssAttributeBase>();
    //     for (int i = 0; i < rangesCount; i++)
    //     {
    //         var range = ranges[i];
    //         var start = range.Start.Value - 1; // HACK: split works unexpected.
    //         var slice = attributes[start..range.End];
    //         result.Add(new(slice[1..], slice.Span[0]));
    //     }
    //     return [.. result];
    // }
}