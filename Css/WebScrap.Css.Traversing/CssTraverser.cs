using WebScrap.Css.Common.Tokens;
using WebScrap.Css.Common.Comparers;

namespace WebScrap.Css.Traversing;

internal static class CssTraverser
{
    public static bool Traverse(IComparer comparer, CssTracker cssTracker)
    {
        var isLazy = true;
        while (cssTracker.IsEmpty is false)
        {
            var (css, tag) = cssTracker.Peek();
            var areEqual = comparer.AreSame(css, tag);
            if (areEqual)
            {
                cssTracker.PopCss();
            }
            else if (isLazy)
            {
                return false;
            }

            isLazy = IsNextModeLazy(css);
            cssTracker.PopTag();
        }

        return cssTracker.IsCompleted;
    }

    private static bool IsNextModeLazy(CssTokenBase lastAcceptedTag) 
        => lastAcceptedTag switch
        {
            DirectChildCssToken => true,
            _ => false
        };
}