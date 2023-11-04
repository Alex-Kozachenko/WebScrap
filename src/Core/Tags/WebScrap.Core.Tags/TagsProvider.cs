using WebScrap.Core.Tags.Data;

namespace WebScrap.Core.Tags;

public class TagsProvider : 
    IObservable<TagsProviderMessage>
{
    readonly HistoryTracker historyTracker = new();
    readonly HashSet<IObserver<TagsProviderMessage>> observers = [];

    public void Process(ReadOnlySpan<char> html)
    {
        var charsProcessed = 0;
        do
        {
            var currentHtml = html[charsProcessed..];
            Process(currentHtml, charsProcessed);
            historyTracker.Update(currentHtml, charsProcessed);
            charsProcessed += Proceed(currentHtml);
        } while (charsProcessed < html.Length);

        ForEach(observers, o => o.OnCompleted());
        observers.Clear();
    }    

    public IDisposable Subscribe(IObserver<TagsProviderMessage> observer)
    {
        observers.Add(observer);
        return new Unsubscriber<TagsProviderMessage>(observers, observer);
    }

    void Process(ReadOnlySpan<char> currentHtml, int charsProcessed)
    {
        if (TagDetector.Detect(currentHtml) != TagKind.Closing)
        {
            return;
        }

        var tag = currentHtml.Clip("<", ">");
        var tagLength = charsProcessed + tag.Length;
        var latestTag = historyTracker.History.Last();
        var range = latestTag.TagOffset..tagLength;
        var innerRange = latestTag.InnerOffset switch 
            {
                null => 0..0,
                var o => o.Value..charsProcessed
            };
        var processedTag = new ProcessedTag(latestTag.TagInfo, range, innerRange);

        OnProcessedTagMet(processedTag);
    }

    void OnProcessedTagMet(ProcessedTag value)
    {
        var history = historyTracker
            .History
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
