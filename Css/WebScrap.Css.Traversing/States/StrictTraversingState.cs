using WebScrap.Common.Tags;
using WebScrap.Css.Common.Tokens;

namespace WebScrap.Css.Traversing.States;

/// <summary>
/// Represents a strict descendance.
/// </summary>
/// <remarks>
/// EXAMPLE:
/// main>div
/// </remarks>
internal sealed class StrictTraversingState(ICssTraverser traverser) 
    : TraversingStateBase(traverser)
{
    internal override TraversingStateBase? Traverse(
        CssTokenBase currentCss, 
        OpeningTag currentTag)
    {
        if (IsValid(currentCss, currentTag))
        {
            return GetNextState(currentCss);
        }
        return this;
    }
}