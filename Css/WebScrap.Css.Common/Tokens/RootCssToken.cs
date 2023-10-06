namespace WebScrap.Css.Common.Tokens;

/// <summary>
/// Represents a root css token.
/// </summary>
/// <remarks>
/// EXAMPLE:
/// `main` 
/// </remarks>
public sealed record class RootCssToken(
    string Name, 
    ILookup<string, string> Attributes) 
    : CssTokenBase(Name, Attributes);