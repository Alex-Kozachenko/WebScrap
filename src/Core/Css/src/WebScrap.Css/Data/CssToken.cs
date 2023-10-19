using WebScrap.Css.Data.Attributes;
using WebScrap.Css.Data.Selectors;
using WebScrap.Css.Data.Tags;

namespace WebScrap.Css.Data;

/// <summary>
/// Represents a token of css query.
/// </summary>
public record class CssToken
{
    /// <summary>
    /// Gets a selector rule for descendants.
    /// </summary>
    public CssSelector Selector { get; init; }

    /// <summary>
    /// Gets a tag name, or a wildcard.
    /// </summary>
    public CssTagBase Tag { get; init; }

    /// <summary>
    /// Gets a lookup of attributes. See format <see cref="CssAttributesLookup"/>  for details.
    /// </summary>
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