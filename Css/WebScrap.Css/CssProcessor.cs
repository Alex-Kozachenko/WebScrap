using WebScrap.Css.Data;
using WebScrap.Css.Contracts;
using WebScrap.Core.Tags;
using System.Collections.Immutable;

namespace WebScrap.Css;

/// <summary>
/// Represents a tags processor, 
/// which filters out the tags
/// which comply the provided css.
/// </summary>
public sealed class CssProcessor(
    ICssComparer comparer,
    ITokensBuilder tokensBuilder,
    ReadOnlySpan<char> css) 
    : TagsProcessorBase
{
    private readonly CssToken[] expectedTags = tokensBuilder.Build(css);
    private readonly List<Range> cssCompliantTagRanges = [];

    /// <summary>
    /// Runs the html processing.
    /// </summary>
    /// <param name="html"> The html for processing. </param>
    /// <returns> Ranges of css-compliant tags. </returns>
    public ImmutableArray<Range> ProcessCss(ReadOnlySpan<char> html)
    {
        Process(html);
        return [..cssCompliantTagRanges];
    }

    protected override void Process(
        UnprocessedTag[] openedTags, 
        ProcessedTag tag)
    {
        var tagInfos = openedTags
            .Select(x => x.TagInfo)
            .ToArray();

        var namesMet = comparer.CompareNames(
                expectedTags, 
                tagInfos);

        var attributesMet = comparer.CompareAttributes(
                expectedTags, 
                tagInfos);

        if (namesMet && attributesMet)
        {
            cssCompliantTagRanges.Add(tag.TagRange);
        }
    }
}