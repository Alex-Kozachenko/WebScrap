using WebScrap.Common.Tags;
using WebScrap.Common.Tags.Creators;

namespace WebScrap.Tags.Creators;

internal class ClosingTagCreator : ITagCreator
{
    public TagBase Create(ReadOnlySpan<char> tag)
        => new ClosingTag(tag.ToString());
}