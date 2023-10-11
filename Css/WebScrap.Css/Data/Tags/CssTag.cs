namespace WebScrap.Css.Data.Tags;

public record class CssTag(string Name) : CssTagBase(Name)
{
    public override string ToString() => Name;
}