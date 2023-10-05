using WebScrap.Common.Tags;
using WebScrap.Css.Common.Tokens;
using WebScrap.Css.Common.Helpers;

namespace WebScrap.Css.Traversing.Validators;

internal sealed class AttributesCssValidator : CssValidatorBase
{
    public override bool IsValid(CssTokenBase currentCssToken, OpeningTag currentTag)
        => AttributesComparer.IsSubsetOf(
                currentCssToken.Attributes, 
                currentTag.Attributes);
}