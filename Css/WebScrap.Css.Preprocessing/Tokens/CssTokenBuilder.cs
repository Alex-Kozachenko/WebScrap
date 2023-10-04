using System.Collections.Immutable;
using WebScrap.Css.Preprocessing;

namespace WebScrap.Css.Preprocessing.Tokens;

internal static class CssTokenBuilder
{
    internal static CssOpeningTag Build(
        ReadOnlyMemory<char> css,
        ReadOnlySpan<char> childSelectors)
    {
        ThrowIfUnsupported(css);
        var childSelector = css.Span[0];
        return childSelectors.Contains(childSelector)
            ? Build(css[1..], childSelector)
            : Build(css, (char?)null);
    }

    private static CssOpeningTag Build(ReadOnlyMemory<char> css, char? childSelector)
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

    private static CssOpeningTag Build(
        ReadOnlyMemory<char> css, 
        char? childSelector, 
        ReadOnlyMemory<char> tagName)
    {
        var attributes = new AttributesReader(css[tagName.Length..]).Read();
        return new(tagName.ToString(), attributes, childSelector);
    }

    private static void ThrowIfUnsupported(ReadOnlyMemory<char> css)
    {
        _ = css.Span.IndexOfAny("*[],") switch 
        {
            -1 => 0,
            var i => throw new ArgumentException($"Unable to process css. Css contains unsupported chars: {css[i..]}.")
        };        
    }
}