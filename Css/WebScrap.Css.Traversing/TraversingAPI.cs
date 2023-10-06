using WebScrap.Common.Tags;
using WebScrap.Css.Common.Tokens;
using WebScrap.Css.Common.Comparers;

namespace WebScrap.Css.Traversing;

public static class TraversingAPI
{
    public static bool TraverseNames(
        IReadOnlyCollection<CssTokenBase> cssCompliantTags,
        IReadOnlyCollection<OpeningTag> traversedTags) 
        => Traverse(
            new NameComparer(),
            cssCompliantTags,
            traversedTags);

    public static bool TraverseAttributes(
        IReadOnlyCollection<CssTokenBase> cssCompliantTags,
        IReadOnlyCollection<OpeningTag> traversedTags) 
        => Traverse(
            new AttributesComparer(),
            cssCompliantTags,
            traversedTags);

    private static bool Traverse(
        IComparer comparer, 
        IReadOnlyCollection<CssTokenBase> cssCompliantTags,
        IReadOnlyCollection<OpeningTag> traversedTags)
    {
        var cssTracker = new CssTracker(cssCompliantTags, traversedTags);
        return CssTraverser.Traverse(comparer, cssTracker);
    }
}