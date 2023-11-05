namespace WebScrap.Core.Tags.Data;

/// <summary>
/// Represents an opened tag, when there is incomplete data about it.
/// </summary>
public record class UnprocessedTag(
    TagInfo TagInfo,
    int TagOffset, 
    int? InnerOffset);