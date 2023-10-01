
namespace WebScrap.Features.IntegrationTests;

[TestFixture]
public class HtmlExtractionFeatureTests
{
    [Test]
    public void SingleTag_ShouldExtract()
    {
        var css = "main>div>p";
        string[] expected = ["<p> One </p>"];
        var html = """
        <main>
            <div>
                <p> One </p>
            </div>
        </main>
        """;

        var actual = API.ExtractHtml(html, css);
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    [Test]
    public void SingleTag_OnTheMiddle_ShouldExtract()
    {
        var css = "main>div>p";
        string[] expected = ["<p> One </p>"];
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

        var actual = API.ExtractHtml(html, css);
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    [Test]
    public void MultipleTags_ShouldExtract()
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

        var actual = API.ExtractHtml(html, css);
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    [Test]
    public void ComplexTag_ShouldExtract()
    {
        var css = "main>div>p";
        var html = """
        <main>
            <div>
                <p>One cup of <strong>a caffeine</strong> for a <i>good</i> start! </p>
            </div>
        </main>
        """;
        string[] expected = ["<p>One cup of <strong>a caffeine</strong> for a <i>good</i> start! </p>"];

        var actual = API.ExtractHtml(html, css);
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    [Test]
    public void AttributedTags_Simple_ShouldExtract()
    {
        var css = "p#main.some.classes";
        var html = """
        <main>
            <div>
                <p id='main' class='some classes'> One </p>
                <p id='main' class='some bigger classes'> Two </p>
            </div>
            <br />
            <p>Nevermind</p>
            <p> Nevermind </p>
            <p id='main' class='some classes'> Important </p>
            <div>
                <p id='main' class='some classes'> One </p>
                <p> Nevermind </p>
                <p>Nevermind</p>
                <p id='main' class='some bigger classes'> Two </p>
            </div>
        </main>
        """;

        string[] expected = 
        [
            "<p id='main' class='some classes'> One </p>",
            "<p id='main' class='some bigger classes'> Two </p>",
            "<p id='main' class='some classes'> Important </p>",
            "<p id='main' class='some classes'> One </p>",
            "<p id='main' class='some bigger classes'> Two </p>"
        ];

        var actual = API.ExtractHtml(html, css);
        Assert.That(actual, Is.EquivalentTo(expected));
    }
}