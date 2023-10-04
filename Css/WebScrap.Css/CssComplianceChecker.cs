using WebScrap.Common.Tags;
using WebScrap.Css.Listeners.Helpers;
using WebScrap.Css.Preprocessing.Tokens;

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
        var clone = Clone(checker);
        return clone.cssCompliantTags.Count <= clone.traversedTags.Count;
    }

    internal static bool CheckNames(CssComplianceChecker checker) 
    {
        var clone = Clone(checker);
        return clone.Traverse((cssTag, travTag)
            => cssTag.Name.SequenceEqual(travTag.Name));
    }

    internal static bool CheckAttributes(CssComplianceChecker checker)
    {
        var clone = Clone(checker);
        return clone.Traverse((cssTag, travTag)
            => AttributesComparer.IsSubsetOf(
                cssTag.Attributes, 
                travTag.Attributes));
    }

    private static CssComplianceChecker Clone(CssComplianceChecker original)
    {
        return new CssComplianceChecker(
            new Stack<CssTokenBase>(original.cssCompliantTags.Reverse()),
            new Stack<OpeningTag>(original.traversedTags.Reverse())
        );
    }

    private bool Traverse(Func<CssTokenBase, OpeningTag, bool> isValid)
    {
        var cssTag = cssCompliantTags.Pop();
        var travTag = traversedTags.Pop();

        if (isValid(cssTag, travTag))
        {
            return DecideNextTraverse(cssTag, isValid);
        }
        else
        {
            return false;
        }
    }


    private bool Traverse_AnyChildCssToken(
        Func<CssTokenBase, OpeningTag, bool> isValid)
    {
        var cssTag = cssCompliantTags.Peek();
        var travTag = traversedTags.Pop();

        if (isValid(cssTag, travTag))
        {
            return DecideNextTraverse(cssTag, isValid);
        }
        else
        {
            return Traverse_AnyChildCssToken(isValid);
        }
    }

    private bool Traverse_DirectChildCssToken(
        Func<CssTokenBase, OpeningTag, bool> isValid)
    {
        var cssTag = cssCompliantTags.Peek();
        var travTag = traversedTags.Pop();

        if (isValid(cssTag, travTag))
        {
            cssCompliantTags.Pop();
            return DecideNextTraverse(cssTag, isValid);
        }
        else
        {
            return false;
        }
    }

    private bool DecideNextTraverse(
        CssTokenBase cssTag,
        Func<CssTokenBase, OpeningTag, bool> isValid) 
        => cssTag switch
        {
            AnyChildCssToken => Traverse_AnyChildCssToken(isValid),
            DirectChildCssToken => Traverse_DirectChildCssToken(isValid),
            RootCssToken => true,
        };
}