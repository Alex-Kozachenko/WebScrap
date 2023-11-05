using System.Text.Json;
using System.Text.Json.Nodes;

namespace WebScrap.Formatters;

internal static class JsonFormatter
{
    internal static JsonArray Format((string, JsonArray)[] arrays)
    {
        var result = new List<JsonNode>();
        foreach (var item in arrays)
        {
            var obj = new
            {
                key = item.Item1,
                values = item.Item2
            };
            result.Add(JsonSerializer.SerializeToNode(obj)!);
        }

        return new([..result]);
    }
}