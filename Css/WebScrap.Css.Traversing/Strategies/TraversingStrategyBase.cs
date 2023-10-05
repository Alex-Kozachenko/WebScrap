using WebScrap.Common.Tags;
using WebScrap.Css.Common.Tokens;
using WebScrap.Css.Traversing.Validators;

namespace WebScrap.Css.Traversing.Strategies;

internal abstract class TraversingStrategyBase
{
    protected readonly CssValidatorBase validator;
    protected readonly Stack<CssTokenBase> cssCompliantTags;
    protected readonly Stack<OpeningTag> traversedTags;
    protected CssTokenBase? lastCompliantTag; // HACK: this field is hucked.

    internal TraversingStrategyBase(
        CssValidatorBase validator,
        Stack<CssTokenBase> cssCompliantTags,
        Stack<OpeningTag> traversedTags,
        CssTokenBase? lastCompliantTag = null)
    {
        AssertCss(cssCompliantTags);
        this.validator = validator;
        this.cssCompliantTags = cssCompliantTags;
        this.traversedTags = traversedTags;
        this.lastCompliantTag = lastCompliantTag;
    }

    internal abstract bool Traverse();

    protected bool TraverseNext() 
        => (cssCompliantTags.Count(), traversedTags.Count()) switch
        {
            (0, _) => true,
            (_, 0) => true,
            (_, _) => CreateNextTraverseStrategy()
                        .Traverse()
        };

    protected TraversingStrategyBase CreateNextTraverseStrategy()
        => lastCompliantTag switch
        {
            AnyChildCssToken => new AnyChildTraversingStrategy(
                validator, 
                cssCompliantTags, 
                traversedTags),
            DirectChildCssToken => new DirectChildTraversingStrategy(
                validator, 
                cssCompliantTags, 
                traversedTags),
            _ => new RootTraversingStrategy(
                validator, 
                cssCompliantTags, 
                traversedTags),
        };

    private static void AssertCss(Stack<CssTokenBase> css)
    {
        _ = css.ToArray() switch 
        {
            var a when a.Last() is not RootCssToken
                => throw new ArgumentException("Incorrect css structure. First element should be root."),
            var a when a[..^1].Any(x => x is RootCssToken)
                => throw new ArgumentException("Incorrect css structure. Only first element could be root."),
            _ => 0,
        };
    }
}