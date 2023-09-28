using WebScrap.Tags.Tools;

namespace WebScrap.Tags;

public abstract record class TagBase(string Name)
{
    public static TagBase Create(ReadOnlySpan<char> tag) 
    {
        tag = tag.Clip("<", ">");
        return tag.IndexOf('/') switch
        {
            0 => ClosingTag.Create(tag[1..]),
            var i when i == tag.Length - 1
              => SelfClosingTag.Create(tag[..^1]),
            _ => OpeningTag.Create(tag)
        };
    }
}