using System.Collections.Immutable;
using WebScrap.Core.Tags.Creating;
using WebScrap.Core.Tags.Data;

namespace WebScrap.Core.Tags;

public class TagsProcessorBase // Or TagsProvider?
{
    private readonly Stack<UnprocessedTag> openedTags;
    private readonly Queue<ProcessedTag> processedTags;

    public TagsProcessorBase()
    {
        openedTags = new();
        processedTags = new();
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
            var a when a[0] == '!' 
                => ProcessComment(currentHtml),
            var a when a[^1] == '/' 
                => ProcessSelfClosingTag(currentHtml),
            var a when a[0] == '/' 
                => ProcessClosingTag(currentHtml, charsProcessed, openedTags.Peek()),
            _ => ProcessOpeningTag(currentHtml, charsProcessed)
        };
    }

    static int ProcessComment(ReadOnlySpan<char> currentHtml)
        => CommentsSkipper.TrySkipComment(currentHtml, out var processed)
            ? processed
            : 0;

    static int ProcessSelfClosingTag(ReadOnlySpan<char> currentHtml)
        => 1 + TagsNavigator.GetNextTagIndex(currentHtml[1..]);

    int ProcessClosingTag(
        ReadOnlySpan<char> currentHtml, 
        int charsProcessed, 
        UnprocessedTag lastOpenedTag)
    {
        var processedTagCreator = new ProcessedTagCreator(charsProcessed, lastOpenedTag);
        var result = processedTagCreator.Create(currentHtml);
        ProcessResult(result);
        return 1 + TagsNavigator.GetNextTagIndex(currentHtml[1..]);
    }

    int ProcessOpeningTag(ReadOnlySpan<char> currentHtml, int charsProcessed)
    {
        var unprocessedTagCreator = new UnprocessedTagCreator(charsProcessed);
        var result = unprocessedTagCreator.Create(currentHtml);
        ProcessResult(result);
        return 1 + TagsNavigator.GetNextTagIndex(currentHtml[1..]);
    }

    void ProcessResult(ProcessedTag result)
    {
        Process([.. openedTags.Reverse()], result);
        openedTags.Pop();
        processedTags.Enqueue(result);
    }

    void ProcessResult(UnprocessedTag result)
    {
        Process([.. openedTags.Reverse()], result);
        openedTags.Push(result);
    }
}