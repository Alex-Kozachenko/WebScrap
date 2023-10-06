using WebScrap.Common.Tags;
using WebScrap.Css.Common.Tokens;

namespace WebScrap.Css.Common.Comparers;

public class NameComparer : IComparer
{
    public bool AreSame(CssTokenBase css, OpeningTag tag) 
        => AreSame(css, tag.Name);
    
    public bool AreSame(CssTokenBase css, ReadOnlySpan<char> tagName) 
        => tagName.SequenceEqual(css.Name);
}
