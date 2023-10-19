using WebScrap.Css.Data;
using WebScrap.Css.Data.Selectors;
using WebScrap.Css.Data.Tags;

namespace WebScrap.Css.Matching.Tests;

[TestFixture]
public class CssComparer_Tests
{
    [TestCase("main div p b", ExpectedResult = true)]
    [TestCase("main div b", ExpectedResult = true)]
    [TestCase("main div p div b", ExpectedResult = true)]
    [TestCase("main b div b", ExpectedResult = true)]
    public bool CompareNames_With_AnyChildCssToken_Finds(string input)
    {
        // main div b
        var cssTags = new List<CssToken>
        {
            new(new RootCssSelector(), new CssTag("main")),
            new(new ChildCssSelector(), new CssTag("div")),
            new(new ChildCssSelector(), new CssTag("b"))
        };

        var tagsMet = input.Split(' ')
            .Select(x => new TagInfo(x, EmptyAttributes))
            .ToArray();

        return CompareNames(cssTags, tagsMet);
    }

    [TestCase("main b div p", ExpectedResult = false)]
    [TestCase("foo bar buzz b", ExpectedResult = false)]
    [TestCase("foo bar div b", ExpectedResult = false)]
    public bool CompareNames_With_AnyChildCssToken_NotFinds(string input)
    {
        // main div b
        var cssTags = new List<CssToken>
        {
            new(new RootCssSelector(), new CssTag("main")),
            new(new ChildCssSelector(), new CssTag("div")),
            new(new ChildCssSelector(), new CssTag("b"))
        };

        var tagsMet = input.Split(' ')
            .Select(x => new TagInfo(x, EmptyAttributes))
            .ToArray();

        return CompareNames(cssTags, tagsMet);
    }

    [TestCase("main div p b", ExpectedResult = false)]
    [TestCase("main div b", ExpectedResult = true)]
    public bool CompareNames_With_DirectChildCssToken_Finds(string input)
    {
        // div>b
        var cssTags = new List<CssToken>
        {
            new(new RootCssSelector(), new CssTag("div")),
            new(new DirectChildCssSelector(), new CssTag("b"))
    };

        var tagsMet = input.Split(' ')
            .Select(x => new TagInfo(x, EmptyAttributes))
            .ToArray();

        return CompareNames(cssTags, tagsMet);
    }

    [Test]
    public void CompareNames_With_Second_RootChildCssToken_Fails()
    {
        var tagsMet = new string[] { "doesntmatter"  }
            .Select(x => new TagInfo(x, EmptyAttributes))
            .ToArray();

        var cssTags = new List<CssToken>
        {
            new(new RootCssSelector(), new CssTag("div")),
            new(new RootCssSelector(), new CssTag("div"))
        };

        Assert.That(() => CompareNames(cssTags, tagsMet), Throws.Exception);
    }

    [Test]
    public void CompareNames_Without_RootChildCssToken_Fails()
    {
        var tagsMet = new string[] { "doesntmatter" }
            .Select(x => new TagInfo(x, EmptyAttributes))
            .ToArray();

        var cssTags = new List<CssToken>
        {
            new(new ChildCssSelector(), new CssTag("div")),
            new(new ChildCssSelector(), new CssTag("div")),
        };

        Assert.That(() => CompareNames(cssTags, tagsMet), Throws.Exception);
    }

    private static bool CompareNames(
        IReadOnlyCollection<CssToken> cssTags,
        IReadOnlyCollection<TagInfo> tagsMet)
        => new CssComparer().CompareNames(
            [.. cssTags], 
            [.. tagsMet]);

    internal static ILookup<string, string> EmptyAttributes
        => Array.Empty<byte>()
            .ToLookup(x => string.Empty, x => string.Empty);
}