using System.Collections.Immutable;
using System.Text.Json.Nodes;
using WebScrap.API.Formatters;
using WebScrap.Modules.Exporting.Json;

namespace WebScrap.API.Data;

/// <summary>
/// Represents the result of scrapping.
/// </summary>
public readonly ref struct ScrapResult
{
    private readonly ReadOnlySpan<char> html;
    readonly (ReadOnlyMemory<char>, Range[])[] cssTagRanges;

    internal ScrapResult(
        ReadOnlySpan<char> html,
        params (ReadOnlyMemory<char>, Range[])[] cssTagRanges)
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
        var arrays = new List<(ReadOnlyMemory<char> css, JsonArray)>();
        foreach (var cssTagRange in cssTagRanges)
        {
            var tagStrings = ExtractStrings(html.ToString(), cssTagRange.Item2);
            arrays.Add((cssTagRange.Item1, JsonApi.Export(tagStrings)));
        }
        
        return JsonFormatter.Format([..arrays]);
    }

    public ImmutableArray<string> AsHtml()
        => ExtractStrings(html.ToString(), cssTagRanges.First().Item2)
            .Select(x => x.ToString())
            .ToImmutableArray();

    private static ImmutableArray<ReadOnlyMemory<char>> ExtractStrings(
        string html,
        IEnumerable<Range> tagRanges)
        => tagRanges
            .Select(range => html[range])
            .Select(x => x.ToString())
            .Select(x => x.Trim())
            .Select(x => x.AsMemory())
            .ToImmutableArray();
}