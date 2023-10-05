using WebScrap.Common.Tags;
using WebScrap.Css.Common.Tokens;
using WebScrap.Css.Traversing.States;
using WebScrap.Css.Traversing.Validators;

namespace WebScrap.Css.Traversing;

internal interface ICssTraverser
{
    public CssValidatorBase Validator { get; }
}

internal interface ICssTracker
{

}

internal class CssTracker : ICssTracker
{
    public readonly Stack<CssTokenBase> cssCompliantTags;
    public readonly Stack<OpeningTag> traversedTags;

    public CssTracker(
        Stack<CssTokenBase> cssCompliantTags, 
        Stack<OpeningTag> traversedTags)
    {
        AssertCss(cssCompliantTags);
        this.cssCompliantTags = cssCompliantTags;
        this.traversedTags = traversedTags;
    }

    public bool IsDone 
        => traversedTags.Count == 0 
        || cssCompliantTags.Count == 0;

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

internal class CssTraverser : ICssTraverser
{
    protected readonly CssValidatorBase validator;
    private readonly CssTracker cssTracker;

    public CssValidatorBase Validator => validator;

    public CssTraverser(CssValidatorBase validator, CssTracker cssTracker)
    {
        this.validator = validator;
        this.cssTracker = cssTracker;
    }

    internal bool Traverse()
    {
        TraversingStateBase? currentTraverser = new InitialTraversingState(this);
        do
        {
            // its just a mess and doesnt work.
            var (css, tag) = (
                cssTracker.cssCompliantTags.Peek(), 
                cssTracker.traversedTags.Pop());
            var nextTraverser = currentTraverser.Traverse(css, tag);

            if (currentTraverser.IsValid(css, tag))
            {
                cssTracker.cssCompliantTags.Pop();
            }
            else
            {
                if (currentTraverser is StrictTraversingState)
                {
                    return false;
                }
            }
            currentTraverser = nextTraverser;

        } while (!cssTracker.IsDone);

        return cssTracker.cssCompliantTags.Count == 0;
    }
}