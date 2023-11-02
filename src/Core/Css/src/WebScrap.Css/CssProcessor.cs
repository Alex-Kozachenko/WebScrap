using WebScrap.Css.Data;
using WebScrap.Css.Contracts;
using WebScrap.Core.Tags;
using System.Collections.Immutable;
using WebScrap.Core.Tags.Providing;

namespace WebScrap.Css;

/// <summary>
/// Represents a tags processor, 
/// which filters out the tags,
/// which comply the provided css.
/// </summary>
public sealed class CssProcessor(
    ICssComparer comparer,
    ITokensBuilder tokensBuilder,
    ReadOnlySpan<char> css)
    : IObserver<TagsProviderMessage>
{
    private readonly CssToken[] expectedTags = tokensBuilder.Build(css);
    private readonly List<Range> cssCompliantTagRanges = [];
    private TagsProvider tagsProvider = new();

    /// <summary>
    /// Processes the html and returns tags which are compliant against the css selectors.
    /// </summary>
    public ImmutableArray<Range> ProcessCss(ReadOnlySpan<char> html)
    {
        tagsProvider = new();
        tagsProvider.Subscribe(this);
        tagsProvider.Process(html);
        return [..cssCompliantTagRanges];
    }

    public void OnCompleted() { }

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