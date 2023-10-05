namespace WebScrap.Css.Common.Tokens;

/// <summary>
/// Represents a css token with a space ' ' selector.
/// </summary>
/// <remarks>
/// EXAMPLE:
/// ` a#foo.bar` 
/// </remarks>
public sealed record class AnyChildCssToken(
    string Name, 
    ILookup<string, string> Attributes) 
    : CssTokenBase(Name, Attributes);
