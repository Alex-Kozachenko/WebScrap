using WebScrap.Css.Data;
using WebScrap.Common.Tags;

namespace WebScrap.Css.Contracts;

public interface ICssComparer
{
    public bool CompareNames(CssToken[] expectedCssTokens, OpeningTag[] tagsHistory);
    public bool CompareAttributes(CssToken[] expectedCssTokens, OpeningTag[] tagsHistory);
}