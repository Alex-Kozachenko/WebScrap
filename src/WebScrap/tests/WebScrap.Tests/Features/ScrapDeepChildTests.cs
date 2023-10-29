using static WebScrap.Tests.Helpers;

namespace WebScrap.Tests.Features;

[TestFixture(Category=Categories.Features)]
public class ExtractDeepChildTests
{
    [Test]
    public void Test1()
    {
        var css = "div.container b";
        var html = """
        <main>
            <div class='container'>
                <p> LoremIpsum </p>
                <p> Lorem <b> Ipsum </b> </p>
                <b> Important </b>
            </div>
        </main>
        """;

        var expected = """
        [
            {
                "key": "div.container b",
                "values": [
                    { "value": "Ipsum" },
                    { "value": "Important" },
                ]
            }
        ]
        """;

        var actual = new WebScrapper(html)
            .Run(css)
            .AsJson()
            .ToJsonString();

        AssertJson(expected, actual);
    }

    [Test]
    public void Test2()
    {
        var css ="div.container b";
        var html = """
        <main class="container">
            <div class="container">
                <p> Lorem <b> Ipsum </b> </p>
            </div>
            <div>
                <p> Lorem <b> Ipsum </b> </p>
            </div>   
        </main>
        """;

        var expected = """
        [
            {
                "key": "div.container b",
                "values": [
                    { "value": "Ipsum" },
                    { "value": "Ipsum" }
                ]
            }
        ]
        """;

        var actual = new WebScrapper(html)
            .Run(css)
            .AsJson()
            .ToJsonString();

        AssertJson(expected, actual);
    }

    [Test]
    public void Test3()
    {
        var css ="main.container b";
        var html = """
        <main class="container">
            <div class="container">
                <p> Lorem <b>Ipsum</b> </p>
                <b> Clatu <b>Barata<b> Nictu</b></b></b>
            </div>
        </main>
        """;
        var expected = """
        [
            {
                "key": "main.container b",
                "values": [
                    { "value": "Ipsum" },
                    { "value": "Nictu" },
                    { "value": "Barata Nictu" },
                    { "value": "Clatu Barata Nictu" }
                ]
            }
        ]
        """;

        var actual = new WebScrapper(html)
            .Run(css)
            .AsJson()
            .ToJsonString();

        AssertJson(expected, actual);
    }

    [Test]
    public void Test4()
    {
        var css = "main>div>p#foo span.bar";
        var html = """
            <main>
                <br />
                <div>
                    <p> <div> <span class="bar"> Ignored </span> </div> </p>
                    <p id="foo"> 
                        Important!
                        <ul>
                            <li> <span> One </span> </li>
                            <li> <span class="bar"> Two </span> </li>
                            <li> <span class="bar buzz"> Three </span> </li>
                        </ul>
                        <div>
                            <span id="four" class="bar buzz"> Four </span>
                        </div>
                    </p>
                </div>
            </main>
        """;

        var expected = """
        [
            {
                "key": "main>div>p#foo span.bar",
                "values": [
                    { "value": "Two" },
                    { "value": "Three" },
                    { "value": "Four" },
                ]
            }
        ]
        """;

        var actual = new WebScrapper(html)
            .Run(css)
            .AsJson()
            .ToJsonString();

        AssertJson(expected, actual);
    }
}