using static WebScrap.Css.Tests.Helpers.CssProcessorHelper;

namespace WebScrap.Css.Tests;

[TestFixture]
public class CssProcessor_Attributes_Tests
{

    [TestCase(
        "p#foo",
        "<div><p> </p> <p id='foo'> </p></div>", 
        "              ^^^^^^^^^^^^^^^^^      ")]
    [TestCase(
        "p#foo",
        "<div><p> </p> <br /> <p id='foo'> </p></div>", 
        "                     ^^^^^^^^^^^^^^^^^      ")]
    [TestCase(
        "p#foo",
        "<div><p> </p> <div> <p id='foo'> </p> </div></div>", 
        "                    ^^^^^^^^^^^^^^^^^             ")]
    [TestCase(
        "p#foo",
        "<div><p> </p> <div> <br /> <p id='foo'> </p> </div></div>", 
        "                           ^^^^^^^^^^^^^^^^^             ")]
    [TestCase(
        "p#foo",
        "<div><p> </p> <div> <div> </div> <p id='foo'> </p> </div></div>", 
        "                                 ^^^^^^^^^^^^^^^^^             ")]
    [TestCase(
        "p#foo",
        "<div> <div> <p> </p>  </div> <p> </p> <p id='foo'> </p> </div>", 
        "                                      ^^^^^^^^^^^^^^^^^       ")]
    [TestCase(
        "p.bar",
        "<div><p> </p> <p class='bar'> </p></div>", 
        "              ^^^^^^^^^^^^^^^^^^^^      ")]
    [TestCase(
        "p.bar",
        "<div><p> </p> <p class='bar buzz'> </p></div>", 
        "              ^^^^^^^^^^^^^^^^^^^^^^^^^      ")]
    [TestCase(
        "p.bar",
        "<div> <p class='bar'> </p> <p class='bar buzz'> </p> </div>", 
        "      ^^^^^^^^^^^^^^^^^^^^ ^^^^^^^^^^^^^^^^^^^^^^^^^       ")]
    [TestCase(
        "p.bar.buzz",
        "<div> <p class='bar'> </p> <p class='bar buzz'> </p> </div>", 
        "                           ^^^^^^^^^^^^^^^^^^^^^^^^^       ")]
    [TestCase(
        "p#foo.bar.buzz",
        "<div><p class='bar'> </p> <p id='foo' class='bar buzz'></p> </div>", 
        "                          ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^       ")]
    [TestCase(
        "p#foo.bar.buzz",
        "<div><p class='bar'> </p> <p id='foo' class='bar buzz fizz'> </p></div>", 
        "                          ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^      ")]
    [TestCase(
        "p#foo.bar.buzz",
        "<div> <p id='foo' class='bar buzz'> </p> <div id='foo' class='bar buzz'></div></div>", 
        "      ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ ")]
    public void CalculateTagIndexes_WithAttributes_ShouldReturn_Ranges(
        string css, 
        string html, 
        string pointers)
    {
        var expected = pointers.ToRanges().ToSubstrings(html);
        var results = CalculateTagRanges(html, css).ToSubstrings(html);
        Assert.That(results, Is.EquivalentTo(expected));
    }
}