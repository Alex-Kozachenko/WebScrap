using WebScrap.Common.Tags;
using WebScrap.Css.Common.Tags;

namespace WebScrap.Css.Common.Comparers;

public class NameComparer : IComparer
{
    public bool AreSame(CssToken css, OpeningTag tag) 
        => AreSame(css, tag.Name);
    
    public bool AreSame(CssToken css, ReadOnlySpan<char> tagName) 
        => css.Tag switch
        {
            AnyCssTag => true,
            CssTag tag => tagName.SequenceEqual(tag.Name),
            _ => throw new InvalidOperationException("Uknown tag type"),
        };
}
