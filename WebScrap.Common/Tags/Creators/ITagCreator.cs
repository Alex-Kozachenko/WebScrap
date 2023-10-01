namespace WebScrap.Common.Tags.Creators;

public interface ITagCreator
{
    public TagBase Create(ReadOnlySpan<char> tag);
}