using WebScrap.Common.Tags;
using WebScrap.Common.Contracts;

namespace WebScrap.Tags.Creators;

internal class InlineTagCreator : ITagCreator
{
    public TagBase Create(ReadOnlySpan<char> tagContent)
    {
        return Cast((OpeningTag) new OpeningTagCreator().Create(tagContent));
    }

    private static TagBase Cast(OpeningTag tag)
    {
        return new InlineTag(tag.Name, tag.Attributes);
    }
}