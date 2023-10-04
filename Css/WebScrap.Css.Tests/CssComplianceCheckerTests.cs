using WebScrap.Common.Tags;
using WebScrap.Css.Preprocessing.Tokens;

namespace WebScrap.Css.Tests;

[TestFixture]
public class CssCompliantCheckerTests
{
    [TestCase(' ', ExpectedResult = true)]
    [TestCase('>', ExpectedResult = false)]
    public bool CheckNames_WithChildSelector_Works(char childSelector)
    {
        var tagsMet = new string[] { "main", "div","p", "b" }
            .Select(x => new OpeningTag(x, EmptyAttributes))
            .ToArray();

        CssOpeningTag[] cssTags = [
            new("div", EmptyAttributes, null),
            new("b", EmptyAttributes, childSelector)
            ];

        var actual = new CssCompliantChecker(
            new Stack<OpeningTag>(tagsMet.Reverse()), 
            new Stack<CssOpeningTag>(cssTags.Reverse()))
            .CheckNames();

        return actual;
    }

    internal static ILookup<string, string> EmptyAttributes 
        => Array.Empty<byte>()
            .ToLookup(x => string.Empty, x => string.Empty);
}