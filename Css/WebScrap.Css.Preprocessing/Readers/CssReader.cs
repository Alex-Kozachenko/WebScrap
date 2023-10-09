using System.Collections.Immutable;
using WebScrap.Css.Common;
using WebScrap.Css.Common.Attributes;
using WebScrap.Css.Common.Selectors;
using WebScrap.Css.Common.Tags;

namespace WebScrap.Css.Preprocessing.Readers;

internal static class CssReader
{
    internal static CssTagBase ReadTag(ReadOnlyMemory<char> css)
    {
        var counter = 0;
        while (counter < css.Length 
            && char.IsLetter(css.Span[counter])) 
            {
                counter++;
            }

        return counter switch
        {
            0 => new AnyCssTag(),
            _ => new CssTag(css[..counter].ToString()),
        };
    }

    internal static CssSelector ReadSelector(char? childSelector) 
        => childSelector switch
        {
            null => new RootCssSelector(),
            ' ' => new AnyChildCssSelector(),
            '>' => new ChildCssSelector(),
            _ => throw new ArgumentException($"Unknown child selector: {childSelector}")
        };

    internal static CssAttributesLookup ReadAttributes(ReadOnlyMemory<char> css)
        => new AttributesReader(css).Read();
}