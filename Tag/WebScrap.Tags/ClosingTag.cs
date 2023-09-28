namespace WebScrap.Tags;

public sealed record class ClosingTag(string Name)
    : TagBase(Name)
{
    public static new ClosingTag Create(ReadOnlySpan<char> tagContent)
        => new(tagContent.ToString());
}
