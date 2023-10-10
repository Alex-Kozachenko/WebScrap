using WebScrap.Common.Tags;

namespace WebScrap.Common.Contracts;

public interface ITagCreator
{
    public TagBase Create(ReadOnlySpan<char> tag);
}