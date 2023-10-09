using System.Collections.Immutable;
using WebScrap.Css.Common;
using WebScrap.Css.Common.Selectors;
using WebScrap.Css.Common.Tags;
using WebScrap.Css.Preprocessing.Readers;

namespace WebScrap.Css.Preprocessing;

internal static class CssTokenBuilder
{
    private static readonly char[] childSelectors = [' ', '>'];

    // TODO: Rework this to recursion, so CssTokensReader probably wont be neccesary.
    internal static CssToken Build(
        ReadOnlyMemory<char> css)
    {
        ThrowIfUnsupported(css);
        
        var childSelector = css.Span[0];
        return childSelectors.Contains(childSelector)
            ? Build(css[1..], CssReader.ReadSelector(childSelector))
            : Build(css, CssReader.ReadSelector(null));
    }

    private static CssToken Build(ReadOnlyMemory<char> css, CssSelector selector) 
        => Build(css, selector, CssReader.ReadTag(css));

    private static CssToken Build(
        ReadOnlyMemory<char> css, 
        CssSelector selector, 
        CssTagBase tag)
    {
        var attributes = CssReader.ReadAttributes(css[tag.Name.Length..]);
        return new CssToken(selector, tag, attributes);
    }

    private static void ThrowIfUnsupported(ReadOnlyMemory<char> css)
    {
        _ = css.Span.IndexOfAny("[],") switch 
        {
            -1 => 0,
            var i => throw new ArgumentException($"Unable to process css. Css contains unsupported chars: {css[i..]}.")
        };
    }
}