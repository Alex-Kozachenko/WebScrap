namespace WebScrap.Css.Data.Tags;

/// <summary>
/// Represents a css-tag selector.
/// </summary>
/// <param name="Name"> Represents a TagName or wildcard.</param>
public abstract record class CssTagBase(string Name)
{
    public override string ToString()
        => string.Empty;
}
