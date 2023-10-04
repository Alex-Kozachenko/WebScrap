using static WebScrap.Css.Preprocessing.Tokens.CssTokenizer;

namespace WebScrap.Css.Preprocessing.Tokens.Tests;

[TestFixture]
public class CssTokenizerTests
{
    [Test]
    public void TokenizeCss_ShouldProcess_Descendants()
    {
        var sample = "main div>p";
        (char?, string)[] expected =
        [
            new(null, "main"),
            new(' ', "div"),
            new('>', "p"),
        ];

        (char?, string)[] result = TokenizeCss(sample)
            .Select(x => (x.ChildSelector, x.Name.ToString()))
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