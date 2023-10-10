using static WebScrap.Css.Tests.Helpers.CssProcessorHelper;

namespace WebScrap.Css.Tests;

[TestFixture]
public class CssProcessor_Attributes_Tests
{

    [TestCase(
        "<div><p> </p> <p id='foo'> </p></div>", 
        "              ^")]
    [TestCase(
        "<div><p> </p> <br /> <p id='foo'> </p></div>", 
        "                     ^")]
    [TestCase(
        "<div><p> </p> <div> <p id='foo'> </p> </div></div>", 
        "                    ^")]
    [TestCase(
        "<div><p> </p> <div> <br /> <p id='foo'> </p> </div></div>", 
        "                           ^")]
    [TestCase(
        "<div><p> </p> <div> <div> </div> <p id='foo'> </p> </div></div>", 
        "                                 ^")]
    [TestCase(
        "<div> <div> <p> </p>  </div> <p> </p> <p id='foo'> </p> </div>", 
        "                                      ^")]
    public void CalculateTagIndexes_IdAttr_ShouldDetect(string html, string pointers)
    {
        var css = "p#foo";
        var expected = pointers.ToIndexes().ToSubstrings(html);
        var results = CalculateTagIndexes(html, css).ToSubstrings(html);
        Assert.That(results, Is.EquivalentTo(expected));
    }

    [TestCase(
        "<div><p> </p> <p class='bar'> </p></div>", 
        "              ^")]

    [TestCase(
        "<div><p> </p> <p class='bar buzz'> </p></div>", 
        "              ^")]

    [TestCase(
        "<div><p class='bar'> </p> <p class='bar buzz'> </p></div>", 
        "     ^                    ^")]
    public void CalculateTagIndexes_Class_ShouldDetect(string html, string pointers)
    {
        var css = "p.bar";
        var expected = pointers.ToIndexes().ToSubstrings(html);
        var results = CalculateTagIndexes(html, css).ToSubstrings(html);
        Assert.That(results, Is.EquivalentTo(expected));
    }

    [TestCase(
        "<div><p class='bar'> </p> <p class='bar buzz'> </p></div>", 
        "                          ^")]
    public void CalculateTagIndexes_MultiClass_ShouldDetect(string html, string pointers)
    {
        var css = "p.bar.buzz";
        var expected = pointers.ToIndexes().ToSubstrings(html);
        var results = CalculateTagIndexes(html, css).ToSubstrings(html);
        Assert.That(results, Is.EquivalentTo(expected));
    }

    [TestCase(
        "<div><p class='bar'> </p> <p id='foo' class='bar buzz'> </p></div>", 
        "                          ^")]
    [TestCase(
        "<div><p class='bar'> </p> <p id='foo' class='bar buzz fizz'> </p></div>", 
        "                          ^")]
    [TestCase(
        "<div><div id='foo' class='bar buzz'> </div> <p id='foo' class='bar buzz fizz'> </p></div>", 
        "                                            ^")]
    public void CalculateTagIndexes_MultiAttr_ShouldDetect(string html, string pointers)
    {
        var css = "p#foo.bar.buzz";
        var expected = pointers.ToIndexes().ToSubstrings(html);
        var results = CalculateTagIndexes(html, css).ToSubstrings(html);
        Assert.That(results, Is.EquivalentTo(expected));
    }
}