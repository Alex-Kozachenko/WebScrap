using static WebScrap.Tests.Helpers;

namespace WebScrap.Tests.Features;

[TestFixture(Category=Categories.Features)]
public class ExtractWildcardTests
{
    [Test]
    public void Test1()
    {
        var css = "*.bar";
        var html = """
        <main>
            <div>
                <p> <div> <span class="bar"> One </span> </div> </p>
                <p> <div class="bar"> Two <span class="bar">One</span></div></p>
                <p>
                    <ul>
                        <li> <span> One </span> </li>
                        <li> <span class="bar"> Two </span> </li>
                        <li> <span class="bar buzz"> Three </span> </li>
                    </ul>
                </p>
            </div>
        </main>
        """;

        var expected = """
        [
            {
                "key": "*.bar",
                "values": [
                    { "value": "One" },
                    { "value": "One" },
                    { "value": "Two One" },
                    { "value": "Two" },
                    { "value": "Three" },
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