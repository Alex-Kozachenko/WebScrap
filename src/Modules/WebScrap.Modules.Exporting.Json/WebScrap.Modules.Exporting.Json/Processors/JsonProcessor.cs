using System.Text.Json;
using System.Text.Json.Nodes;
using WebScrap.Modules.Extracting.Html.Tables;
using WebScrap.Modules.Extracting.Html.Text;

namespace WebScrap.Modules.Exporting.Json.Processors;

internal class JsonProcessor
{
    internal JsonArray Process(IEnumerable<string> tags)
    {
        var result = new List<JsonNode>();

        foreach (var tag in tags)
        {
            var obj = new { value = ExtractValue(tag) };
            if (obj.value.GetRawText().Trim('\"') != string.Empty)
            {
                result.Add(JsonSerializer.SerializeToNode(obj)!);
            }
        }

        return new JsonArray([..result]);
    }

    JsonElement ExtractValue(string tag)
        => tag switch 
        {
            var s when s.StartsWith("<table")
                => ExtractTable(tag),
            _ => ExtractString(tag)
        };

    JsonElement ExtractTable(string tag)
    {
        var table = new TableExtractor().ExtractTable(tag);
        var result = new 
        {
            headers = table.Header,
            values = table.ValueRows
        };
        return JsonSerializer.SerializeToElement(result);
    }

    static JsonElement ExtractString(string tag)
    {
        var result = new TextExtractor().ExtractText(tag);
        return JsonSerializer.SerializeToElement(result);
    }
}