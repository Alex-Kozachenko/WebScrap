using WebScrap.Css.Common.Tokens;
using WebScrap.Css.Common.Comparers;

namespace WebScrap.Css.Traversing;

internal static class CssTraverser
{
    public static bool Traverse(IComparer comparer, CssTracker cssTracker)
    {
        var isGreedy = false;
        CssTokenBase? lastAcceptedTag = null;
        while (cssTracker.IsEmpty is false)
        {
            var (css, tag) = cssTracker.Peek();
            var areEqual = comparer.AreSame(css, tag);
            if (areEqual)
            {
                lastAcceptedTag = css;
                cssTracker.PopCss();
            }
            else if (!isGreedy)
            {
                return false;
            }

            isGreedy = IsNextModeGreedy(lastAcceptedTag);
            cssTracker.PopTag();
        }

        return cssTracker.IsCompleted;
    }

    private static bool IsNextModeGreedy(CssTokenBase? lastAcceptedTag) 
        => lastAcceptedTag switch
        {
            DirectChildCssToken => false,
            _ => true
        };
}