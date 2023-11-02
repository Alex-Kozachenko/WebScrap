using System.Collections.Immutable;

namespace WebScrap.Core.Tags.Data;

public record class TagsProviderMessage(
    ImmutableArray<TagInfo> TagsHistory,
    ProcessedTag CurrentTag);