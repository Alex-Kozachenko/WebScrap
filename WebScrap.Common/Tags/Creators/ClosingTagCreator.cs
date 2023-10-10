using WebScrap.Common.Tags;
using WebScrap.Common.Contracts;

namespace WebScrap.Common.Tags.Creators;

public class ClosingTagCreator : ITagCreator
{
    public TagBase Create(ReadOnlySpan<char> tag)
        => new ClosingTag(tag.ToString());
}