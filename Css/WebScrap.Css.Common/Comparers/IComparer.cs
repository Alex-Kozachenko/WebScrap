using WebScrap.Common.Tags;

namespace WebScrap.Css.Common.Comparers;

public interface IComparer
{
    public bool AreSame(CssToken css, OpeningTag tag);
}
