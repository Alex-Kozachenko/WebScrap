using System.Collections.Immutable;
using WebScrap.Core.Tags.Creating;

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

    private int Process(ReadOnlySpan<char> html, int charsProcessed)
    {
        var currentHtml = html[charsProcessed..];

        if (currentHtml.StartsWith("<!--"))
        {
            return SkipComment(currentHtml);
        }

        openedTags.TryPeek(out var lastOpenedTag);

        var detector = new TagDetector(
            unprocessedTagListeners,
            processedTagListeners,
            new UnprocessedTagCreator(charsProcessed),
            new ProcessedTagCreator(charsProcessed, lastOpenedTag!));
        
        detector.Detect(currentHtml);

        return TagsNavigator.GetNextTagIndex(currentHtml[1..]) + 1;
    }

    private int SkipComment(ReadOnlySpan<char> html)
    {
        var specialCharIndex = html.IndexOf('-');
        if (html[specialCharIndex + 1] == '>')
        {
            return specialCharIndex + 2;
        }
        else
        {
            return specialCharIndex + 1 +  SkipComment(html[1..][specialCharIndex..]);
        }
    }
}