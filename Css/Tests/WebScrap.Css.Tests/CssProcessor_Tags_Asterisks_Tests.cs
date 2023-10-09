using static WebScrap.Css.Tests.CssProcessor;

namespace WebScrap.Css.Tests;

[TestFixture]
public class CssProcessor_Tags_Asterisks_Tests
{
    [TestCase(
        "<p>",
        "^")]
    public void CalculateTagIndexes_WithAsterisks_ShouldReturn_Indexes(
        string html,
        string pointers)
    {
        var css = "*";
        var expected = pointers.ToSubstrings(html);
        var results = CalculateTagIndexes(html, css).Select(x => html[x..]);
        Assert.That(results, Is.EquivalentTo(expected));
    }
}