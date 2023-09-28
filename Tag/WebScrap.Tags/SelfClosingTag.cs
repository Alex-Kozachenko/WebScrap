using AttributesList = System.Collections.Generic.List<
    System.Collections.DictionaryEntry>;

namespace WebScrap.Tags;

public sealed record class SelfClosingTag(
    string Name,
    AttributesList Attributes)
        : TagBase(Name)
{
    public static new SelfClosingTag Create(ReadOnlySpan<char> tagContent)
    {
        throw new NotImplementedException();
    }
}