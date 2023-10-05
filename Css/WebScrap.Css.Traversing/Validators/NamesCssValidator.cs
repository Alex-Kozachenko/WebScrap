using WebScrap.Common.Tags;
using WebScrap.Css.Common.Tokens;

namespace WebScrap.Css.Traversing.Validators;

internal sealed class NamesCssValidator : CssValidatorBase
{
    public override bool IsValid(CssTokenBase currentCssToken, OpeningTag currentTag)
        => currentCssToken.Name.SequenceEqual(currentTag.Name);
}
