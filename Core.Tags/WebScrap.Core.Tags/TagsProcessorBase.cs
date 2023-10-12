using System.Collections.Immutable;
using System.Diagnostics;
using WebScrap.Common.Tags;
using WebScrap.Core.Tags.Creators;

namespace WebScrap.Core.Tags;

public class TagsProcessorBase
{
    private readonly TagFactory tagFactory;
    private readonly Stack<OpenedTag> openedTags;
    private readonly Queue<ProcessedTag> processedTags;

    private readonly List<Action<OpenedTag>> openedTagsProcessors;
    private readonly List<Action<ProcessedTag>> processedTagsProcessors;

    public TagsProcessorBase()
    {
        tagFactory = new();
        openedTags = new();
        processedTags = new();

        // looks like a command chain.
        openedTagsProcessors = [
            o => Process([.. openedTags.Reverse()], o),
            openedTags.Push
        ];

        processedTagsProcessors = [
            o => Process([.. openedTags.Reverse()], o),
            o => openedTags.Pop(),
            o => processedTags.Enqueue(o)
        ];
    }

    public ImmutableArray<ProcessedTag> Process(ReadOnlySpan<char> html)
    {
        var charsProcessed = 0;
        openedTags.Clear();
        processedTags.Clear();

        do
        {
            var currentHtml = html[charsProcessed..];
            var tag = tagFactory.CreateTagBase(currentHtml);
            charsProcessed += Process(tag, currentHtml, charsProcessed);
        } while (charsProcessed < html.Length && openedTags.Count != 0);

        return [.. processedTags.Reverse()];
    }

    protected virtual void Process(OpenedTag[] openedTags, OpenedTag openedTag) { }
    protected virtual void Process(OpenedTag[] openedTags, ProcessedTag tag) { }

    private int Process(TagBase tag, ReadOnlySpan<char> html, int charsProcessed)
    {
        switch (tag)
        {
            case InlineTag: break;
            case OpeningTag t: 
            {
                var openedTag = CreateOpenedTag(t, html, charsProcessed);
                openedTagsProcessors.ForEach(x => x(openedTag));
                break;
            }
            case ClosingTag: 
            {
                var openedTag = openedTags.Peek();
                var processedTag = CreateProcessedTag(html, openedTag, charsProcessed);
                processedTagsProcessors.ForEach(x => x(processedTag));
                break;
            }
        };

        return TagsNavigator.GetNextTagIndex(html[1..]) + 1;
    }

    private static ProcessedTag CreateProcessedTag(
        ReadOnlySpan<char> html, 
        OpenedTag latestTag, 
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

    private static OpenedTag CreateOpenedTag(
        OpeningTag tag, 
        ReadOnlySpan<char> html, 
        int charsProcessed)
    {
        int? innerOffset = tag switch
        {
            InlineTag => null,
            _ => html[1..].IndexOf('>') + 1 + 1
        };

        return new OpenedTag(
                charsProcessed, 
                charsProcessed + innerOffset, 
                tag);
    }
}