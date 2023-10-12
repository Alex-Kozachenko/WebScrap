namespace WebScrap.Core.Tags.Data;

public record class TagRanges(Range Range, Range InnerTextRange)
{
    public int Length => Range.End.Value - Range.Start.Value;
}