using WebScrap.Common.Tools;
using WebScrap.Common.Tags.Creators;

namespace WebScrap.Common.Tags;

public class TagFactory(ITagCreatorResolver resolver) : ITagCreator
{
    public TagBase Create(ReadOnlySpan<char> tag)
    {
        var tagCreator = resolver.Resolve(tag);
        tag = tag.Clip("<", ">");
        return tag.IndexOf('/') switch 
        {
            0 => tagCreator.Create(tag[1..]),
            _ => tagCreator.Create(tag),
        };
    }
}