namespace WebScrap.Css.Common.Tokens;

public sealed record class RootCssToken(
    string Name, 
    ILookup<string, string> Attributes) 
    : CssTokenBase(Name, Attributes);