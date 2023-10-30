using System.Collections.Immutable;
using WebScrap.Core.Tags.Creating;
using WebScrap.Core.Tags.Data;

namespace WebScrap.Core.Tags;

public class TagsProcessorBase
{
    private readonly Stack<UnprocessedTag> openedTags;
    private readonly Queue<ProcessedTag> processedTags;

    private readonly List<Action<UnprocessedTag>> unprocessedTagListeners;
    private readonly List<Action<ProcessedTag>> processedTagListeners;

    public TagsProcessorBase()
    {
        openedTags = new();
        processedTags = new();

        unprocessedTagListeners = [
                o => Process([.. openedTags.Reverse()], o),
                o => openedTags.Push(o)];
        
        processedTagListeners = [
                o => Process([.. openedTags.Reverse()], o),
                o => openedTags.Pop(),
                o => processedTags.Enqueue(o)];
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

    protected virtual void Process(
        UnprocessedTag[] openedTags, 
        UnprocessedTag unprocessedTag) { }
        
    protected virtual void Process(
        UnprocessedTag[] openedTags, 
        ProcessedTag tag) { }

    int Process(ReadOnlySpan<char> html, int charsProcessed)
    {
        var currentHtml = html[charsProcessed..];

        if (CommentsSkipper.TrySkipComment(currentHtml, out var processed))
        {
            return processed;
        }

        // TODO: refactor this.
        #region HACK
        openedTags.TryPeek(out var lastOpenedTag);
        var detector = new TagDetector(
            unprocessedTagListeners,
            processedTagListeners,
            new UnprocessedTagCreator(charsProcessed),
            new ProcessedTagCreator(charsProcessed, lastOpenedTag!));
    
        detector.Detect(currentHtml);
        #endregion

        return TagsNavigator.GetNextTagIndex(currentHtml[1..]) + 1;
    }
}