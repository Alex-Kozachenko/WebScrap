namespace WebScrap.API.Tests;

[TestFixture(Category=Categories.Features)]
public class ParseTableTests
{
    [Test]
    public void Scrap_Table_AsJson_Returns_ConsistentJson()
    {
        var css = "table";
        var html = """
        <table>
            <tr> <th>Key</th> <th>Value</th> </tr>
            <tr> <td>Width</td> <td>2</td> </tr>
            <tr> <td>Height</td> <td>3</td> </tr>
        </table>
        """;

        string expected = """
        {
            "value": 
            {
                "headers": ["Key", "Value"],
                "values": [
                    ["Width", "2"],
                    ["Height", "3"]
                ]
            }
        }
        """;

        var actual = new Scrapper().Scrap(html, css)
            .AsJson()
            .First();

        Helpers.AssertJson(expected, actual);
    }
}