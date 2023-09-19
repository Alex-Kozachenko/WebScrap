using static Core.TagsLocator;
using static Core.Tests.TestHelpers.IsHelpers;

[TestFixture]
public class TagsLocatorTests
{
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