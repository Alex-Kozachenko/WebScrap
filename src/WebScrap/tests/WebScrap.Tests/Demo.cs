namespace WebScrap.Tests.IntegrationTests;

public class Demo
{
    [Test]
    [Explicit]
    public void Demo1()
    {
        var css = ".target";
        var html = """
            <main>
                <span class="target"> Two </span>
                <span class="target buzz"> Three </span>
                <span id="four" class="target buzz"> Four </span>
                <table class="target">
                    <tr> <th> Key </th> <th> Value </th> </tr>
                    <tr> <td> Width </td> <td> 2 </td> </tr>
                    <tr> <td> Height </td> <td> 3 </td> </tr>
                </table>
            </main>
        """;

        string expected = """
        [
            {
                "key": ".target",
                "values": 
                [
                    { "value": "Two" },
                    { "value": "Three" },
                    { "value": "Four" },
                    { "value": 
                        {
                            "headers": ["Key", "Value"],
                            "values": [
                                ["Width", "2"],
                                ["Height", "3"]
                            ]
                        }
                    }
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
    [Explicit]
    public void Demo_Url()
    {
        var request = "https://www.gpucheck.com/gpu-benchmark-graphics-card-comparison-chart";

        // Download the html:
        using var client = new HttpClient();
        using var response = client.GetAsync(request).Result;
        var html = response.Content.ReadAsStringAsync().Result;

        // Run the WebScrapper:
        var css = "table";
        var resultJson = new WebScrapper(html)
            .Run(css)
            .AsJson();

        var result = resultJson.ToJsonString();
    }
}