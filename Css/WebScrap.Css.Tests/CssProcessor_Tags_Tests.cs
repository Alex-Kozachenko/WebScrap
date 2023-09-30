using static WebScrap.Css.CssProcessor;

namespace WebScrap.Css.Tests;

[TestFixture]
public class CssProcessor_Tags_Tests
{

    [TestCase]
    public void CalculateTagIndexes_WithMultipleTags_ShouldReturn_MultipleIndexes()
    {
        var css = "p";
        var (html, pointers) = (
            "<p>__</p>__<p>__",
            "^          ^");
        var expected = PointersToIndexes(pointers).Select(x => html[x..]);
        var results = CalculateTagIndexes(html, css).Select(x => html[x..]);
        Assert.That(results, Is.EquivalentTo(expected));
    }

    [TestCase(
        "<p>__</p>__<p>__",
        "")]
    [TestCase(
        "<div><p>_",
        "     ^")]
    [TestCase(
        "<div><p>_<p>_",
        "     ^")]
    [TestCase(
        "<div>_<a></a>_<p>_",
        "              ^")]
    [TestCase(
        "<div>_<div></div>_<p>_",
        "                  ^")]
    [TestCase(
        "<div><div><p>_",
        "          ^")]
    public void CalculateTagIndexes_WithComplexCss_ShouldReturn_MultipleIndexes(
        string html,
        string pointers)
    {
        var css = "div>p";
        var expected = PointersToIndexes(pointers).Select(x => html[x..]);
        var results = CalculateTagIndexes(html, css).Select(x => html[x..]);
        Assert.That(results, Is.EquivalentTo(expected));
    }

    [TestCase("<div><aside><p>_")]
    [TestCase("<div></div>_<p>_")]

    public void CalculateTagIndexes_DirectChildMissing_ShouldReturn_Empty(string html)
    {
        var css = "div>p";
        var results = CalculateTagIndexes(html, css).Select(x => html[x..]);
        Assert.Multiple(() =>
        {
            foreach (var result in results)
            {
                Assert.That(result, Is.Empty);
            }
        });
    }

    [TestCase]
    public void CalculateTagIndexes_WithSingleTag_ShouldReturn_SingleIndex()
    {
        var css = "p";
        var html = "<p>__</p>";
        var expected = 0;
        var result = CalculateTagIndexes(html, css).First();
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("_____<p></p>")]
    [TestCase("p>__<p>")]
    [TestCase("__</p>__")]
    public void CalculateTagIndexes_IncorrectHtml_ShouldFail(
        string sample)
    {
        var css = "p";
        Assert.That(
            () => CalculateTagIndexes(sample, css),
            Throws.ArgumentException);
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