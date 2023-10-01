namespace WebScrap.Css.Tokens;

internal record struct CssToken(
    ReadOnlyMemory<char> Tag,
    char? ChildSelector, 
    ILookup<string, string> Attributes)
{
}