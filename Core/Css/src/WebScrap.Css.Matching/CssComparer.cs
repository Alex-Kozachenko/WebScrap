using WebScrap.Core.Tags;
using WebScrap.Css.Data;
using WebScrap.Css.Contracts;
using WebScrap.Css.Matching.Comparers;
using WebScrap.Css.Matching.Core;

namespace WebScrap.Css.Matching;

public class CssComparer : ICssComparer
{
    public bool CompareAttributes(
        CssToken[] expectedCssTokens, 
        TagInfo[] tagInfos)
        => Compare(new AttributesComparer(), expectedCssTokens, tagInfos);

    public bool CompareNames(
        CssToken[] expectedCssTokens, 
        TagInfo[] tagInfos)
        => Compare(new NameComparer(), expectedCssTokens, tagInfos);

    private static bool Compare(
        IComparer comparer, 
        CssToken[] expectedCssTokens, 
        TagInfo[] tagInfos)
    {
        var tracker = new CssTracker(expectedCssTokens, tagInfos);
        return new CssTraverser(comparer, tracker).Traverse();
    }
}