using static WebScrap.Tags.Tools.AttributeExtractor;

namespace WebScrap.Tags;

public sealed record class OpeningTag(
    string Name,
    ILookup<string, string> Attributes,
    string InnerText)
        : TagBase(Name)
{
    public static new OpeningTag Create(ReadOnlySpan<char> tagContent)
    {
        var name = tagContent[..tagContent.IndexOf(' ')];
        tagContent = tagContent[tagContent.IndexOf(' ')..][1..];
        return CreateAttributes(tagContent, name);
    }

    private static OpeningTag CreateAttributes(
        ReadOnlySpan<char> tagContent,
        ReadOnlySpan<char> tagName)
    {
        var key = GetKey(tagContent).ToString();
        var values = GetValues(tagContent);
        return CreateInnerText(
            tagContent, 
            tagName, 
            values.ToLookup(x => key, x => x));
    }

    private static OpeningTag CreateInnerText(
        ReadOnlySpan<char> tagContent,
        ReadOnlySpan<char> tagName,
        ILookup<string, string> tagAttributes)
        => new(
            Name: tagName.ToString(),
            Attributes: tagAttributes,
            InnerText: string.Empty);
}