using System.Text.Json;
using System.Text.Json.Nodes;
using WebScrap.Modules.Export.Json.Tools;

namespace WebScrap.Modules.Export.Json;

public class JsonApi
{
    public static JsonArray Export(IEnumerable<ReadOnlyMemory<char>> tags)
    {
        var result = new List<JsonNode>();
        foreach (var tag in tags)
        {
            var strippedTag = Html.Strip(tag.Span);
            var obj = new { value = strippedTag };
            result.Add(JsonSerializer.SerializeToNode(obj)!);
        }

        return new JsonArray([..result]);
    }
}
