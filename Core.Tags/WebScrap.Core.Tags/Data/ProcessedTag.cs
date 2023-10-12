using WebScrap.Common.Tags;

namespace WebScrap.Core.Tags.Data;

public record class ProcessedTag(
    OpeningTag Metadata, 
    Range TagRange, 
    Range InnerTextRange)
{
    public int Length => TagRange.End.Value - TagRange.Start.Value;

    public int TextLength => InnerTextRange.End.Value - InnerTextRange.Start.Value;
}