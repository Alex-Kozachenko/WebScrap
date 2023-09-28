namespace WebScrap.Tags;

public abstract record class TagBase(ReadOnlyMemory<char> Name)
{
    public static TagBase Create(ReadOnlySpan<char> tag) 
    {
        tag = Clip(tag, "<", ">");
        return tag.IndexOf('/') switch
        {
            0 => ClosingTag.Create(tag[1..]),
            var i
                when i == tag.Length - 1
                =>  SelfClosingTag.Create(tag[..^1]),
            _ => OpeningTag.Create(tag)
        };
    }

    protected static ReadOnlySpan<char> Clip(
        ReadOnlySpan<char> html,
        ReadOnlySpan<char> beginAny,
        ReadOnlySpan<char> endAny)
    {
        var begin = html.IndexOfAny(beginAny);
        var end = begin + 1 + html[begin..][1..].IndexOfAny(endAny);
        return html[begin..end][1..];
    }
}