namespace WebScrap.Common.Tags.Creators;

public interface ITagCreatorResolver
{
    public ITagCreator Resolve(ReadOnlySpan<char> tag);
}