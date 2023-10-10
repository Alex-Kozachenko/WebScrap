using WebScrap.Common.Tags;

using WebScrap.Common.Css;
using WebScrap.Css.Matching.Comparers;

namespace WebScrap.Css.Matching;

public static class MatchingAPI
{
    public class IsMatch
    {
        public static bool Names(
            CssToken[] expectedTags,
            OpeningTag[] traversedTags) 
            => new CssTraverser(
                new NameComparer(), 
                new CssTracker(expectedTags, traversedTags))
                    .Traverse();

        public static bool Attributes(
            CssToken[] expectedTags,
            OpeningTag[] traversedTags) 
            => new CssTraverser(
                new AttributesComparer(), 
                new CssTracker(expectedTags, traversedTags))
                    .Traverse();
    }

    
}