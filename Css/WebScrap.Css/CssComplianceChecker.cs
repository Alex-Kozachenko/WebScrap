using WebScrap.Common.Tags;
using WebScrap.Css.Listeners.Helpers;
using WebScrap.Css.Preprocessing.Tokens;

namespace WebScrap.Css;

internal class CssCompliantChecker(
    Stack<OpeningTag> traversedTags, 
    Stack<CssOpeningTag> cssCompliantTags)
{
    internal bool CheckLength()
        => cssCompliantTags.Count <= traversedTags.Count;

    internal bool CheckNames()
    {
        var cssTags = new Stack<CssOpeningTag>(cssCompliantTags.Reverse());
        var travTags = new Stack<OpeningTag>(traversedTags.Reverse());
        while (cssTags.Count != 0)
        {
            var travTag = travTags.Pop();
            var cssTag = cssTags.Pop();
            if (!cssTag.Name.SequenceEqual(travTag.Name))
            {
                return false;
            }
        }
        return true;
    }

    internal bool CheckAttributes()
    {
        var cssTags = new Stack<OpeningTag>(cssCompliantTags.Reverse());
        var travTags = new Stack<OpeningTag>(traversedTags.Reverse());
        while (cssTags.Count != 0)
        {
            var travTag = travTags.Pop();
            var cssTag = cssTags.Pop();

            if (!AttributesComparer.IsSubsetOf(cssTag.Attributes, travTag.Attributes))
            {
                return false;
            }
        }
        return true;
    }
}