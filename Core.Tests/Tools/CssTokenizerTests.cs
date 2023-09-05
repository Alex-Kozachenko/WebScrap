using Core.Tools;

namespace Core.Tools.Tests;

[TestFixture]
public class CssTokenizerTests
{
    [Test]
    public void TokenizeCss_ShouldWork()
    {
        // TODO: name this test. I'm just outa ideas rn.
        var sample = "main div#foo>p.bar";
        var expected = new CssToken[]
        {
            new CssToken("main"),
            new ChildCssToken("div#foo", CssDescendanceKind.Deep),
            new ChildCssToken("p.bar", CssDescendanceKind.Child)
        };

        var result = CssTokenizer.TokenizeCss(sample);
        Assert.That(result, Is.EquivalentTo(expected));
    }
}