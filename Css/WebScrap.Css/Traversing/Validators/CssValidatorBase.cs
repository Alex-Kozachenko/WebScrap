using WebScrap.Common.Tags;
using WebScrap.Css.Common.Tokens;

namespace WebScrap.Css.Traversing.Validators;

// TODO: better name please.
internal abstract class CssValidatorBase
{
    public abstract bool IsValid(CssTokenBase currentCssToken, OpeningTag currentTag);
}

