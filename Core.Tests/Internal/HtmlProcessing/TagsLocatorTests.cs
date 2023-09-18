using static Core.Internal.HtmlProcessing.TagsLocator;

namespace Core.Internal.HtmlProcessing.Tests;

[TestFixture]
public class TagsLocatorTests
{

    #region cases
    [TestCase(
        "_____<p></p>",
        "     ^"
    )]
    [TestCase(
        "p>__<p>",
        "    ^"
    )]
    [TestCase(
        "<p>__<p>",
        "^"
    )]
    [TestCase(
        "__</p>__",
        "  ^"
    )]
    #endregion
    public void GetNextTagIndex_ShouldWork(string sample, string targetPointer)
    {
        var expected = targetPointer.Length - 1;
        var result = GetNextTagIndex(sample);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void LocateTagsByCss_ShouldWork()
    {
        var css = "main>div>p";
        var html = """
        <main>
            <div>
                <p> One </p>
            </div>
        </main>
        """;

        var expected = "One";
        var actual = new string(LocateTagsByCss(html, css).First()).Strip();
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    [Test]
    public void LocateTagsByCss_ShouldWork_WhenSuccessfullBranch_IsInterrupted()
    {
        var css = "main>div>p";
        var html = """
        <main>
            <p>
            </p>
            <div>
                <span> Nonsense </span>
                <p> One </p>
            </div>
        </main>
        """;

        var expected = "One";
        var actual = new string(LocateTagsByCss(html, css).First()).Strip();
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    [Test]
    public void LocateTagsByCss_ShouldWork_OnMultipleBranches()
    {
        var css = "main>div>p";
        var html = """
        <main>
            <br />
            <div>
                <p> One </p>
            </div>
            <br />
            <div>
                <p> Two</p>
            </div>
        </main>
        """;

        string[] expected = ["One","Two"];
        var actual = LocateTagsByCss(html, css).Select(x => new string(x).Strip()).ToArray();
        Assert.That(actual, Is.EquivalentTo(expected));
    }
}