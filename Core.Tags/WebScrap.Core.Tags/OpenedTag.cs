namespace WebScrap.Core.Tags;

public record class OpenedTag(
    TagInfo Metadata,
    int TagOffset, 
    int? InnerOffset);