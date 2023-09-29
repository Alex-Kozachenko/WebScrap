using static WebScrap.Tags.Attributes.AttributeExtractor;

namespace WebScrap.Tags.Creators;

internal class OpeningTagCreator : TagCreatorBase
{
    protected override TagBase CreateTag(ReadOnlySpan<char> tagContent)
    {
        var index = tagContent.IndexOf(' ');
        if (index == -1)
        {
            return new OpeningTag(tagContent.ToString(), EmptyAttributes);
        }
        var name = tagContent[..index];
        tagContent = tagContent[tagContent.IndexOf(' ')..][1..];
        
        return new OpeningTag(
            name.ToString(), 
            GetAttributes(tagContent));
    }
}