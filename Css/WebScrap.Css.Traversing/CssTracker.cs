using WebScrap.Common.Tags;
using WebScrap.Css.Common;
using WebScrap.Css.Common.Selectors;


namespace WebScrap.Css.Traversing;

internal class CssTracker
{
    internal readonly Stack<CssToken> expectedTags;
    internal readonly Stack<OpeningTag> traversedTags;

    internal CssTracker(
        IReadOnlyCollection<CssToken> expectedTags, 
        IReadOnlyCollection<OpeningTag> traversedTags)
    {
        AssertCss(expectedTags);
        this.expectedTags = new(expectedTags);
        this.traversedTags = new(traversedTags);
    }

    internal bool IsEmpty
        => traversedTags.Count == 0 
        || expectedTags.Count == 0;

    internal bool IsCompleted
        => expectedTags.Count == 0;

    internal (CssToken, OpeningTag) Peek()
        => (expectedTags.Peek(), traversedTags.Peek());

    internal void PopCss() => expectedTags.Pop();
    internal void PopTag() => traversedTags.Pop();

    internal void Clear()
    {
        traversedTags.Clear();
    }

    private static void AssertCss(IReadOnlyCollection<CssToken> css)
    {
        var selectors = css.Select(x => x.Selector).ToArray();
        
        if (selectors.First() is not RootCssSelector)
        {
            throw new ArgumentException("Incorrect css structure. First element should be root.");
        }

        if (selectors.Skip(1).Any(x => x is RootCssSelector))
        {
            throw new ArgumentException("Incorrect css structure. Only first element could be root.");
        }
    }
}
