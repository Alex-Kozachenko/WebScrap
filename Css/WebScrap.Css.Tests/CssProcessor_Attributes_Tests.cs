using static WebScrap.Css.CssProcessor;

namespace WebScrap.Css.Tests;

[TestFixture]
public class CssProcessor_Attributes_Tests
{

    [TestCase(
        "<p> </p> <p id='foo'> </p>", 
        "         ^")]
    [TestCase(
        "<p> </p> <br /> <p id='foo'> </p>", 
        "                ^")]
    [TestCase(
        "<p> </p> <div> <p id='foo'> </p> </div>", 
        "               ^")]
    [TestCase(
        "<p> </p> <div> <br /> <p id='foo'> </p> </div>", 
        "                      ^")]
    [TestCase(
        "<p> </p> <div> <div> </div> <p id='foo'> </p> </div>", 
        "                            ^")]
    [TestCase(
        "<div> <div> <p> </p>  </div> <p> </p> <p id='foo'> </p> </div>", 
        "                                      ^")]
    public void CalculateTagIndexes_IdAttr_ShouldDetect(string html, string pointer)
    {
        var css = "p#foo";
        var expected = PointersToIndexes(pointer).Select(x => html[x..]);
        var results = CalculateTagIndexes(html, css).Select(x => html[x..]);
        Assert.That(results, Is.EquivalentTo(expected));
    }

    [TestCase(
        "<p> </p> <p class='bar'> </p>", 
        "         ^")]

    [TestCase(
        "<p> </p> <p class='bar buzz'> </p>", 
        "         ^")]

    [TestCase(
        "<p class='bar'> </p> <p class='bar buzz'> </p>", 
        "^                    ^")]
    public void CalculateTagIndexes_Class_ShouldDetect(string html, string pointer)
    {
        var css = "p.bar";
        var expected = PointersToIndexes(pointer).Select(x => html[x..]);
        var results = CalculateTagIndexes(html, css).Select(x => html[x..]);
        Assert.That(results, Is.EquivalentTo(expected));
    }

    [TestCase(
        "<p class='bar'> </p> <p class='bar buzz'> </p>", 
        "                     ^")]
    public void CalculateTagIndexes_MultiClass_ShouldDetect(string html, string pointer)
    {
        var css = "p.bar.buzz";
        var expected = PointersToIndexes(pointer).Select(x => html[x..]);
        var results = CalculateTagIndexes(html, css).Select(x => html[x..]);
        Assert.That(results, Is.EquivalentTo(expected));
    }

    private static int[] PointersToIndexes(ReadOnlySpan<char> pointers)
    {
        var result = new List<int>();
        for (var i = 0; i < pointers.Length; i++)
        {
            var ch = pointers[i];
            if (ch == '^')
            {
                result.Add(i);
            }
        }
        return [.. result];
    }
}