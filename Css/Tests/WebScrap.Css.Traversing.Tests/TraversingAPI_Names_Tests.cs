

namespace WebScrap.Css.Traversing.Tests;

[TestFixture]
public class TraversingAPI_Names_Tests
{
    [TestCase("main div p b", ExpectedResult = true)]
    [TestCase("main div b", ExpectedResult = true)]
    [TestCase("main div p div b", ExpectedResult = true)]
    [TestCase("main b div b", ExpectedResult = true)]
    public bool TraverseNames_With_AnyChildCssToken_Finds(string input)
    {
        // main div b
        var cssTags = new List<CssTokenBase>
        {
            new RootCssToken("main", EmptyAttributes),
            new AnyChildCssToken("div", EmptyAttributes),
            new AnyChildCssToken("b", EmptyAttributes),
        };

        var tagsMet = input.Split(' ')
            .Select(x => new OpeningTag(x, EmptyAttributes))
            .ToArray();

        return TraverseNames(cssTags, tagsMet);
    }

    [TestCase("main b div p", ExpectedResult = false)]
    [TestCase("foo bar buzz b", ExpectedResult = false)]
    [TestCase("foo bar div b", ExpectedResult = false)]
    public bool TraverseNames_With_AnyChildCssToken_NotFinds(string input)
    {
        // main div b
        var cssTags = new List<CssTokenBase>
        {
            new RootCssToken("main", EmptyAttributes),
            new AnyChildCssToken("div", EmptyAttributes),
            new AnyChildCssToken("b", EmptyAttributes),
        };

        var tagsMet = input.Split(' ')
            .Select(x => new OpeningTag(x, EmptyAttributes))
            .ToArray();

        return TraverseNames(cssTags, tagsMet);
    }

    [TestCase("main div p b", ExpectedResult = false)]
    [TestCase("main div b", ExpectedResult = true)]
    public bool TraverseNames_With_DirectChildCssToken_Finds(string input)
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

        return TraverseNames(cssTags, tagsMet);
    }

    [Test]
    public void TraverseNames_With_Second_RootChildCssToken_Fails()
    {
        var tagsMet = new string[] { "doesntmatter"  }
            .Select(x => new OpeningTag(x, EmptyAttributes))
            .ToArray();

        var cssTags = new List<CssTokenBase>
        {
            new RootCssToken("div", EmptyAttributes),
            new RootCssToken("div", EmptyAttributes),
        };

        Assert.That(() => TraverseNames(cssTags, tagsMet), Throws.Exception);
    }

    [Test]
    public void TraverseNames_Without_RootChildCssToken_Fails()
    {
        var tagsMet = new string[] { "doesntmatter" }
            .Select(x => new OpeningTag(x, EmptyAttributes))
            .ToArray();

        var cssTags = new List<CssTokenBase>
        {
            new AnyChildCssToken("div", EmptyAttributes),
            new AnyChildCssToken("div", EmptyAttributes),
        };

        Assert.That(() => TraverseNames(cssTags, tagsMet), Throws.Exception);
    }

    private static bool TraverseNames(
        IEnumerable<CssTokenBase> cssTags,
        IEnumerable<OpeningTag> tagsMet) 
        => TraversingAPI.TraverseNames(
            new Stack<CssTokenBase>(cssTags),
            new Stack<OpeningTag>(tagsMet));

    internal static ILookup<string, string> EmptyAttributes
        => Array.Empty<byte>()
            .ToLookup(x => string.Empty, x => string.Empty);
}