using WebScrap.Common.Tags;
using WebScrap.Core.Tags.Extractors;

namespace WebScrap.Core.Tags.Creators;

public class OpeningTagCreator : ITagCreator
{
    public TagBase Create(ReadOnlySpan<char> tagContent)
    {
        var index = tagContent.IndexOf(' ');
        if (index == -1)
        {
            return new OpeningTag(
                tagContent.ToString(), 
                AttributeExtractor.EmptyAttributes);
        }
        var name = tagContent[..index];
        tagContent = tagContent[tagContent.IndexOf(' ')..][1..];
        
        return new OpeningTag(
            name.ToString(), 
            AttributeExtractor.GetAttributes(tagContent));
    }
}