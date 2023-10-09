namespace WebScrap.Css.Common.Tags;

public record class CssTag(string Name) : CssTagBase(Name)
{
    public override string ToString() => Name;
}