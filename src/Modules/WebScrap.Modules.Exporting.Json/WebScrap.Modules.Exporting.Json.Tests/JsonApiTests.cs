using System.Text.Json.Nodes;

namespace WebScrap.Modules.Exporting.Json.Tests;

public class JsonApiTests
{
    [Test]
    public void Export_EmptyTag_ShouldWork()
    {
        var html = "<div></div>";
        var expected = """[]""";
        var actual = JsonApi.Export([html]).ToJsonString();
        AssertJson(expected, actual);
    }

    [Test]
    public void Export_Tag_ShouldWork()
    {
        var html = "<div>foo</div>";
        var expected = """[{"value":"foo"}]""";
        var actual = JsonApi.Export([html]).ToJsonString();
        AssertJson(expected, actual);
    }

    [Test]
    public void Export_NestedTag_ShouldWork()
    {
        var html = "<p>One cup of <strong>a caffeine</strong> for a <i>good</i> start! </p>";
        var expected = """[{"value":"One cup of a caffeine for a good start!"}]""";
        var actual = JsonApi.Export([html]).ToJsonString();
        AssertJson(expected, actual);
    }

    [Test]
    public void Export_Table_ShouldWork()
    {
        var html = """
        <table>
            <tr>
                <th>th1</th>
                <th>th2</th>
            </tr>
            <tr>
                <td>td11</th>
                <td>td12</td>
            </tr>
            <tr>
                <td>td21</th>
                <td>td22</td>
            </tr>
        </table>
        """;

        var expected = """
        [
            {
                "value":
                {
                    "headers": ["th1", "th2"], 
                    "values":
                    [
                        [ "td11", "td12" ],
                        [ "td21", "td22" ]
                    ]
                }
            }
        ]
        """;

        var actual = JsonApi.Export([html]).ToJsonString();
        AssertJson(expected, actual);
    }

    private void AssertJson(string expected, string actual)
    {
        var exArr = JsonArray.Parse(expected);
        var acArr = JsonArray.Parse(actual);
        var equals = JsonNode.DeepEquals(exArr, acArr);
        Assert.That(equals, Is.True, () => $"Incorrect Json: {expected}");
    }
}