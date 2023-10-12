namespace WebScrap.Core.Tags.Creating;

internal class ProcessedTagCreator(
    List<Action<ProcessedTag>> processedTagListeners,
    int closingTagOffset,
    UnprocessedTag latestTag)
{
    internal void Run(ReadOnlySpan<char> tag)
    {
        tag = tag.Clip("<", ">");
        var tagLength = closingTagOffset + tag.Length;
        var range = latestTag.TagOffset..tagLength;
        var innerRange = latestTag.InnerOffset switch 
            {
                null => 0..0,
                var o => o.Value..closingTagOffset
            };
        var result = new ProcessedTag(latestTag.TagInfo, range, innerRange);
        processedTagListeners.ForEach(x => x(result));
    }
}