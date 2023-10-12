using System.Collections.Immutable;
using WebScrap.Core.Tags.Creators;

namespace WebScrap.Core.Tags;

public class TagsProcessorBase
{
    private readonly TagObserver tagObserver;
    private readonly Stack<OpenedTag> openedTags;
    private readonly Queue<ProcessedTag> processedTags;

    public TagsProcessorBase()
    {
        openedTags = new();
        processedTags = new();

        tagObserver = new(
            openedTagListeners: [
                o => Process([.. openedTags.Reverse()], o),
                o => openedTags.Push(o)
            ],
            processedTagListeners: [
                o => Process([.. openedTags.Reverse()], o),
                o => openedTags.Pop(),
                o => processedTags.Enqueue(o)
            ]);
    }

    public ImmutableArray<ProcessedTag> Process(ReadOnlySpan<char> html)
    {
        var charsProcessed = 0;
        openedTags.Clear();
        processedTags.Clear();

        do
        {
            charsProcessed += Process(html, charsProcessed);
        } while (charsProcessed < html.Length && openedTags.Count != 0);

        return [.. processedTags.Reverse()];
    }

    protected virtual void Process(OpenedTag[] openedTags, OpenedTag openedTag) { }
    protected virtual void Process(OpenedTag[] openedTags, ProcessedTag tag) { }

    private int Process(ReadOnlySpan<char> html, int charsProcessed)
    {
        var currentHtml = html[charsProcessed..];
        openedTags.TryPeek(out var lastOpenedTag);
        tagObserver.Run(currentHtml, lastOpenedTag, charsProcessed);
        return TagsNavigator.GetNextTagIndex(currentHtml[1..]) + 1;
    }
}