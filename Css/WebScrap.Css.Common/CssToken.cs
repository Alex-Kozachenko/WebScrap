using WebScrap.Css.Common.Attributes;
using WebScrap.Css.Common.Selectors;
using WebScrap.Css.Common.Tags;

namespace WebScrap.Css.Common;

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