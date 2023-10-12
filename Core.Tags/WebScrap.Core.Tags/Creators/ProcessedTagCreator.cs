namespace WebScrap.Core.Tags.Creators;

internal class ProcessedTagCreator
{
    public ProcessedTag CreateProcessedTag(
        ReadOnlySpan<char> html, 
        OpenedTag latestTag, 
        int closingTagOffset)
    {
        var tagLength = closingTagOffset + html.Length + 3;
        var range = latestTag.TagOffset..tagLength;
        var innerRange = latestTag.InnerOffset switch 
            {
                null => 0..0,
                var o => o.Value..closingTagOffset
            };
        return new(latestTag.Metadata, range, innerRange);
    }
}