using WebScrap.Common.Tags;
using WebScrap.Css.Common.Tokens;

namespace WebScrap.Css.Traversing;

internal class CssTracker
{
    internal readonly Stack<CssTokenBase> expectedTags;
    internal readonly Stack<OpeningTag> traversedTags;

    internal CssTracker(
        IReadOnlyCollection<CssTokenBase> expectedTags, 
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

    internal (CssTokenBase, OpeningTag) Peek()
        => (expectedTags.Peek(), traversedTags.Peek());

    internal void PopCss() => expectedTags.Pop();
    internal void PopTag() => traversedTags.Pop();

    internal void Clear()
    {
        traversedTags.Clear();
    }

    private static void AssertCss(IReadOnlyCollection<CssTokenBase> css)
    {
        _ = css.ToArray() switch 
        {
            var a when a.First() is not RootCssToken
                => throw new ArgumentException("Incorrect css structure. First element should be root."),
            var a when a.Skip(1).Any(x => x is RootCssToken)
                => throw new ArgumentException("Incorrect css structure. Only first element could be root."),
            _ => 0,
        };
    }
}
