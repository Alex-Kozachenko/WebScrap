namespace Core.Tools;

internal record struct CssToken(char? ChildSelector, ReadOnlyMemory<char> Css)
{
    public static implicit operator ValueTuple<char?, string>(CssToken target)
        => (target.ChildSelector, target.Css.ToString());
}