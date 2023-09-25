using WebScrap.Processors;
using static WebScrap.IntegrationTests.TestHelpers.IsHelpers;

namespace WebScrap.Features.IntegrationTests;

[TestFixture]
public class HtmlExtractionFeatureTests
{
    [Test]
    public void SingleTag_ShouldExtract()
    {
        var css = "main>div>p";
        var expected = "<p> One </p>";
        var html = """
        <main>
            <div>
                <p> One </p>
            </div>
        </main>
        """;

        var actual = ExtractHtmlTags(html, css);
        Assert.That(actual, EquivalentTo([expected]));
    }

    [Test]
    public void SingleTag_OnTheMiddle_ShouldExtract()
    {
        var css = "main>div>p";
        var expected = "<p> One </p>";
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

        var actual = ExtractHtmlTags(html, css);
        Assert.That(actual, EquivalentTo([expected]));
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

        var actual = ExtractHtmlTags(html, css);
        Assert.That(actual, EquivalentTo(expected));
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
        var expected = "<p>One cup of <strong>a caffeine</strong> for a <i>good</i> start! </p>";

        var actual = ExtractHtmlTags(html, css);
        Assert.That(actual, EquivalentTo([expected]));
    }

    private static string[] ExtractHtmlTags(
        string html,
        string css)
        => CssProcessor.CalculateTagIndexes(html, css)
            .Select(tagIndex => new Range(
                tagIndex,
                tagIndex + TagsProcessor.GetEntireTagLength(html.Substring(tagIndex))))
            .Select(range => html[range])
            .ToArray();
}