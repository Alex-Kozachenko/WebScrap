using WebScrap.Common.Tags;
using WebScrap.Css.Common.Tokens;
namespace WebScrap.Css.Traversing.States;

internal abstract class TraversingStateBase(ICssTraverser traverser)
{
    internal abstract TraversingStateBase? Traverse(
        CssTokenBase currentCss, 
        OpeningTag currentTag);

    internal bool IsValid(
        CssTokenBase currentCss, 
        OpeningTag currentTag)
        => traverser.Validator.IsValid(currentCss, currentTag);

    protected TraversingStateBase GetNextState(CssTokenBase lastAcceptedTag)
    {
        return lastAcceptedTag switch
        {
            DirectChildCssToken => new StrictTraversingState(traverser),
            AnyChildCssToken => new FreeTraversingState(traverser),
            RootCssToken => new FinalTraversingState(traverser),
            _ => throw new Exception()
        };
    }

    
}