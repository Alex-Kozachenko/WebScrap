using WebScrap.Core.Tags;
using WebScrap.Css.Data;
using WebScrap.Css.Data.Selectors;

namespace WebScrap.Css.Matching.Core;

internal class CssTracker
{
    internal readonly Stack<CssToken> expectedTags;
    internal readonly Stack<TagInfo> tagsHistory;

    internal CssTracker(
        CssToken[] expectedTags, 
        TagInfo[] tagsHistory)
    {
        AssertCssStructure(expectedTags);

        this.expectedTags = new(expectedTags);
        this.tagsHistory = new(tagsHistory);
    }

    internal bool IsEmpty
        => tagsHistory.Count == 0 
        || expectedTags.Count == 0;

    internal bool IsCompleted
        => expectedTags.Count == 0;

    internal (CssToken, TagInfo) Peek()
        => (expectedTags.Peek(), tagsHistory.Peek());

    internal void PopCss() => expectedTags.Pop();
    internal void PopTag() => tagsHistory.Pop();

    internal void Clear()
    {
        tagsHistory.Clear();
    }

    private static void AssertCssStructure(IReadOnlyCollection<CssToken> css)
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
