using WebScrap.Common;
using WebScrap.Common.Contracts;
using WebScrap.Common.Css;
using WebScrap.Common.Tags;
using WebScrap.Css.Matching;

namespace WebScrap.Css;

public class CssProcessor(
    TagFactoryBase tagFactory, 
    CssToken[] expectedTags) 
    : ProcessorBase(tagFactory)
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