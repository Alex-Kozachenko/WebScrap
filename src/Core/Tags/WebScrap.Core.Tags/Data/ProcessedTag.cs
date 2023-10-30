namespace WebScrap.Core.Tags.Data;

/// <summary>
/// Represents the completed tag with metadata and offsets from begin to end.
/// </summary>
public record class ProcessedTag(
    TagInfo TagInfo, 
    Range TagRange, 
    Range InnerTextRange)
{
    public int Length => TagRange.End.Value - TagRange.Start.Value;

    public int TextLength => InnerTextRange.End.Value - InnerTextRange.Start.Value;
}