using System.Collections.Immutable;
using WebScrap.Css.Attributes;

namespace WebScrap.Css.Tokens;

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
        var attributes = new Reader(css[tagName.Length..]).Read();
        return new(tagName, childSelector, attributes);
    }
}