using WebScrap.Css.Preprocessing.Tokens;
using System.Collections.Immutable;

namespace WebScrap.Css.Preprocessing;

internal static class CssTokenBuilder
{
    internal static CssTokenBase Build(
        ReadOnlyMemory<char> css,
        ReadOnlySpan<char> childSelectors)
    {
        ThrowIfUnsupported(css);
        var childSelector = css.Span[0];
        return childSelectors.Contains(childSelector)
            ? Build(css[1..], childSelector)
            : Build(css, (char?)null);
    }

    private static CssTokenBase Build(ReadOnlyMemory<char> css, char? childSelector)
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

    private static CssTokenBase Build(
        ReadOnlyMemory<char> css, 
        char? childSelector, 
        ReadOnlyMemory<char> tagName)
    {
        var attributes = new AttributesReader(css[tagName.Length..]).Read();
        return childSelector switch 
        {
            null => new RootCssToken(tagName.ToString(), attributes),
            ' ' => new AnyChildCssToken(tagName.ToString(), attributes),
            '>' => new DirectChildCssToken(tagName.ToString(), attributes),
            _ => throw new ArgumentException($"Unknown child selector: {childSelector}")
        };
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