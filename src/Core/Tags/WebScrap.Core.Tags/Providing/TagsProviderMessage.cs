using System.Collections.Immutable;
using WebScrap.Core.Tags.Data;

namespace WebScrap.Core.Tags.Providing;

public record class TagsProviderMessage(
    ImmutableArray<TagInfo> TagsHistory,
    ProcessedTag CurrentTag);