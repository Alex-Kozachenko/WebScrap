using WebScrap.Common.Tags;
using WebScrap.Css.Common.Tokens;

namespace WebScrap.Css.Tests;

[TestFixture]
public class CssCompliantCheckerTests
{
    [TestCase("main div p b", ExpectedResult = true)]
    [TestCase("main div b", ExpectedResult = true)]
    [TestCase("main div p div b", ExpectedResult = true)]
    [TestCase("main b div b", ExpectedResult = true)]
    [TestCase("main b div p", ExpectedResult = false)]
    public bool CheckNames_With_AnyChildCssToken_Works(string input)
    {
        // div b
        var cssTags = new List<CssTokenBase>
        {
            new RootCssToken("div", EmptyAttributes),
            new AnyChildCssToken("b", EmptyAttributes),
        };

        var tagsMet = input.Split(' ')
            .Select(x => new OpeningTag(x, EmptyAttributes))
            .ToArray();

        return CheckNames(cssTags, tagsMet);
    }

    [TestCase("main div p b", ExpectedResult = false)]
    [TestCase("main div b", ExpectedResult = true)]
    public bool CheckNames_With_DirectChildCssToken_Works(string input)
    {
        // div>b
        var cssTags = new List<CssTokenBase>
        {
            new RootCssToken("div", EmptyAttributes),
            new DirectChildCssToken("b", EmptyAttributes),
        };

        var tagsMet = input.Split(' ')
            .Select(x => new OpeningTag(x, EmptyAttributes))
            .ToArray();

        return CheckNames(cssTags, tagsMet);
    }

    [Test]
    public void CheckNames_With_Second_RootChildCssToken_Fails()
    {
        var tagsMet = new string[] { "doesntmatter"  }
            .Select(x => new OpeningTag(x, EmptyAttributes))
            .ToArray();

        var cssTags = new List<CssTokenBase>
        {
            new RootCssToken("div", EmptyAttributes),
            new RootCssToken("div", EmptyAttributes),
        };

        Assert.That(() => CheckNames(cssTags, tagsMet), Throws.Exception);
    }

    [Test]
    public void CheckNames_Without_RootChildCssToken_Fails()
    {
        var tagsMet = new string[] { "doesntmatter" }
            .Select(x => new OpeningTag(x, EmptyAttributes))
            .ToArray();

        var cssTags = new List<CssTokenBase>
        {
            new AnyChildCssToken("div", EmptyAttributes),
            new AnyChildCssToken("div", EmptyAttributes),
        };

        Assert.That(() => CheckNames(cssTags, tagsMet), Throws.Exception);
    }

    private bool CheckNames(
        IEnumerable<CssTokenBase> cssTags,
        IEnumerable<OpeningTag> tagsMet)
    {
        // TODO: this API is frigging drunk.
        var checker = new CssComplianceChecker(
            new Stack<CssTokenBase>(cssTags),
            new Stack<OpeningTag>(tagsMet));
        return CssComplianceChecker.CheckNames(checker);
    }

    internal static ILookup<string, string> EmptyAttributes
        => Array.Empty<byte>()
            .ToLookup(x => string.Empty, x => string.Empty);
}