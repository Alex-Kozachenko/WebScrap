using WebScrap.Common.Contracts;
using WebScrap.Common.Tools;
using WebScrap.Tags.Creators;

namespace WebScrap.Tags;

internal class TagFactory : TagFactoryBase
{
    protected override ITagCreator GetTagCreator(ReadOnlySpan<char> tag)
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