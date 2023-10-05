using WebScrap.Common.Tags;
using WebScrap.Css.Common.Tokens;
using WebScrap.Css.Traversing.Strategies;
using WebScrap.Css.Traversing.Validators;

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
        return new RootTraversingStrategy(
            new NamesCssValidator(), 
            clone.cssCompliantTags,
            clone.traversedTags)
            .Traverse();
    }

    internal static bool CheckAttributes(CssComplianceChecker checker)
    {
        var clone = Clone(checker);
        return new RootTraversingStrategy(
            new AttributesCssValidator(), 
            clone.cssCompliantTags,
            clone.traversedTags)
            .Traverse();
    }

    private static CssComplianceChecker Clone(CssComplianceChecker original)
    {
        return new CssComplianceChecker(
            new Stack<CssTokenBase>(original.cssCompliantTags.Reverse()),
            new Stack<OpeningTag>(original.traversedTags.Reverse())
        );
    }
}