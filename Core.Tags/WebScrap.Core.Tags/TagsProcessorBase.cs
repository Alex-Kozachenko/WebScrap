using WebScrap.Common.Tags;
using WebScrap.Core.Tags.Creators;
using WebScrap.Core.Tags.Data;

namespace WebScrap.Core.Tags;

public class TagsProcessorBase
{
    private readonly TagFactory tagFactory = new();
    private readonly Stack<TagsHistoryRecord> tagsHistory = new();
    private readonly Queue<(OpeningTag, TagRanges)> processedTagsRangesHistory = new();

    protected OpeningTag[] TagsHistory 
        => tagsHistory
            .Reverse()
            .Select(x => x.Metadata)
            .ToArray();

    public (OpeningTag Tag, TagRanges TagRange)[] ProcessedTagsRanges 
        => processedTagsRangesHistory
            .Reverse()
            .ToArray();

    public void Run(ReadOnlySpan<char> html)
    {
        var charsProcessed = 0;
        tagsHistory.Clear();
        do
        {
            var currentHtml = html[charsProcessed..];
            var tag = ExtractTag(currentHtml);
            Process(tag, currentHtml, charsProcessed);
            charsProcessed += Proceed(currentHtml);
        } while (charsProcessed < html.Length && tagsHistory.Count != 0);
    }

    protected virtual void Process(OpeningTag tag, TagsHistoryRecord tagHistoryRecord) { }
    protected virtual void Process(OpeningTag openingTag, ClosingTag closingTag, TagRanges tagInfo) { }

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

    private void Process(TagBase tag, ReadOnlySpan<char> html, int charsProcessed)
    {
        switch (tag)
        {
            case InlineTag t: break;
            case OpeningTag t: Process(t, html, charsProcessed); break;
            case ClosingTag t: Process(t, html, charsProcessed); break;
        };
    }

    private void Process(OpeningTag tag, ReadOnlySpan<char> html, int charsProcessed)
    {
        var record = new TagsHistoryRecord(
                charsProcessed, 
                charsProcessed + GetInnerOffset(html, tag), 
                tag);
        tagsHistory.Push(record);
        Process(tag, record);
    }

    private void Process(ClosingTag tag, ReadOnlySpan<char> html, int charsProcessed)
    {
        var tagInfo = GetTagInfo(html, tagsHistory.Peek(), charsProcessed);
        Process(tagsHistory.Peek().Metadata, tag, tagInfo);
        processedTagsRangesHistory.Enqueue((tagsHistory.Peek().Metadata, tagInfo));
        tagsHistory.Pop();
    }

    private static int? GetInnerOffset(ReadOnlySpan<char> html, OpeningTag openingTag)
    {
        return openingTag switch
        {
            InlineTag => null,
            _ => html[1..].IndexOf('>') + 1 + 1
        };
    }

    private static TagRanges GetTagInfo(
        ReadOnlySpan<char> html, 
        TagsHistoryRecord latestTag, 
        int closingTagOffset)
    {
        var tagLength = closingTagOffset + html.IndexOf('>') + 1;
        var range = latestTag.TagOffset..tagLength;
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