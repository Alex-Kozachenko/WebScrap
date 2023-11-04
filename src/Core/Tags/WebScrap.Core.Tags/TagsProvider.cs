using WebScrap.Core.Tags.Creating;
using WebScrap.Core.Tags.Data;

namespace WebScrap.Core.Tags;

public class TagsProvider : 
    IObservable<TagsProviderMessage>
{
    private readonly Stack<UnprocessedTag> openedTags = new();
    private readonly HashSet<IObserver<TagsProviderMessage>> observers = [];

    public void Process(ReadOnlySpan<char> html)
    {
        var charsProcessed = 0;
        do
        {
            var currentHtml = html[charsProcessed..];
            Process(currentHtml, charsProcessed);
            ProcessHistory(currentHtml, charsProcessed); // HACK: order matters!
            charsProcessed += Proceed(currentHtml);
        } while (charsProcessed < html.Length);

        ForEach(observers, o => o.OnCompleted());
    }    

    public IDisposable Subscribe(IObserver<TagsProviderMessage> observer)
    {
        observers.Add(observer);
        return new Unsubscriber<TagsProviderMessage>(observers, observer);
    }

    void Process(ReadOnlySpan<char> currentHtml, int charsProcessed)
    {
        if (TagDetector.Detect(currentHtml) == TagKind.Closing)
        {
            var processedTag = new ClosingTagCreator(
                currentHtml, 
                charsProcessed, 
                openedTags.Peek())
                .Create();
            OnProcessedTagMet(processedTag);
        }
    }

    void ProcessHistory(ReadOnlySpan<char> currentHtml, int charsProcessed)
    {
        switch (TagDetector.Detect(currentHtml))
        {
            case TagKind.Opening:
            {
                var openedTag = new OpeningTagCreator(currentHtml, charsProcessed)
                    .Create();
                openedTags.Push(openedTag);
                break;
            }
            case TagKind.Closing:
            {
                openedTags.Pop();
                break;
            }
        }
    }

    void OnProcessedTagMet(ProcessedTag value)
    {
        var history = openedTags
            .Reverse()
            .Select(x => x.TagInfo);
        var message = new TagsProviderMessage(
            [..history], 
            value);
        ForEach(observers, o => o.OnNext(message));
    }

    static int Proceed(ReadOnlySpan<char> currentHtml)
        => TagDetector.Detect(currentHtml) switch
        {
            TagKind.Comment => TagsNavigator.SkipComment(currentHtml),
            _ => 1 + TagsNavigator.GetNextTagIndex(currentHtml[1..])
        };

    static void ForEach<T>(IEnumerable<T> set, Action<T> action)
    {
        foreach (var item in set)
        {
            action(item);
        }
    }
}
