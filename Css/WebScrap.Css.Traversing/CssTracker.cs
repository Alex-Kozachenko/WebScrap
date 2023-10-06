using WebScrap.Common.Tags;
using WebScrap.Css.Common.Tokens;

namespace WebScrap.Css.Traversing;

internal class CssTracker
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

    public bool IsEmpty
        => traversedTags.Count == 0 
        || cssCompliantTags.Count == 0;

    public bool IsCompleted
        => cssCompliantTags.Count == 0;

    public (CssTokenBase, OpeningTag) Peek()
        => (cssCompliantTags.Peek(), traversedTags.Peek());

    public void PopCss() => cssCompliantTags.Pop();
    public void PopTag() => traversedTags.Pop();

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
