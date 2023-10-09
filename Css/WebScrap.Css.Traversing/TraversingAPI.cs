using WebScrap.Common.Tags;

using WebScrap.Css.Common.Comparers;
using WebScrap.Css.Common;

namespace WebScrap.Css.Traversing;

public static class TraversingAPI
{
    public static bool TraverseNames(
        IReadOnlyCollection<CssToken> expectedTags,
        IReadOnlyCollection<OpeningTag> traversedTags) 
        => Traverse(
            new NameComparer(),
            expectedTags,
            traversedTags);

    public static bool TraverseAttributes(
        IReadOnlyCollection<CssToken> expectedTags,
        IReadOnlyCollection<OpeningTag> traversedTags) 
        => Traverse(
            new AttributesComparer(),
            expectedTags,
            traversedTags);

    private static bool Traverse(
        IComparer comparer,
        IReadOnlyCollection<CssToken> expectedTags,
        IReadOnlyCollection<OpeningTag> traversedTags) 
        => new CssTraverser(comparer, expectedTags, traversedTags)
            .Traverse();
}