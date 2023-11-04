using static WebScrap.Css.Tests.Helpers.CssProcessorHelper;

namespace WebScrap.Css.Tests;

[TestFixture]
public class CssProcessor_Tags_Tests
{

    [TestCase(
        "p", 
        "<div><p>__</p>__<p>__</p></div>", 
        "     ^^^^^^^^^  ^^^^^^^^^      ")]
    [TestCase(
        "div>p", 
        "<p>__</p>__", 
        "           ")]
    [TestCase(
        "div>p", 
        "<div> <p>_</p> </div>",
        "      ^^^^^^^^       ")]
    [TestCase(
        "div>p", 
        "<div> <p>_<p>_</p>__</p> </div>",
        "      ^^^^^^^^^^^^^^^^^^      ")]
    [TestCase(
        "div>p", 
        "<div>_<a></a>_<p>_</p> </div>",
        "              ^^^^^^^^       ")]
    [TestCase(
        "div>p", 
        "<div>_<div></div>_<p>_</p>_</div>",
        "                  ^^^^^^^^       ")]
    [TestCase(
        "div>p", 
        "<div><div> <p>_</p> </div></div>",
        "           ^^^^^^^^             ")]
    public void CalculateTagIndexes_WithTags_ShouldReturn_Ranges(
        string css, 
        string html, 
        string pointers)
    {
        var expected = pointers.ToRanges().ToSubstrings(html);
        var results = CalculateTagRanges(html, css).ToSubstrings(html);
        Assert.That(results, Is.EquivalentTo(expected));
    }

    
    [TestCase(
        "div>p",
        "<main><div><aside><p>_</p> </aside></div></main>")]
    [TestCase(
        "div>p",
        "<main><div></div>_<p>_</p></main>")]
    public void CalculateTagIndexes_WithUnlocatableCss_Returns_EmptyResults(
        string css,
        string html)
    {
        var results = CalculateTagRanges(html, css).ToSubstrings(html);
        Assert.Multiple(() =>
        {
            foreach (var result in results)
            {
                Assert.That(result, Is.Empty);
            }
        });
    }
}