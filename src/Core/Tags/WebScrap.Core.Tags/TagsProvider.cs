using System.Collections.Immutable;
using WebScrap.Core.Tags.Creating;
using WebScrap.Core.Tags.Data;

namespace WebScrap.Core.Tags;

public class TagsProvider : IObservable<ProcessedTag>, IObservable<UnprocessedTag>
{
    private readonly Stack<UnprocessedTag> openedTags = new();
    private readonly Queue<ProcessedTag> processedTags = new();
    private readonly HashSet<IObserver<ProcessedTag>> processedTagObservers = [];
    private readonly HashSet<IObserver<UnprocessedTag>> unprocessedTagObservers = [];

    public UnprocessedTag[] OpenedTags => openedTags.Reverse().ToArray(); // TODO: fix this exposure.
    public Queue<ProcessedTag> ProcessedTags => processedTags; // TODO: fix this exposure.

    public ImmutableArray<ProcessedTag> Process(ReadOnlySpan<char> html)
    {
        var charsProcessed = 0;
        do
        {
            charsProcessed += Process(html[charsProcessed..], charsProcessed);
        } while (charsProcessed < html.Length && openedTags.Count != 0);

        ForEach(processedTagObservers, o => o.OnCompleted());
        ForEach(unprocessedTagObservers, o => o.OnCompleted());

        return [.. processedTags.Reverse()];
    }

    public IDisposable Subscribe(IObserver<ProcessedTag> observer)
    {
        if (processedTagObservers.Add(observer))
        {
            ForEach(processedTags, o => observer.OnNext(o));
        }
        return new Unsubscriber<ProcessedTag>(processedTagObservers, observer);
    }

    public IDisposable Subscribe(IObserver<UnprocessedTag> observer)
    {
        if (unprocessedTagObservers.Add(observer))
        {
            ForEach(openedTags, o => observer.OnNext(o));
        }

        return new Unsubscriber<UnprocessedTag>(unprocessedTagObservers, observer);
    }

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
        ForEach(processedTagObservers, o => o.OnNext(result));
        openedTags.Pop();
        processedTags.Enqueue(result);
    }

    void ProcessResult(UnprocessedTag result)
    {
        ForEach(unprocessedTagObservers, o => o.OnNext(result));
        openedTags.Push(result);
    }

    static void ForEach<T>(IEnumerable<T> set, Action<T> action)
    {
        foreach (var item in set)
        {
            action(item);
        }
    }
}
