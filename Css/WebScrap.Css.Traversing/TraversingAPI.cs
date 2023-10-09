using WebScrap.Common.Tags;
using WebScrap.Css.Common.Tokens;
using WebScrap.Css.Common.Comparers;

namespace WebScrap.Css.Traversing;

public static class TraversingAPI
{
    public static bool TraverseNames(
        IReadOnlyCollection<CssTokenBase> expectedTags,
        IReadOnlyCollection<OpeningTag> traversedTags) 
        => Traverse(
            new NameComparer(),
            expectedTags,
            traversedTags);

    public static bool TraverseAttributes(
        IReadOnlyCollection<CssTokenBase> expectedTags,
        IReadOnlyCollection<OpeningTag> traversedTags) 
        => Traverse(
            new AttributesComparer(),
            expectedTags,
            traversedTags);

    private static bool Traverse(
        IComparer comparer,
        IReadOnlyCollection<CssTokenBase> expectedTags,
        IReadOnlyCollection<OpeningTag> traversedTags) 
        => new CssTraverser(comparer, expectedTags, traversedTags)
            .Traverse();
}