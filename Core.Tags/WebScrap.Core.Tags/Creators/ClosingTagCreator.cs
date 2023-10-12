using WebScrap.Common.Tags;

namespace WebScrap.Core.Tags.Creators;

internal class ClosingTagCreator : ITagCreator
{
    public TagBase Create(ReadOnlySpan<char> tag)
        => new ClosingTag(tag.ToString());
}