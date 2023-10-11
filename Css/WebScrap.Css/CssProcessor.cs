using WebScrap.Css.Data;
using WebScrap.Common.Tags;
using WebScrap.Core.Tags;
using WebScrap.Css.Contracts;
using WebScrap.Core.Tags.Data;

namespace WebScrap.Css;

public class CssProcessor(
    ICssComparer comparer,
    ITokensBuilder tokensBuilder,
    ReadOnlySpan<char> css) 
    : TagsProcessorBase
{
    private readonly CssToken[] expectedTags = tokensBuilder.Build(css);
    public List<int> TagIndexes = [];

    protected override void Process(OpeningTag tag, TagsHistoryRecord tagsHistoryRecord)
    {
        var isEntireCssMet = comparer.CompareNames(
                expectedTags, 
                TagsHistory) 
            && comparer.CompareAttributes(
                expectedTags, 
                TagsHistory);

        if (isEntireCssMet)
        {
            TagIndexes.Add(tagsHistoryRecord.TagOffset);
        }
    }
}