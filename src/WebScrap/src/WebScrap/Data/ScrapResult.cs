using System.Collections.Immutable;
using System.Text.Json.Nodes;
using WebScrap.Formatters;
using WebScrap.Modules.Exporting.Json;

namespace DevOvercome.WebScrap.Data;

/// <summary>
/// Represents the result of scrapping.
/// </summary>
internal readonly struct ScrapResult : IScrapResult
{
    readonly string html;
    readonly CssTagRanges[] cssTagRanges;

    internal ScrapResult(
        string html,
        params CssTagRanges[] cssTagRanges)
    {
        this.html = html;
        this.cssTagRanges = cssTagRanges;
    }

    /// <summary>
    /// Gets the stored value in JSON format.
    /// </summary>
    /// <returns>
    /// Array of json objects in string format.
    /// { value: %JsonValue% }
    /// where %JsonValue% is any JsonObject.
    /// </returns>
    public JsonArray AsJson()
    {
        var arrays = new List<(string css, JsonArray)>();
        foreach (var cssTagRange in cssTagRanges)
        {
            var tagStrings = ExtractStrings(html, cssTagRange.TagRanges);
            arrays.Add((cssTagRange.Css, JsonApi.Export(tagStrings)));
        }
        
        return JsonFormatter.Format([..arrays]);
    }

    static string[] ExtractStrings(
        string html,
        IEnumerable<Range> tagRanges)
        => tagRanges
            .Select(range => html[range])
            .Select(x => x.Trim())
            .ToArray();
}