using WebScrap.Common.Tags;
using WebScrap.Common.Tags.Extractors;
using WebScrap.Common.Contracts;

namespace WebScrap.Common.Tags.Creators;

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