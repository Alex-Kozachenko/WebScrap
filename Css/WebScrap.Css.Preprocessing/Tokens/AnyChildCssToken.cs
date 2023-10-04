namespace WebScrap.Css.Preprocessing.Tokens;

public sealed record class AnyChildCssToken(
    string Name, 
    ILookup<string, string> Attributes) 
    : CssTokenBase(Name, Attributes);
