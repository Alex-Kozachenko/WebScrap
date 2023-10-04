namespace WebScrap.Css.Preprocessing.Tokens;

public sealed record class DirectChildCssToken(
    string Name, 
    ILookup<string, string> Attributes) 
    : CssTokenBase(Name, Attributes);
