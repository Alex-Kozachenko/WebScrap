namespace WebScrap.Common.Css.Tags;

public record class CssTag(string Name) : CssTagBase(Name)
{
    public override string ToString() => Name;
}