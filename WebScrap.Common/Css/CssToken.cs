using WebScrap.Common.Css.Attributes;
using WebScrap.Common.Css.Selectors;
using WebScrap.Common.Css.Tags;

namespace WebScrap.Common.Css;

public record class CssToken
{
    public CssSelector Selector { get; init; }
    public CssTagBase Tag { get; init; }
    public CssAttributesLookup Attributes { get; init; }

    public CssToken(
        CssSelector selector,
        CssTagBase tag,
        CssAttributesLookup? attributes = null)
    {
        Selector = selector;
        Tag = tag;
        Attributes = attributes ?? [];
    }
}