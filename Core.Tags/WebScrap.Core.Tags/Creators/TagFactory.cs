using WebScrap.Common.Tags;
using WebScrap.Common.Tools;

namespace WebScrap.Core.Tags.Creators;

internal class TagFactory
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

    private ITagCreator GetTagCreator(ReadOnlySpan<char> tag)
    {
        tag = tag.Clip("<", ">");
        return tag.IndexOf('/') switch
        {
            0 => new ClosingTagCreator(),
            var i 
                when i == (tag.Length - 1)
              => new InlineTagCreator(),
            _ => new OpeningTagCreator(),
        };
    }
}