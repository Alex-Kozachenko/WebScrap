using static WebScrap.Css.Preprocessing.CssTokenizer;
using WebScrap.Css.Preprocessing.Tokens;

namespace WebScrap.Css.Preprocessing.Tests;

[TestFixture]
public class CssTokenizerTests
{
    [Test]
    public void TokenizeCss_ShouldProcess_Descendants()
    {
        var sample = "main div>p";
        (Type, string)[] expected =
        [
            new(typeof(RootCssToken), "main"),
            new(typeof(AnyChildCssToken), "div"),
            new(typeof(DirectChildCssToken), "p"),
        ];

        (Type, string)[] result = TokenizeCss(sample)
            .Select(x => (x.GetType(), x.Name.ToString()))
            .ToArray();
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public void TokenizeCss_ShouldProcess_MultipleSelectors()
    {
        var sample = "p#foo.bar.buzz";
        var expected = new KeyValuePair<string, string>[]
        {
            new("id", "foo"),
            new("class", "bar"),
            new("class", "buzz"),
        }.ToLookup(x => x.Key, x => x.Value);

        var result = TokenizeCss(sample)
            .First()
            .Attributes;

        Assert.That(result, Is.EquivalentTo(expected));
    }
}