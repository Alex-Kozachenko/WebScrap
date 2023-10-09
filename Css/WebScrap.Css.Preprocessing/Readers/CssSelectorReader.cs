
using WebScrap.Css.Common.Selectors;

namespace WebScrap.Css.Preprocessing.Readers;

internal readonly ref struct CssSelectorReader(char? selector)
{
    internal static int Read(char? selector, out CssSelector cssSelector) 
        => new CssSelectorReader(selector).Read(out cssSelector);

    private int Read(out CssSelector cssSelector) 
    {
        cssSelector = selector switch
        {
            null => new RootCssSelector(),
            ' ' => new AnyChildCssSelector(),
            '>' => new ChildCssSelector(),
            _ => throw new ArgumentException($"Unknown child selector: {selector}")
        };

        return cssSelector is RootCssSelector ? 0 : 1;
    }
}