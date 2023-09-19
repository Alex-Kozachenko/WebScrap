namespace Core.Css.Tools;

internal record struct CssToken(char? ChildSelector, ReadOnlyMemory<char> Css)
{
    public override readonly string ToString()
        => ChildSelector + Css.ToString();
}