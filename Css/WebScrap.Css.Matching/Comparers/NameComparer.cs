using WebScrap.Common.Tags;
using WebScrap.Css.Data;
using WebScrap.Css.Data.Tags;

namespace WebScrap.Css.Matching.Comparers;

public class NameComparer : IComparer
{
    public bool AreSame(CssToken css, OpeningTag tag) 
        => AreSame(css, tag.Name);
    
    public bool AreSame(CssToken css, ReadOnlySpan<char> tagName) 
        => css.Tag switch
        {
            WildcardCssTag => true,
            CssTag tag => tagName.SequenceEqual(tag.Name),
            _ => throw new InvalidOperationException("Uknown tag type"),
        };
}
