using WebScrap.Common.Css;
using WebScrap.Common.Css.Selectors;
using WebScrap.Common.Css.Tags;
using WebScrap.Css;

namespace WebScrap.Css.Matching.Tests;

[TestFixture]
public class MatchingAPI_Names_Tests
{
    [TestCase("main div p b", ExpectedResult = true)]
    [TestCase("main div b", ExpectedResult = true)]
    [TestCase("main div p div b", ExpectedResult = true)]
    [TestCase("main b div b", ExpectedResult = true)]
    public bool IsMatch_Names_With_AnyChildCssToken_Finds(string input)
    {
        // main div b
        var cssTags = new List<CssToken>
        {
            new(new RootCssSelector(), new CssTag("main")),
            new(new AnyChildCssSelector(), new CssTag("div")),
            new(new AnyChildCssSelector(), new CssTag("b"))
        };

        var tagsMet = input.Split(' ')
            .Select(x => new OpeningTag(x, EmptyAttributes))
            .ToArray();

        return IsMatch_Names(cssTags, tagsMet);
    }

    [TestCase("main b div p", ExpectedResult = false)]
    [TestCase("foo bar buzz b", ExpectedResult = false)]
    [TestCase("foo bar div b", ExpectedResult = false)]
    public bool IsMatch_Names_With_AnyChildCssToken_NotFinds(string input)
    {
        // main div b
        var cssTags = new List<CssToken>
        {
            new(new RootCssSelector(), new CssTag("main")),
            new(new AnyChildCssSelector(), new CssTag("div")),
            new(new AnyChildCssSelector(), new CssTag("b"))
        };

        var tagsMet = input.Split(' ')
            .Select(x => new OpeningTag(x, EmptyAttributes))
            .ToArray();

        return IsMatch_Names(cssTags, tagsMet);
    }

    [TestCase("main div p b", ExpectedResult = false)]
    [TestCase("main div b", ExpectedResult = true)]
    public bool IsMatch_Names_With_DirectChildCssToken_Finds(string input)
    {
        // div>b
        var cssTags = new List<CssToken>
        {
            new(new RootCssSelector(), new CssTag("div")),
            new(new ChildCssSelector(), new CssTag("b"))
        };

        var tagsMet = input.Split(' ')
            .Select(x => new OpeningTag(x, EmptyAttributes))
            .ToArray();

        return IsMatch_Names(cssTags, tagsMet);
    }

    [Test]
    public void IsMatch_Names_With_Second_RootChildCssToken_Fails()
    {
        var tagsMet = new string[] { "doesntmatter"  }
            .Select(x => new OpeningTag(x, EmptyAttributes))
            .ToArray();

        var cssTags = new List<CssToken>
        {
            new(new RootCssSelector(), new CssTag("div")),
            new(new RootCssSelector(), new CssTag("div"))
        };

        Assert.That(() => IsMatch_Names(cssTags, tagsMet), Throws.Exception);
    }

    [Test]
    public void IsMatch_Names_Without_RootChildCssToken_Fails()
    {
        var tagsMet = new string[] { "doesntmatter" }
            .Select(x => new OpeningTag(x, EmptyAttributes))
            .ToArray();

        var cssTags = new List<CssToken>
        {
            new(new AnyChildCssSelector(), new CssTag("div")),
            new(new AnyChildCssSelector(), new CssTag("div")),
        };

        Assert.That(() => IsMatch_Names(cssTags, tagsMet), Throws.Exception);
    }

    private static bool IsMatch_Names(
        IReadOnlyCollection<CssToken> cssTags,
        IReadOnlyCollection<OpeningTag> tagsMet) 
        => MatchingAPI.IsMatch.Names(cssTags.ToArray(), tagsMet.ToArray());

    internal static ILookup<string, string> EmptyAttributes
        => Array.Empty<byte>()
            .ToLookup(x => string.Empty, x => string.Empty);
}