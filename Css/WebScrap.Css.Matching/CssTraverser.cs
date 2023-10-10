
using WebScrap.Common.Tags;
using WebScrap.Common.Css;
using WebScrap.Common.Css.Selectors;
using WebScrap.Css.Matching.Comparers;

namespace WebScrap.Css.Matching;

internal class CssTraverser(
    IComparer comparer, 
    CssTracker cssTracker)
{
    private readonly CssTracker cssTracker = cssTracker;
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