using WebScrap.Common.Tags;
using WebScrap.Css.Common.Tokens;

namespace WebScrap.Css.Traversing.States;

/// <summary>
/// The very first state, before the first css tag is known.
/// </summary>
internal class InitialTraversingState(ICssTraverser traverser) 
    : TraversingStateBase(traverser)
{
    internal override TraversingStateBase? Traverse(
        CssTokenBase currentCss, OpeningTag currentTag)
    {
        return GetNextState(currentCss);
    }
}