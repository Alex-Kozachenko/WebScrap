using System.Text.Json;
using System.Text.Json.Nodes;
using WebScrap.Modules.Exporting.Json.Tools;

namespace WebScrap.Modules.Exporting.Json;

public class JsonApi
{
    public static JsonArray Export(IEnumerable<ReadOnlyMemory<char>> tags)
    {
        var result = new List<JsonNode>();
        foreach (var tag in tags)
        {
            var isSpecialTag = false;
            var strippedTag = Html.Strip(tag.Span);
            var obj = new { value = strippedTag };
            result.Add(JsonSerializer.SerializeToNode(obj)!);
        }

        return new JsonArray([..result]);
    }
}
