using WebScrap.Common.Tags;
using WebScrap.Css.Common.Tokens;
using WebScrap.Css.Traversing.Validators;

namespace WebScrap.Css.Traversing.Strategies;

internal sealed class RootTraversingStrategy(
    CssValidatorBase validator,
    Stack<CssTokenBase> cssCompliantTags,
    Stack<OpeningTag> traversedTags) 
        : TraversingStrategyBase(validator, cssCompliantTags, traversedTags)
{
    public override bool Traverse()
    {
        var currentCss = cssCompliantTags.Peek();
        var travTag = traversedTags.Pop();

        if (!validator.IsValid(currentCss, travTag))
        {
            return false;
        }
        cssCompliantTags.Pop();
        return TraverseNext(currentCss);
    }
}