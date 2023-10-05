using WebScrap.Common.Tags;
using WebScrap.Css.Common.Tokens;
using WebScrap.Css.Traversing.Validators;

namespace WebScrap.Css.Traversing.Strategies;

internal abstract class TraversingStrategyBase
{
    protected readonly CssValidatorBase validator;
    protected readonly Stack<CssTokenBase> cssCompliantTags;
    protected readonly Stack<OpeningTag> traversedTags;

    public TraversingStrategyBase(
        CssValidatorBase validator,
        Stack<CssTokenBase> cssCompliantTags,
        Stack<OpeningTag> traversedTags)
    {
        AssertCss(cssCompliantTags);
        this.validator = validator;
        this.cssCompliantTags = cssCompliantTags;
        this.traversedTags = traversedTags;
    }

    public abstract bool Traverse();

    protected bool TraverseNext(CssTokenBase currentCssTag)
        => CreateNextTraverseStrategy(currentCssTag)
            .Traverse();

    internal TraversingStrategyBase CreateNextTraverseStrategy(
        CssTokenBase currentCssTag)
        => currentCssTag switch
        {
            AnyChildCssToken => new AnyChildTraversingStrategy(
                validator, 
                cssCompliantTags, 
                traversedTags),
            DirectChildCssToken => new DirectChildTraversingStrategy(
                validator, 
                cssCompliantTags, 
                traversedTags),
            RootCssToken => new DirectChildTraversingStrategy(
                validator, 
                cssCompliantTags, 
                traversedTags),
            _ => throw new InvalidOperationException(),
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