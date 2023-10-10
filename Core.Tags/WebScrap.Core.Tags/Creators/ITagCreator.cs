using WebScrap.Common.Tags;

namespace WebScrap.Core.Tags.Creators;

public interface ITagCreator
{
    public TagBase Create(ReadOnlySpan<char> tag);
}