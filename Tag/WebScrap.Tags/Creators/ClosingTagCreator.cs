namespace WebScrap.Tags.Creators;

internal class ClosingTagCreator : TagCreatorBase
{
    protected override TagBase CreateTag(ReadOnlySpan<char> tag)
        => new ClosingTag(tag.ToString());
}