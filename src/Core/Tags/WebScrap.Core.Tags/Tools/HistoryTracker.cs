using WebScrap.Core.Tags.Data;
using WebScrap.Core.Tags.Tools.Extracting;

namespace WebScrap.Core.Tags.Tools;

internal class HistoryTracker
{
    private readonly Stack<UnprocessedTag> openedTags = new();

    public UnprocessedTag[] History => openedTags.Reverse().ToArray();

    public void Update(ReadOnlySpan<char> currentHtml, int offset)
    {
        switch (TagDetector.Detect(currentHtml))
        {
            case TagKind.Opening:
            {
                var tag = currentHtml.Clip("<", ">");
                openedTags.Push(new(
                    CreateTagInfo(tag),
                    TagOffset: offset,
                    InnerOffset: offset + tag.Length));
                break;
            }
            case TagKind.Closing:
            {
                openedTags.Pop();
                break;
            }
        }
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