namespace WebScrap.Css.Common.Tokens;

public sealed record class AnyChildCssToken(
    string Name, 
    ILookup<string, string> Attributes) 
    : CssTokenBase(Name, Attributes);
