using WebScrap.Core.Tags.Extracting;

namespace WebScrap.Core.Tags.Creating;

internal class UnprocessedTagCreator(
    List<Action<UnprocessedTag>> unprocessedTagListeners,
    int charsProcessed)
{
    internal void Run(ReadOnlySpan<char> tag) 
    {
        tag = tag.Clip("<", ">");
        var result = new UnprocessedTag(
            CreateTagInfo(tag),
            TagOffset: charsProcessed,
            InnerOffset: charsProcessed + tag.Length);

        unprocessedTagListeners.ForEach(x => x(result));
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