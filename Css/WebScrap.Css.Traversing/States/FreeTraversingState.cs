using WebScrap.Common.Tags;
using WebScrap.Css.Common.Tokens;

namespace WebScrap.Css.Traversing.States;

/// <summary>
/// Represents a free descendance.
/// </summary>
/// <remarks>
/// 
internal sealed class FreeTraversingState(ICssTraverser traverser) 
    : TraversingStateBase(traverser)
{
    internal override TraversingStateBase? Traverse(CssTokenBase currentCss, OpeningTag currentTag)
    {
        if (IsValid(currentCss, currentTag))
        {
            return GetNextState(currentCss);
        }
        return this;
    }
}