using WebScrap.Common.Tags;

namespace WebScrap.Core.Tags.Creators;

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