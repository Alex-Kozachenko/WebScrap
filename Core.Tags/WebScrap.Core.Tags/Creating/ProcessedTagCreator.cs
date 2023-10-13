namespace WebScrap.Core.Tags.Creating;

internal class ProcessedTagCreator(
    int closingTagOffset,
    UnprocessedTag latestTag)
{
    internal ProcessedTag Create(ReadOnlySpan<char> tag)
    {
        tag = tag.Clip("<", ">");
        var tagLength = closingTagOffset + tag.Length;
        var range = latestTag.TagOffset..tagLength;
        var innerRange = latestTag.InnerOffset switch 
            {
                null => 0..0,
                var o => o.Value..closingTagOffset
            };
        return new ProcessedTag(latestTag.TagInfo, range, innerRange);
    }
}