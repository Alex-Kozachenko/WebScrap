using WebScrap.Css.Data.Attributes;
using WebScrap.Css.Data.Selectors;
using WebScrap.Css.Data.Tags;

namespace WebScrap.Css.Data;

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