using static WebScrap.Tags.Tools.AttributeExtractor;

namespace WebScrap.Tags;

public sealed record class OpeningTag(
    string Name,
    ILookup<string, string> Attributes,
    string? InnerText)
        : TagBase(Name)
{
    public bool IsSelfClosing = InnerText == null;
    public static new OpeningTag Create(ReadOnlySpan<char> tagContent)
    {
        var index = tagContent.IndexOf(' ');
        if (index == -1)
        {
            var isSelfClosing = tagContent.EndsWith("/");
            return CreateInnerText(tagContent, tagContent, null, isSelfClosing);
        }
        var name = tagContent[..index];
        tagContent = tagContent[tagContent.IndexOf(' ')..][1..];
        return CreateAttributes(tagContent, name);
    }

    private static OpeningTag CreateAttributes(
        ReadOnlySpan<char> tagContent,
        ReadOnlySpan<char> tagName)
    {
        var isSelfClosing = tagContent.EndsWith("/");
        tagContent = tagContent.TrimEnd('/');
        var key = GetKey(tagContent).ToString();
        var values = isSelfClosing ? [] : GetValues(tagContent);
        return CreateInnerText(
            tagContent, 
            tagName, 
            values.ToLookup(x => key, x => x),
            isSelfClosing);
    }

    private static OpeningTag CreateInnerText(
        ReadOnlySpan<char> tagContent,
        ReadOnlySpan<char> tagName,
        ILookup<string, string>? tagAttributes,
        bool isSelfClosing)
        => new(
            Name: tagName.ToString(),
            Attributes: tagAttributes ?? Array.Empty<int>().ToLookup(x => "", x => ""),
            InnerText: isSelfClosing ? null : string.Empty);
}