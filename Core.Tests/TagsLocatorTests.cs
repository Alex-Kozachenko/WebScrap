using static Core.TagsLocator;
using static Core.Tests.TestHelpers.IsHelpers;

[TestFixture]
public class TagsLocatorTests
{
    [Test]
    public void LocateTagRanges_ShouldWork()
    {
        var css = "main>div>p";
        var html = """
        <main>
            <div>
                <p> One </p>
            </div>
        </main>
        """;

        var expected = "<p> One </p>";

        var range = LocateTagRanges(html, css).First();
        var actual = html[range];

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void LocateTagRanges_ShouldWork_WhenSuccessfullBranch_IsInterrupted()
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
        var expected = "<p> One </p>";

        var range = LocateTagRanges(html, css).First();
        var actual = html[range];
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void LocateTagRanges_ShouldWork_OnMultipleBranches()
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
        string[] expected = ["<p>One</p>","<p>Two</p>"];

        var ranges = LocateTagRanges(html, css);
        var actual = ranges.Select(r => html[r]).ToArray();
        Assert.That(actual, EquivalentTo(expected));
    }

    public void LocateTagRanges_ShouldReturn_InnerText()
    {
        var css = "main>div>p";
        var html = """
        <main>
            <div>
                <p>One cup of <strong>a caffeine</strong> for a <i>good</i> start! </p>
            </div>
        </main>
        """;
        var expected = "<p>One cup of <strong>a caffeine</strong> for a <i>good</i> start! </p>";

        var range = LocateTagRanges(html, css).First();
        var actual = html[range];
        Assert.That(actual, Is.EqualTo(expected));
    }
}