using System.Collections.Immutable;
using WebScrap.Core.Tags.Creating;
using WebScrap.Core.Tags.Data;
using WebScrap.Core.Tags.Providing;

namespace WebScrap.Core.Tags;

public class TagsProvider : IObservable<TagsProviderMessage>
{
    private readonly Stack<UnprocessedTag> openedTags = new();

    // TODO: Telling which tags has been met ever. Do I need this?
    private readonly Queue<ProcessedTag> processedTags = new();
    private readonly HashSet<IObserver<TagsProviderMessage>> observers = [];

    public ImmutableArray<ProcessedTag> Process(ReadOnlySpan<char> html)
    {
        var charsProcessed = 0;
        do
        {
            charsProcessed += Process(html[charsProcessed..], charsProcessed);
        } while (charsProcessed < html.Length && openedTags.Count != 0);

        ForEach(observers, o => o.OnCompleted());

        return [.. processedTags.Reverse()];
    }

    public IDisposable Subscribe(IObserver<TagsProviderMessage> observer)
    {
        observers.Add(observer);
        return new Unsubscriber<TagsProviderMessage>(observers, observer);
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

    int ProcessComment(ReadOnlySpan<char> currentHtml)
        => CommentsSkipper.TrySkipComment(currentHtml, out var processed)
            ? processed
            : 0;

    int ProcessSelfClosingTag(ReadOnlySpan<char> currentHtml)
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
        NotifyResult(result);

        openedTags.Pop();
        processedTags.Enqueue(result);
    }

    void NotifyResult(ProcessedTag result)
    {
        var history = openedTags.Reverse().Select(x => x.TagInfo);
        var message = new TagsProviderMessage(
            [..history], 
            result);
        ForEach(observers, o => o.OnNext(message));
    }

    void ProcessResult(UnprocessedTag result)
    {
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
