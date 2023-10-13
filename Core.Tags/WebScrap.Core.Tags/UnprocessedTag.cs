namespace WebScrap.Core.Tags;

public record class UnprocessedTag(
    TagInfo TagInfo,
    int TagOffset, 
    int? InnerOffset);