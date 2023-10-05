namespace WebScrap.Css.Common.Tokens;

public abstract record class CssTokenBase(
    string Name, 
    ILookup<string, string> Attributes);
