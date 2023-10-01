using WebScrap.Common.Tools;

namespace WebScrap.Common.Tags.Creators;

public abstract class TagFactoryBase
{
    public TagBase CreateTagBase(ReadOnlySpan<char> tag)
    {
        var tagCreator = GetTagCreator(tag);
        tag = tag.Clip("<", ">");
        return tag.IndexOf('/') switch 
        {
            0 => tagCreator.Create(tag[1..]),
            _ => tagCreator.Create(tag),
        };
    }

    protected abstract ITagCreator GetTagCreator(ReadOnlySpan<char> tag);
}