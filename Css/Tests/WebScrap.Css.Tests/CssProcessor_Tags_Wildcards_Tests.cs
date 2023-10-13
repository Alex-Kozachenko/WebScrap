using static WebScrap.Css.Tests.Helpers.CssProcessorHelper;

namespace WebScrap.Css.Tests;

[TestFixture]
public class CssProcessor_Tags_Wildcards_Tests
{
    [TestCase(
        "*",
        "<p></p>", 
        "^^^^^^^")]
    [TestCase(
        "div>*",
        "<main><div> <p></p> <div></div> </div></main>",
        "            ^^^^^^^ ^^^^^^^^^^^              ")]
    [TestCase(
        "main *",
        "<main> <p></p> <div></div> </main>", 
        "       ^^^^^^^ ^^^^^^^^^^^        ")]
    [TestCase(
        "main *",
        "<main> <aside><p></p></aside> </main>", 
        "       ^^^^^^^^^^^^^^^^^^^^^^        ",
        "              ^^^^^^^                ")]
    [TestCase(
        "*>span",
        "<main> <span></span> </main>", 
        "       ^^^^^^^^^^^^^        ")]
    [TestCase(
        "* span",
        "<main><p> <span></span> </p> <span></span> </main>", 
        "          ^^^^^^^^^^^^^      ^^^^^^^^^^^^^        ")]
    public void CalculateTagIndexes_WithWildcards_ShouldReturn_Ranges(
        string css,
        string html, 
        params string[] pointersArray)
    {
        var expected = new List<string>();
        foreach (var pointers in pointersArray)
        {
            expected.AddRange(pointers.ToRanges().ToSubstrings(html));
        }

        var results = CalculateTagRanges(html, css).ToSubstrings(html);
        Assert.That(results, Is.EquivalentTo(expected));
    }
}