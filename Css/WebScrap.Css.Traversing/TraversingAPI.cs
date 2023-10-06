using WebScrap.Common.Tags;
using WebScrap.Css.Common.Tokens;
using WebScrap.Css.Common.Comparers;

namespace WebScrap.Css.Traversing;

public static class TraversingAPI
{
    public static bool TraverseNames(
        Stack<CssTokenBase> cssCompliantTags,
        Stack<OpeningTag> traversedTags) 
        => Traverse(
            new NameComparer(),
            cssCompliantTags,
            traversedTags);

    public static bool TraverseAttributes(
        Stack<CssTokenBase> cssCompliantTags,
        Stack<OpeningTag> traversedTags) 
        => Traverse(
            new AttributesComparer(),
            cssCompliantTags,
            traversedTags);

    private static bool Traverse(
        IComparer comparer, 
        Stack<CssTokenBase> cssCompliantTags,
        Stack<OpeningTag> traversedTags)
    {
        var css = new Stack<CssTokenBase>(cssCompliantTags.Reverse());
        var tags = new Stack<OpeningTag>(traversedTags.Reverse());
        var cssTracker = new CssTracker(css, tags);
        return CssTraverser.Traverse(comparer, cssTracker);
    }
}