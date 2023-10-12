using System.Collections.Immutable;
using WebScrap.Common.Tags;
using WebScrap.Core.Tags.Creators;
using WebScrap.Core.Tags.Data;

namespace WebScrap.Core.Tags;

public class TagsProcessorBase
{
    private readonly TagFactory tagFactory = new();
    private readonly Stack<TagsHistoryRecord> tagsHistory = new();
    private readonly Queue<ProcessedTag> processedTags = new();

    protected OpeningTag[] TagsHistory 
        => tagsHistory
            .Reverse()
            .Select(x => x.Metadata)
            .ToArray();

    public ImmutableArray<ProcessedTag> Process(ReadOnlySpan<char> html)
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

        return [.. processedTags.Reverse()];
    }

    protected virtual void Process(OpeningTag tag, TagsHistoryRecord tagHistoryRecord) { }
    protected virtual void Process(ProcessedTag tag) { }

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
            case InlineTag: break;
            case OpeningTag t: ProcessOpeningTag(t, html, charsProcessed); break;
            case ClosingTag: ProcessClosingTag(html, charsProcessed); break;
        };
    }

    private void ProcessOpeningTag(
        OpeningTag tag, 
        ReadOnlySpan<char> html, 
        int charsProcessed)
    {
        var record = new TagsHistoryRecord(
                charsProcessed, 
                charsProcessed + GetInnerOffset(html, tag), 
                tag);
        tagsHistory.Push(record);
        Process(tag, record);
    }

    private void ProcessClosingTag(ReadOnlySpan<char> html, int charsProcessed)
    {
        var processedTag = GetProcessedTag(html, tagsHistory.Peek(), charsProcessed);
        Process(processedTag);
        processedTags.Enqueue(processedTag);

        tagsHistory.Pop();
    }

    private static ProcessedTag GetProcessedTag(
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
        return new(latestTag.Metadata, range, innerRange);
    }

    private static int? GetInnerOffset(ReadOnlySpan<char> html, OpeningTag openingTag)
    {
        return openingTag switch
        {
            InlineTag => null,
            _ => html[1..].IndexOf('>') + 1 + 1
        };
    }
}