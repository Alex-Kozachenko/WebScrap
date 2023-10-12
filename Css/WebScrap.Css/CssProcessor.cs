using WebScrap.Css.Data;
using WebScrap.Css.Contracts;
using WebScrap.Core.Tags;
using System.Collections.Immutable;

namespace WebScrap.Css;

public sealed class CssProcessor(
    ICssComparer comparer,
    ITokensBuilder tokensBuilder,
    ReadOnlySpan<char> css) 
    : TagsProcessorBase
{
    private readonly CssToken[] expectedTags = tokensBuilder.Build(css);
    private readonly List<int> tagIndexes = [];

    public ImmutableArray<int> ProcessCss(ReadOnlySpan<char> html)
    {
        Process(html);
        return [..tagIndexes];
    }

    protected override void Process(
        UnprocessedTag[] openedTags, 
        UnprocessedTag unprocessedTag)
    {
        var tagInfos = openedTags
            .Select(x => x.TagInfo)
            .ToList();

        tagInfos.Add(unprocessedTag.TagInfo);

        var namesMet = comparer.CompareNames(
                expectedTags, 
                [..tagInfos]);

        var attributesMet = comparer.CompareAttributes(
                expectedTags, 
                [..tagInfos]);

        if (namesMet && attributesMet)
        {
            tagIndexes.Add(unprocessedTag.TagOffset);
        }
    }
}