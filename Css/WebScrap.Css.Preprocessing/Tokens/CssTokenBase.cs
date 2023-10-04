namespace WebScrap.Css.Preprocessing.Tokens;

public abstract record class CssTokenBase(
    string Name, 
    ILookup<string, string> Attributes);
