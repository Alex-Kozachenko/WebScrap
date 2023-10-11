using WebScrap.Css.Data.Selectors;

namespace WebScrap.Css.Preprocessing.Readers;

internal readonly ref struct CssSelectorReader(ReadOnlySpan<char> selector)
{
    private readonly ReadOnlySpan<char> selector = selector;

    internal static int Read(ReadOnlySpan<char> selector, out CssSelector result) 
        => new CssSelectorReader(selector).Read(out result);

    private int Read(out CssSelector result) 
    {
        if (selector.IsEmpty)
        {
            result = new RootCssSelector();
            return 0;
        }

        result = selector[0] switch
        {
            ' ' => new AnyChildCssSelector(),
            '>' => new ChildCssSelector(),
            _ => throw new ArgumentException($"Unknown child selector: {selector}")
        };
        return 1;
    }
}