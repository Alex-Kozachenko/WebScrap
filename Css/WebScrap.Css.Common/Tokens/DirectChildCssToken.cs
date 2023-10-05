namespace WebScrap.Css.Common.Tokens;

public sealed record class DirectChildCssToken(
    string Name, 
    ILookup<string, string> Attributes) 
    : CssTokenBase(Name, Attributes);
