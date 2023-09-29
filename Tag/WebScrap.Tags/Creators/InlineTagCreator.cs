namespace WebScrap.Tags.Creators;

internal class InlineTagCreator : OpeningTagCreator
{
    protected override TagBase CreateTag(ReadOnlySpan<char> tagContent)
    {
        return Cast((OpeningTag) base.CreateTag(tagContent));
    }

    private static TagBase Cast(OpeningTag tag)
    {
        return new InlineTag(tag.Name, tag.Attributes);
    }
}