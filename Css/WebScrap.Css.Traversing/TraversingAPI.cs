using WebScrap.Common.Tags;
using WebScrap.Css.Common.Tokens;
using WebScrap.Css.Traversing.Validators;

namespace WebScrap.Css.Traversing;

public static class TraversingAPI
{
    public static bool TraverseNames(
        Stack<CssTokenBase> cssCompliantTags,
        Stack<OpeningTag> traversedTags) 
        => Traverse(
            new NamesCssValidator(),
            cssCompliantTags,
            traversedTags);

    public static bool TraverseAttributes(
        Stack<CssTokenBase> cssCompliantTags,
        Stack<OpeningTag> traversedTags) 
        => Traverse(
            new AttributesCssValidator(),
            cssCompliantTags,
            traversedTags);

    private static bool Traverse(
        CssValidatorBase validator, 
        Stack<CssTokenBase> cssCompliantTags,
        Stack<OpeningTag> traversedTags)
    {
        var css = new Stack<CssTokenBase>(cssCompliantTags.Reverse());
        var tags = new Stack<OpeningTag>(traversedTags.Reverse());
        var cssTracker = new CssTracker(css, tags);
        return new CssTraverser(validator, cssTracker)
                .Traverse();
    }
}