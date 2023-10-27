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
        Assert.That(equals, Is.True, () => $"Incorrect json: {actual}");
    }

    private static JsonNode? ParseArray(string array)
        => JsonNode.Parse(array, 
            documentOptions: new JsonDocumentOptions() 
                {
                     AllowTrailingCommas = true 
                });
}