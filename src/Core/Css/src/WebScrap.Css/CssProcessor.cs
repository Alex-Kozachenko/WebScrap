using WebScrap.Css.Data;
using WebScrap.Css.Contracts;
using WebScrap.Core.Tags.Data;
using System.Collections.Immutable;

namespace WebScrap.Css;

/// <summary>
/// Represents a tags processor, 
/// which filters out the tags,
/// which comply the provided css.
/// </summary>
public sealed class CssProcessor(
    ICssComparer comparer,
    CssToken[] expectedTags)
    : IObserver<TagsProviderMessage>
{
    private readonly CssToken[] expectedTags = expectedTags;
    private readonly List<Range> cssCompliantTagRanges = [];
    private IDisposable? unsubscriber;

    public ImmutableArray<Range> CssCompliantTagRanges => [..cssCompliantTagRanges];

    public CssProcessor Subscribe(IObservable<TagsProviderMessage> observer)
    {
        unsubscriber = observer.Subscribe(this);
        return this;
    }

    public void OnCompleted() 
    {
         unsubscriber?.Dispose(); 
    }

    public void OnError(Exception error) { }

    public void OnNext(TagsProviderMessage message)
    {
        var tagInfos = message.TagsHistory;

        var namesMet = comparer.CompareNames(
                expectedTags, 
                [..tagInfos]);

        var attributesMet = comparer.CompareAttributes(
                expectedTags, 
                [..tagInfos]);

        if (namesMet && attributesMet)
        {
            cssCompliantTagRanges.Add(message.CurrentTag.TagRange);
        }
    }
}