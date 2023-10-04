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

        var cssTags = new List<CssTokenBase>
        {
            new RootCssToken("div", EmptyAttributes),
            childSelector switch
            {
                ' ' => new AnyChildCssToken("b", EmptyAttributes),
                '>' => new DirectChildCssToken("b", EmptyAttributes),
                _ => throw new ArgumentException()
            }
        };

        // TODO: this API is frigging drunk.
        var checker = new CssComplianceChecker(
            new Stack<CssTokenBase>(cssTags),
            new Stack<OpeningTag>(tagsMet));
        var actual = CssComplianceChecker.CheckNames(checker);
        return actual;
    }

    internal static ILookup<string, string> EmptyAttributes 
        => Array.Empty<byte>()
            .ToLookup(x => string.Empty, x => string.Empty);
}