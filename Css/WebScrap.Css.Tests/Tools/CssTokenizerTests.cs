using static WebScrap.Css.Tools.CssTokenizer;

namespace WebScrap.Css.Tools.Tests;

[TestFixture]
public class CssTokenizerTests
{
    [Test]
    public void TokenizeCss_ShouldProcess_Descendants()
    {
        var sample = "main div>p";
        (char?, string)[] expected = 
        [
            new (null, "main"),
            new (' ', "div"),
            new ('>', "p"),
        ];

        (char?, string)[] result = TokenizeCss(sample)
            .Select(x => (x.ChildSelector, x.Tag.ToString()))
            .ToArray();
        Assert.That(result, Is.EquivalentTo(expected));
    }
}