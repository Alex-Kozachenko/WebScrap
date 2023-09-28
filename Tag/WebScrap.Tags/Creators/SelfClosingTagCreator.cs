namespace WebScrap.Tags.Creators;

internal class SelfClosingTagCreator : OpeningTagCreator
{
    protected override TagBase CreateTag(ReadOnlySpan<char> tagContent)
    {
        return Cast(base.CreateTag(tagContent) as OpeningTag);
    }

    private TagBase Cast(OpeningTag tag)
    {
        return new SelfClosingTag(tag.Name, tag.Attributes);
    }
}