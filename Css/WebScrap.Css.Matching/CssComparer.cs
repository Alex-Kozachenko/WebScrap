using WebScrap.Common.Tags;
using WebScrap.Css.Data;
using WebScrap.Css.Contracts;
using WebScrap.Css.Matching.Comparers;
using WebScrap.Css.Matching.Engine;

namespace WebScrap.Css.Matching;

public class CssComparer : ICssComparer
{
    public bool CompareAttributes(
        CssToken[] expectedCssTokens, 
        OpeningTag[] tagsHistory)
        => Compare(new AttributesComparer(), expectedCssTokens, tagsHistory);

    public bool CompareNames(
        CssToken[] expectedCssTokens, 
        OpeningTag[] tagsHistory)
        => Compare(new NameComparer(), expectedCssTokens, tagsHistory);

    private static bool Compare(
        IComparer comparer, 
        CssToken[] expectedCssTokens, 
        OpeningTag[] tagsHistory)
    {
        var tracker = new CssTracker(expectedCssTokens, tagsHistory);
        return new CssTraverser(comparer, tracker).Traverse();
    }
}