namespace WebScrap.Css.Data.Tags;

/// <summary>
/// Represents a named css-tag selector.
/// </summary>
/// <param name="Name">TagName.</param>
public record class CssTag(string Name) : CssTagBase(Name)
{
    public override string ToString() => Name;
}