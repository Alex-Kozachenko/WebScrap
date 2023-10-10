using static WebScrap.Css.Tests.CssProcessor;

namespace WebScrap.Css.Tests;

[TestFixture]
public class CssProcessor_Tags_Wildcards_Tests
{
    [TestCase(
        "<p>", "*",
        "^")]
    [TestCase(
        "<main><div><p></p><div></div></div></main>", "div>*",
        "           ^      ^")]
    [TestCase(
        "<div><p></p><div></div>", "div *",
        "     ^      ^")]
    [TestCase(
        "<main><span></span></main>", "*>span",
        "      ^")]
    [TestCase(
        "<main><p> <span></span> </p> </main>", "* span",
        "          ^")]
    public void CalculateTagIndexes_WithWildcards_ShouldReturn_Indexes(
        string html, string css,
        string pointers)
    {
        var expected = pointers.ToIndexes().ToSubstrings(html);
        var results = CalculateTagIndexes(html, css).Select(x => html[x..]);
        Assert.That(results, Is.EquivalentTo(expected));
    }
}