namespace WebScrap.API.KnownIssues.Tests;

public class Navigation_KnownIssues_Tests
{
    [TestCase(Category=Categories.KnownIssues)]
    public void ExtractHtml_UnableToAccept_Html_With_WhitespacesOnStart()
    {
        var css = "div";
        var html ="    <div></div>";

        Assert.That(() => 
            Extract.Html(html, css),
            Throws.Exception);
    }

    [Test]
    public void ExtractHtml_DropsProcessing_DueTo_TagName()
    {
        var css = "p.some";
        var html = """
        <div>
            <p class="some"> One </p>
            <p> Ignore </p>
            <p class="some"> Two </p>
        </div>
        """;

        // Because there is <p> Ignore </p>.
        string[] expected = """
            <p class="some"> One </p>
            <p> Ignore </p>
            <p class="some"> Two </p>
        """.Split(Environment.NewLine);

        string[] actual = ["""
            <p class="some"> One </p>
        """];

        var wrongResult = Extract.Html(html, css);
        Assert.Pass();
    }

    [Test]
    public void ExtractHtml_OnTaglessCss_ReturnsNothing()
    {
        var css = ".some";
        var html = """
        <div>
            <p class="some"> One </p>
        </div>
        """;

        string[] expected = """
            <p class="some"> One </p>
        """.Split(Environment.NewLine);

        string[] actual = [];

        // NOTE: Check CssTokenBuilder.Build.
        var wrongResult = Extract.Html(html, css);
        Assert.Pass();
    }
}