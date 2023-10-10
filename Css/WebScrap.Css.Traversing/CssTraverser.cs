
using WebScrap.Css.Common.Comparers;
using WebScrap.Common.Tags;
using WebScrap.Css.Common;
using WebScrap.Css.Common.Selectors;

namespace WebScrap.Css.Traversing;

internal class CssTraverser(
    IComparer comparer, 
    IReadOnlyCollection<CssToken> expectedTags,
    IReadOnlyCollection<OpeningTag> traversedTags)
{
    private readonly CssTracker cssTracker = new(expectedTags, traversedTags);
    private bool isGreedy = false;

    public bool Traverse()
    {
        while (cssTracker.IsEmpty is false)
        {
            Process();
            UpdateTracker();
        }

        return cssTracker.IsCompleted;
    }

    private void Process()
    {
        var (css, tag) = cssTracker.Peek();
        var areEqual = comparer.AreSame(css, tag);
        if (areEqual)
        {
            isGreedy = IsNextModeGreedy(css.Selector);
        }
        else if(isGreedy is false)
        {
            cssTracker.Clear();
        }
    }

    private void UpdateTracker()
    {
        if (cssTracker.IsEmpty)
        {
            return;
        }

        var (css, tag) = cssTracker.Peek();
        var areEqual = comparer.AreSame(css, tag);
        if (areEqual)
        {
            cssTracker.PopCss();
        }
        cssTracker.PopTag();
    }

    private static bool IsNextModeGreedy(CssSelector? lastAcceptedSelector) 
        => lastAcceptedSelector switch
        {
            ChildCssSelector => false,
            _ => true
        };
}