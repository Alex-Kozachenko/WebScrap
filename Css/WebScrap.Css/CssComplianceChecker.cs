using WebScrap.Common.Tags;
using WebScrap.Css.Common.Tokens;
using WebScrap.Css.Traversing;

namespace WebScrap.Css;

// TODO: its unclear how its working.
// And its a legit drunk code.
internal class CssComplianceChecker
{
    private readonly Stack<CssTokenBase> cssCompliantTags;
    private readonly Stack<OpeningTag> traversedTags;

    public CssComplianceChecker(
        Stack<CssTokenBase> cssCompliantTags,
        Stack<OpeningTag> traversedTags)
    {
        this.cssCompliantTags = cssCompliantTags;
        this.traversedTags = traversedTags;
    }
    internal static bool CheckLength(CssComplianceChecker checker)
    {
        return checker.cssCompliantTags.Count <= checker.traversedTags.Count;
    }

    internal static bool CheckNames(CssComplianceChecker checker) 
    {
        var clone = Clone(checker);
        return TraversingAPI.TraverseNames(
            clone.cssCompliantTags,
            clone.traversedTags);
    }

    internal static bool CheckAttributes(CssComplianceChecker checker)
    {
        var clone = Clone(checker);
        return TraversingAPI.TraverseNames(
            clone.cssCompliantTags,
            clone.traversedTags);
    }

    private static CssComplianceChecker Clone(CssComplianceChecker original)
    {
        return new CssComplianceChecker(
            new Stack<CssTokenBase>(original.cssCompliantTags.Reverse()),
            new Stack<OpeningTag>(original.traversedTags.Reverse())
        );
    }
}