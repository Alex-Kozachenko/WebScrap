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
            charsProcessed += Process(html[charsProcessed..], charsProcessed);
        } while (charsProcessed < html.Length && openedTags.Count != 0);

        return [.. processedTags.Reverse()];
    }

    protected virtual void Process(
        UnprocessedTag[] openedTags, 
        UnprocessedTag unprocessedTag) { }
        
    protected virtual void Process(
        UnprocessedTag[] openedTags, 
        ProcessedTag tag) { }

    int Process(ReadOnlySpan<char> currentHtml, int charsProcessed)
    {
        if (!currentHtml.StartsWith("<"))
        {
            throw new ArgumentException($"Html should start with tag. {currentHtml}");
        }

        return currentHtml.Clip("<", ">", true) switch
        {
            var a when a[0] == '!' => ProcessComment(currentHtml),
            var a when a[^1] == '/' => ProcessSelfClosingTag(currentHtml),
            var a when a[0] == '/' => ProcessClosingTag(currentHtml, charsProcessed),
            _ => ProcessOpeningTag(currentHtml, charsProcessed)
        };
    }

    int ProcessComment(ReadOnlySpan<char> currentHtml)
        => CommentsSkipper.TrySkipComment(currentHtml, out var processed)
            ? processed
            : 0;

    int ProcessSelfClosingTag(ReadOnlySpan<char> currentHtml)
        => 1 + TagsNavigator.GetNextTagIndex(currentHtml[1..]);

    int ProcessClosingTag(ReadOnlySpan<char> currentHtml, int charsProcessed)
    {
        // Extract
        openedTags.TryPeek(out var lastOpenedTag);
        var processedTagCreator = new ProcessedTagCreator(charsProcessed, lastOpenedTag!);
        var result = processedTagCreator.Create(currentHtml);
        processedTagListeners.ForEach(x => x(result));
        return 1 + TagsNavigator.GetNextTagIndex(currentHtml[1..]);
    }

    int ProcessOpeningTag(ReadOnlySpan<char> currentHtml, int charsProcessed)
    {
        // Extract
        var unprocessedTagCreator = new UnprocessedTagCreator(charsProcessed);
        // TODO: Please make consistant names, why two names for same object?
        var result = unprocessedTagCreator.Create(currentHtml);
        unprocessedTagListeners.ForEach(x => x(result));
        return 1 + TagsNavigator.GetNextTagIndex(currentHtml[1..]);
    }
}