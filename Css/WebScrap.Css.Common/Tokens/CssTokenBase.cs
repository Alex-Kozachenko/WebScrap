namespace WebScrap.Css.Common.Tokens;

/// <summary>
/// Represents a css token without a selector.
/// </summary>
/// <remarks>
/// EXAMPLE:
/// `?a#foo.bar` 
/// </remarks>
public abstract record class CssTokenBase(
    string Name, 
    ILookup<string, string> Attributes);
