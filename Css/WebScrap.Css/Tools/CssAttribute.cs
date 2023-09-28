namespace WebScrap.Css.Tools;

/// <summary>
/// Contains a single entry of a css selector.
/// </summary>
/// <remarks>
/// <div class="foo bar"> should be a collection like:
///  class=foo
///  class=bar
/// </remarks>
internal abstract record class CssAttributeBase(
    ReadOnlyMemory<char> SelectorText, 
    char SelectorKind = default)
{
    public override string ToString()
        => SelectorKind.ToString() + SelectorText;

    public static CssAttributeBase Create(ReadOnlyMemory<char> cssToken)
    {
        if (cssToken.Length == 0)
        {
            return new UnknownCssAttribute(string.Empty.AsMemory());
        }
        return cssToken.Span[0] switch
        {
            '#' => new IdCssAttribute(cssToken[1..]),
            '.' => new ClassCssAttribute(cssToken[1..]),
            _ => new UnknownCssAttribute(cssToken)
        };
    }
}

internal record class UnknownCssAttribute(ReadOnlyMemory<char> Selector)
    : CssAttributeBase(Selector, default);

internal record class ClassCssAttribute(ReadOnlyMemory<char> Selector)
    : CssAttributeBase(Selector, '.');

internal record class IdCssAttribute(ReadOnlyMemory<char> Selector)
    : CssAttributeBase(Selector, '#');