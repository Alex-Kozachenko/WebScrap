using WebScrap.Common.Tags;
using WebScrap.Css.Common.Tokens;

namespace WebScrap.Css.Common.Comparers;

public interface IComparer
{
    public bool AreSame(CssTokenBase css, OpeningTag tag);
}
