namespace WebScrap.Css.Preprocessing.Tokens;

public record struct CssToken(
    ReadOnlyMemory<char> Tag,
    char? ChildSelector, 
    ILookup<string, string> Attributes)
{
}