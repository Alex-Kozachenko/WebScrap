using WebScrap.Common.Tags;
using WebScrap.Common.Css;

namespace WebScrap.Css.Matching.Comparers;

public interface IComparer
{
    public bool AreSame(CssToken css, OpeningTag tag);
}
