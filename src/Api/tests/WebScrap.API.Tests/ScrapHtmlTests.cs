using static WebScrap.API.Tests.Helpers;

namespace WebScrap.Features.IntegrationTests;

[TestFixture]
public class ScrapHtmlTests
{
    [Test]
    public void Scrap_SingleEmptyTag_ShouldExtract()
    {
        var css = "main";
        var html = "<main></main>";
        var expected = """
        [
            {
                "key": "main",
                "values": []
            }
        ]
        """;

        var actual = new Scrapper()
            .Scrap(html, css)
            .AsJson()
            .ToJsonString();

        AssertJson(expected, actual);
    }

    [Test]
    public void Scrap_SingleTag_WithSpacesHtml_ShouldExtract()
    {
        var css = "main";
        var html = "   <main></main>  ";
        var expected = """
        [
            {
                "key": "main",
                "values": []
            }
        ]
        """;

        var actual = new Scrapper()
            .Scrap(html, css)
            .AsJson()
            .ToJsonString();

        AssertJson(expected, actual);
    }

    [Test]
    public void Scrap_SingleTag_ComplexCss_ShouldExtract()
    {
        var css = "main>div>p";
        var html = """
        <main>
            <div>
                <p> One </p>
            </div>
        </main>
        """;
        
        var expected = """
        [
            {
                "key": "main>div>p",
                "values": [
                    {
                        "value": "One"
                    }
                ]
            }
        ]
        """;

        var actual = new Scrapper()
            .Scrap(html, css)
            .AsJson()
            .ToJsonString();

        AssertJson(expected, actual);
    }

    [Test]
    public void Scrap_SingleTag_OnTheMiddle_ShouldExtract()
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

        var expected = """
        [
            {
                "key": "main>div>p",
                "values": [
                    {
                        "value": "One"
                    }
                ]
            }
        ]
        """;

        var actual = new Scrapper()
            .Scrap(html, css)
            .AsJson()
            .ToJsonString();

        AssertJson(expected, actual);
    }

    [Test]
    public void Scrap_MultipleTags_ShouldExtract()
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

        var expected = """
        [
            {
                "key": "main>div>p",
                "values": [
                    {
                        "value": "One"
                    },
                    {
                        "value": "Two"
                    }
                ]
            }
        ]
        """;

        var actual = new Scrapper()
            .Scrap(html, css)
            .AsJson()
            .ToJsonString();

        AssertJson(expected, actual);
    }

    [Test]
    public void Scrap_ComplexTag_ShouldExtract()
    {
        var css = "main>div>p";
        var html = """
        <main>
            <div>
                <p>One cup of <strong>a caffeine</strong> for a <i>good</i> start! </p>
            </div>
        </main>
        """;

        var expected = """
        [
            {
                "key": "main>div>p",
                "values": [
                    {
                        "value": "One cup of a caffeine for a good start!"
                    },
                ]
            }
        ]
        """;

        var actual = new Scrapper()
            .Scrap(html, css)
            .AsJson()
            .ToJsonString();

        AssertJson(expected, actual);
    }

    [Test]
    public void Scrap_AttributedTags_Simple_ShouldExtract()
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

        var expected = """
        [
            {
                "key": "p#main.some.classes",
                "values": [
                    { "value": "One" },
                    { "value": "Two" },
                    { "value": "Important" },
                    { "value": "One" },
                    { "value": "Two" },
                ]
            }
        ]
        """;

        var actual = new Scrapper()
            .Scrap(html, css)
            .AsJson()
            .ToJsonString();

        AssertJson(expected, actual);
    }
}