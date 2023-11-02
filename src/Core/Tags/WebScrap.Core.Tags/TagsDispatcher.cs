using WebScrap.Core.Tags.Creating;
using WebScrap.Core.Tags.Data;
using WebScrap.Core.Tags.Tools;

namespace WebScrap.Core.Tags;

internal class TagsDispatcher : 
    IObservable<UnprocessedTag>, 
    IObservable<ProcessedTag>
{
    private readonly HashSet<IObserver<ProcessedTag>> processedTagObservers = [];

    private readonly HashSet<IObserver<UnprocessedTag>> unprocessedTagObservers = [];

    private readonly Stack<UnprocessedTag> openedTags = new();

    public Stack<UnprocessedTag> OpenedTags => openedTags;

    public IDisposable Subscribe(IObserver<UnprocessedTag> observer)
    {
        unprocessedTagObservers.Add(observer);
        return new Unsubscriber<UnprocessedTag>(unprocessedTagObservers, observer);
    }

    public IDisposable Subscribe(IObserver<ProcessedTag> observer)
    {
        processedTagObservers.Add(observer);
        return new Unsubscriber<ProcessedTag>(processedTagObservers, observer);
    }

    internal int Process(ReadOnlySpan<char> currentHtml, int charsProcessed)
    {
        if (!currentHtml.StartsWith("<"))
        {
            throw new ArgumentException($"Html should start with tag. {currentHtml}");
        }

        UnprocessedTag? unprocessedTag = null;
        ProcessedTag? processedTag = null;

        var result = currentHtml.Clip("<", ">", true) switch
        {
            var a when a[0] == '!' 
                => new CommentTagCreator(currentHtml).Proceed(),
            var a when a[^1] == '/' 
                => new InlineTagCreator(currentHtml).Proceed(),
            var a when a[0] == '/' 
                => new ClosingTagCreator(currentHtml, charsProcessed, openedTags.Peek())
                    .Create(out processedTag)
                    .Proceed(),
            _ => new OpeningTagCreator(currentHtml, charsProcessed)
                    .Create(out unprocessedTag)
                    .Proceed()
        };

        if (unprocessedTag != null)
        {
            openedTags.Push(unprocessedTag);
            ForEach(unprocessedTagObservers, x => x.OnNext(unprocessedTag));
        }

        if (processedTag != null)
        {
            ForEach(processedTagObservers, x => x.OnNext(processedTag));
            openedTags.Pop();
        }

        return result;
    }

    static void ForEach<T>(IEnumerable<T> set, Action<T> action)
    {
        foreach (var item in set)
        {
            action(item);
        }
    }
}