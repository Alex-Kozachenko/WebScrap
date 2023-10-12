using WebScrap.Common.Tags;

namespace WebScrap.Core.Tags.Creators;

internal interface ITagCreator
{
    public TagBase Create(ReadOnlySpan<char> tag);
}