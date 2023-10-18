using System.Text.Json;
using System.Text.Json.Nodes;
using WebScrap.Modules.Extracting.Html.Contracts;
using WebScrap.Modules.Extracting.Html.Text;

namespace WebScrap.Modules.Exporting.Json;

public class JsonApi
{
    private static readonly ITextExtractor extractor = new TextExtractor();
    public static JsonArray Export(IEnumerable<ReadOnlyMemory<char>> tags)
    {
        var result = new List<JsonNode>();
        foreach (var tag in tags)
        {
            var strippedTag = extractor.ExtractText(tag.Span);
            var obj = new { value = strippedTag };
            result.Add(JsonSerializer.SerializeToNode(obj)!);
        }

        return new JsonArray([..result]);
    }
}
