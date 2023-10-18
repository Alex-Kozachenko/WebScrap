using System.Text.Json;
using System.Text.Json.Nodes;
using WebScrap.Modules.Extracting.Html.Tables;
using WebScrap.Modules.Extracting.Html.Text;

namespace WebScrap.Modules.Exporting.Json.Processors;

internal class JsonProcessor
{
    internal JsonArray Process(IEnumerable<ReadOnlyMemory<char>> tags)
    {
        var result = new List<JsonNode>();

        foreach (var tag in tags)
        {
            var obj = new { value = ExtractValue(tag) };
            result.Add(JsonSerializer.SerializeToNode(obj)!);
        }

        return new JsonArray([..result]);
    }

    private JsonElement ExtractValue(ReadOnlyMemory<char> tag)
        => tag.Span switch 
        {
            var s when s.StartsWith("<table")
                => ExtractTable(tag),
            _ => ExtractString(tag)
        };

    private JsonElement ExtractTable(ReadOnlyMemory<char> tag)
    {
        var table = new TableExtractor().ExtractTable(tag.Span);
        var result = new 
        {
            headers = table.Header,
            values = table.ValueRows
        };
        var a = JsonSerializer.SerializeToElement(result);
        return a;
    }

    private static JsonElement ExtractString(ReadOnlyMemory<char> tag)
    {
        var result = new TextExtractor().ExtractText(tag.Span);
        return JsonSerializer.SerializeToElement(result);
    }
}