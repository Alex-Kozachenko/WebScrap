using WebScrap.Common.Tags;
using WebScrap.Core.Tags.Creators;
using WebScrap.Core.Tags.Data;

namespace WebScrap.Core.Tags;

/// <summary>
/// Represents a process of linear html-Matching,
/// with help of <see cref="CharsProcessed"/> property.
/// </summary>
/// <remarks>
/// - It is concerned about any html tag, 
/// reacting on opening and closing tag as well.
/// - Highly depends on <see cref="CharsProcessed"/> property value, 
/// which is controlled by derived classes.
/// </remarks>
public class TagsProcessorBase
{
    private readonly TagFactory tagFactory = new();
    private readonly Stack<TagsHistoryRecord> tagsHistory = new();

    // TODO: remove; This field is hellata confusive >:-O
    public int CharsProcessed { get; private set; }
    protected OpeningTag[] TagsHistory 
        => tagsHistory
            .Reverse()
            .Select(x => x.Metadata)
            .ToArray();

    public void Run(ReadOnlySpan<char> html)
    {
        CharsProcessed = 0;
        tagsHistory.Clear();
        do
        {
            Process(html[CharsProcessed..]);
            CharsProcessed += Proceed(html[CharsProcessed..]);
        } while (CharsProcessed < html.Length && tagsHistory.Count != 0);
    }

    protected virtual void Process(OpeningTag tag, TagsHistoryRecord tagHistoryRecord) { }
    protected virtual void Process(ClosingTag tag, TagInfo tagInfo) { }

    private int Proceed(ReadOnlySpan<char> html)
        => TagsNavigator.GetNextTagIndex(html[1..]) + 1;

    private TagBase ExtractTag(ReadOnlySpan<char> html)
    {
        if (!html.StartsWith("<"))
        {
            throw new ArgumentException($"Html should start with tag. {html}");
        }

        return tagFactory.CreateTagBase(html);
    }

    private void Process(ReadOnlySpan<char> html)
    {
        var tag = ExtractTag(html);
        if (tag is InlineTag iTag)
        {
            return;
        }

        if (tag is OpeningTag oTag)
        {
            var record = new TagsHistoryRecord(
                CharsProcessed, 
                CharsProcessed + GetInnerOffset(html, oTag), 
                oTag);
            tagsHistory.Push(record);
            Process(oTag, record);
        }
        
        if (tag is ClosingTag cTag)
        {
            var tagInfo = GetTagInfo(html, tagsHistory.Peek(), CharsProcessed);
            Process(cTag, tagInfo);
            tagsHistory.Pop();
        }
    }

    private static int? GetInnerOffset(ReadOnlySpan<char> html, OpeningTag openingTag)
    {
        return openingTag switch
        {
            InlineTag => null,
            _ => html[1..].IndexOf('>') + 1 + 1
        };
    }

    private static TagInfo GetTagInfo(
        ReadOnlySpan<char> html, 
        TagsHistoryRecord latestTag, 
        int closingTagOffset)
    {
        var tagEnd = closingTagOffset + html.IndexOf('>');
        var range = latestTag.TagOffset..tagEnd;
        var innerRange = latestTag.InnerOffset switch 
            {
                null => 0..0,
                var o => o.Value..closingTagOffset
            };

        return new(
            Range: range,
            InnerTextRange: innerRange
        );
    }

}