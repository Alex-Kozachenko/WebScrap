using static Core.Internal.HtmlProcessing.TagsLocator;
using static Core.Tests.TestHelpers.IsHelpers;

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

        var expected = " One ";

        var actual = LocateTagsByCss(html, css);
        Assert.That(actual, EquivalentTo([expected]));
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
        var expected = " One ";

        var actual = LocateTagsByCss(html, css);
        Assert.That(actual, EquivalentTo([expected]));
    }

    [Test]
    public void LocateTagsByCss_ShouldWork_OnMultipleBranches()
    {
        var css = "main>div>p";
        var html = """
        <main>
            <br />
            <div>
                <p>One</p>
            </div>
            <br />
            <div>
                <p>Two</p>
            </div>
        </main>
        """;
        string[] expected = ["One","Two"];

        var actual = LocateTagsByCss(html, css);
        Assert.That(actual, EquivalentTo(expected));
    }

    [Test]
    public void LocateTagsByCss_ShouldReturn_InnerText()
    {
        var css = "main>div>p";
        var html = """
        <main>
            <div>
                <p>One cup of <strong>a caffeine</strong> for a <i>good</i> start! </p>
            </div>
        </main>
        """;
        var expected = "One cup of a caffeine for a good start! ";

        var actual = LocateTagsByCss(html, css);
        Assert.That(actual, EquivalentTo([expected]));
    }
}