using WebScrap.Core.Tags.Data;

namespace WebScrap.Core.Tags.Creating;

internal readonly ref struct ClosingTagCreator(ReadOnlySpan<char> html, int charsProcessed, UnprocessedTag latestTag)
{
    private readonly ReadOnlySpan<char> html = html;
    private readonly int charsProcessed = charsProcessed;
    private readonly UnprocessedTag latestTag = latestTag;

    internal ProcessedTag Create()
    {
        var tag = html.Clip("<", ">");
        var tagLength = charsProcessed + tag.Length;
        var range = latestTag.TagOffset..tagLength;
        var innerRange = latestTag.InnerOffset switch 
            {
                null => 0..0,
                var o => o.Value..charsProcessed
            };
        return new ProcessedTag(latestTag.TagInfo, range, innerRange);
    }
}