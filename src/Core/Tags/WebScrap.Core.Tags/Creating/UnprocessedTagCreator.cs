using WebScrap.Core.Tags.Data;
using WebScrap.Core.Tags.Extracting;

namespace WebScrap.Core.Tags.Creating;

internal class UnprocessedTagCreator(
    int charsProcessed)
{
    internal UnprocessedTag Create(ReadOnlySpan<char> tag) 
    {
        tag = tag.Clip("<", ">");
        return new UnprocessedTag(
            CreateTagInfo(tag),
            TagOffset: charsProcessed,
            InnerOffset: charsProcessed + tag.Length);
    }

    private static TagInfo CreateTagInfo(ReadOnlySpan<char> tag)
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