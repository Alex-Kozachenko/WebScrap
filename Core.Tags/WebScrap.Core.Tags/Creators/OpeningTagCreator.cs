using WebScrap.Common.Tags;
using WebScrap.Core.Tags.Extractors;

namespace WebScrap.Core.Tags.Creators;

internal class OpenedTagCreator
{
    public OpenedTag CreateOpenedTag(
        ReadOnlySpan<char> html, 
        int charsProcessed)
    {
        var tagInfo = CreateTagInfo(html);
        int? innerOffset = tagInfo switch
        {
            // InlineTag => null, + 3
            _ => html.Length + 2
        };

        return new OpenedTag(
            tagInfo,
            charsProcessed, 
            charsProcessed + innerOffset);
    }

    private TagInfo CreateTagInfo(ReadOnlySpan<char> tagContent)
    {
        var index = tagContent.IndexOf(' ');
        if (index == -1)
        {
            return new TagInfo(
                tagContent.ToString(), 
                AttributeExtractor.EmptyAttributes);
        }
        var name = tagContent[..index];
        tagContent = tagContent[tagContent.IndexOf(' ')..][1..];
        
        return new TagInfo(
            name.ToString(), 
            AttributeExtractor.GetAttributes(tagContent));
    }
}