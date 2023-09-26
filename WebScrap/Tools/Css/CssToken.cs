using System.Collections.Immutable;

namespace WebScrap.Tools.Css;

internal record struct CssToken(
    ReadOnlyMemory<char> Tag,
    char? ChildSelector = default, 
    ImmutableArray<CssAttributeBase> Attributes = default)
{
    public override readonly string ToString()
        => ChildSelector 
            + Tag.ToString() 
            + string.Join("", Attributes.Select(x => x.ToString()));

}