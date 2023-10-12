namespace WebScrap.Core.Tags;

public record class ProcessedTag(
    TagInfo Metadata, 
    Range TagRange, 
    Range InnerTextRange)
{
    public int Length => TagRange.End.Value - TagRange.Start.Value;

    public int TextLength => InnerTextRange.End.Value - InnerTextRange.Start.Value;
}