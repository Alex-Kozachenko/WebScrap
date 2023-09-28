using static WebScrap.Tags.Tools.AttributeExtractor;

namespace WebScrap.Tags.Creators;

internal class OpeningTagCreator : TagCreatorBase
{
    protected override TagBase CreateTag(ReadOnlySpan<char> tagContent)
    {
        var index = tagContent.IndexOf(' ');
        if (index == -1)
        {
            return CreateInnerText(tagContent, tagContent, null);
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
            values.ToLookup(x => key, x => x));
    }

    // Should be lazy
    private static OpeningTag CreateInnerText(
        ReadOnlySpan<char> tagContent,
        ReadOnlySpan<char> tagName,
        ILookup<string, string>? tagAttributes)
        => new(
            Name: tagName.ToString(),
            Attributes: tagAttributes ?? Array.Empty<int>().ToLookup(x => "", x => ""));
}