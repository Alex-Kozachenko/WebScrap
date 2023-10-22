using System.Text.Json;
using System.Text.Json.Nodes;

namespace WebScrap.API.Tests;

internal static class Helpers
{
    internal static void AssertJson(string expected, string actual)
    {
        var exArr = ParseArray(expected);
        var acArr = ParseArray(actual);
        var equals = JsonNode.DeepEquals(exArr, acArr);
        Assert.That(equals, Is.True);
    }

    internal static void AssertJson(
        IEnumerable<string> expected, 
        IEnumerable<string> actual)
    {
        for (int i = 0; i < actual.Count(); i++)
        {
            var ex = expected.ElementAt(i);
            var ac = actual.ElementAt(i);
            AssertJson(ex, ac);
        }
    }

    private static JsonNode? ParseArray(string array)
        => JsonNode.Parse(array, 
            documentOptions: new JsonDocumentOptions() 
                {
                     AllowTrailingCommas = true 
                });
}