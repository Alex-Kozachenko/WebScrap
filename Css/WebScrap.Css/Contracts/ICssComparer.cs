using WebScrap.Css.Data;
using WebScrap.Core.Tags;

namespace WebScrap.Css.Contracts;

public interface ICssComparer
{
    public bool CompareNames(
        CssToken[] expectedCssTokens, 
        TagInfo[] tagsHistory);
        
    public bool CompareAttributes(
        CssToken[] expectedCssTokens, 
        TagInfo[] tagsHistory);
}