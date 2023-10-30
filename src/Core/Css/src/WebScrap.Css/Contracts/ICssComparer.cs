using WebScrap.Css.Data;
using WebScrap.Core.Tags.Data;

namespace WebScrap.Css.Contracts;

/// <summary>
/// Represents a comparer for css token via name and attributes.
/// </summary>
public interface ICssComparer
{
    public bool CompareNames(
        CssToken[] expectedCssTokens, 
        TagInfo[] tagsHistory);
        
    public bool CompareAttributes(
        CssToken[] expectedCssTokens, 
        TagInfo[] tagsHistory);
}