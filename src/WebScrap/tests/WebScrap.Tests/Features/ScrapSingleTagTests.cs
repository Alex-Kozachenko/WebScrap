namespace WebScrap.Tests.Features;

public class ScrapSingleTagTests
{
    [Test]
    public void Scrap_SingleTag_AsJson_ShouldWork()
    {
        var css = "p";
        var html = """
        <main>
            <p> Preface </p>
            <div> 
                <p> Content </p>
            </div>
        </main>
        """;

        string expected = """
        [
            {
                "key": "p",
                "values": 
                [
                    { "value":"Preface" },
                    { "value":"Content" },
                ]
            }
        ]
        """;

        var actual = new WebScrapper(html)
            .Run(css)
            .AsJson()
            .ToJsonString();

        Helpers.AssertJson(expected, actual);
    }

    [Test]
    public void Scrap_SingleTag_MultipleCss_AsJson_ShouldWork()
    {
        string[] css = [ ".preface", ".content" ];
        var html = """
        <main>
            <p class="preface"> Preface </p>
            <div> 
                <p class="content"> Content </p>
            </div>
        </main>
        """;

        string expected = """
        [
            {
                "key": ".preface",
                "values": 
                [
                    { "value":"Preface" },
                ]
            },
            {
                "key": ".content",
                "values": 
                [
                    { "value":"Content" },
                ]
            }
        ]
        """;

        var actual = new WebScrapper(html)
            .Run(css)
            .AsJson()
            .ToJsonString();

        Helpers.AssertJson(expected, actual);
    }
}