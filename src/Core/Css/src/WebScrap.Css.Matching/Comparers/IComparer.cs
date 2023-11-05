using WebScrap.Core.Tags;
using WebScrap.Core.Tags.Data;
using WebScrap.Css.Data;

namespace WebScrap.Css.Matching.Comparers;

public interface IComparer
{
    public bool AreSame(CssToken css, TagInfo tagInfo);
}
