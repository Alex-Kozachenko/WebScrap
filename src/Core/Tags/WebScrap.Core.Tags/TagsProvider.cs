using WebScrap.Core.Tags.Data;
using WebScrap.Core.Tags.Messaging;

namespace WebScrap.Core.Tags;

public class TagsProvider : ITagObservable
{
    readonly HistoryTracker historyTracker = new();
    readonly Broadcaster broadcaster = new();

    public void Process(ReadOnlySpan<char> html)
    {
        var charsProcessed = TagsNavigator.GetNextTagIndex(html);
        do
        {
            var currentHtml = html[charsProcessed..];
            Process(currentHtml, charsProcessed);
            historyTracker.Update(currentHtml, charsProcessed);
            charsProcessed += Proceed(currentHtml);
        } while (charsProcessed < html.Length);

        broadcaster.OnCompleted();
    }    

    public IDisposable Subscribe(IObserver<TagsProviderMessage> observer)
        => broadcaster.Subscribe(observer);

    public IDisposable Subscribe(IObserver<TagsProviderMessage> observer, params string[] tagNames)
        => broadcaster.Subscribe(observer, tagNames);

    void Process(ReadOnlySpan<char> currentHtml, int charsProcessed)
    {
        if (TagDetector.Detect(currentHtml) != TagKind.Closing)
        {
            return;
        }
        var latestTag = historyTracker.History.LastOrDefault();

        if (latestTag == null)
        {
            // TODO: log a warning here!
            return;
        }

        var tag = currentHtml.Clip("<", ">");
        var tagLength = charsProcessed + tag.Length;
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

        broadcaster.OnNext(message, value.TagInfo.Name);
    }

    static int Proceed(ReadOnlySpan<char> currentHtml)
        => TagDetector.Detect(currentHtml) switch
        {
            TagKind.Comment => TagsNavigator.SkipComment(currentHtml),
            _ => 1 + TagsNavigator.GetNextTagIndex(currentHtml[1..])
        };
}
