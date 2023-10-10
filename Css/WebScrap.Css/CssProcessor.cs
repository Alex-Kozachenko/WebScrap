using WebScrap.Common.Css;
using WebScrap.Common.Tags;
using WebScrap.Css.Matching;
using WebScrap.Core.Tags;

namespace WebScrap.Css;

public class CssProcessor(
    CssToken[] expectedTags) 
    : TagsProcessor
{
    private readonly CssToken[] expectedTags = expectedTags;
    public List<int> TagIndexes = [];

    protected override void Process(OpeningTag tag)
    {
        var isEntireCssMet = MatchingAPI.IsMatch.Names(
                expectedTags, 
                TagsHistory) 
            && MatchingAPI.IsMatch.Attributes(
                expectedTags, 
                TagsHistory);

        if (isEntireCssMet)
        {
            TagIndexes.Add(CharsProcessed);
        }
    }

    protected override void Process(ClosingTag tag)
    {
    }
}