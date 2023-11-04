using WebScrap.Core.Tags.Data;
using WebScrap.Core.Tags.Extracting;

namespace WebScrap.Core.Tags.Creating;

internal readonly ref struct OpeningTagCreator(ReadOnlySpan<char> html, int charsProcessed)
{
    private readonly ReadOnlySpan<char> html = html;
    private readonly int charsProcessed = charsProcessed;

    internal UnprocessedTag Create() 
    {
        var tag = html.Clip("<", ">");
        return new(
            CreateTagInfo(tag),
            TagOffset: charsProcessed,
            InnerOffset: charsProcessed + tag.Length);
    }

    static TagInfo CreateTagInfo(ReadOnlySpan<char> tag)
    {
        if (tag.IndexOf(' ') == -1)
        {
            var name = tag.Clip("<", ">", true);
            return new TagInfo(
                name.ToString(), 
                AttributeExtractor.EmptyAttributes);
        }
        else
        {
            var name = tag.Clip("<", " ", true);
            var attributes = tag.Clip(" ", ">", true);
            return new TagInfo(
                name.ToString(), 
                AttributeExtractor.GetAttributes(attributes));
        }
    }
}