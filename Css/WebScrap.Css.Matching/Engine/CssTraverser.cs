
using WebScrap.Css.Data.Selectors;
using WebScrap.Css.Matching.Comparers;

namespace WebScrap.Css.Matching.Engine;

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
        var (css, tagInfo) = cssTracker.Peek();
        var areEqual = comparer.AreSame(css, tagInfo);
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

        var (css, tagInfo) = cssTracker.Peek();
        var areEqual = comparer.AreSame(css, tagInfo);
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