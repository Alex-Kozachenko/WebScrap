namespace WebScrap.Tags;

public sealed record class ClosingTag(ReadOnlyMemory<char> Name)
    : TagBase(Name)
{
    public static new ClosingTag Create(ReadOnlySpan<char> tagContent)
        => new(tagContent.ToString().AsMemory());
}
