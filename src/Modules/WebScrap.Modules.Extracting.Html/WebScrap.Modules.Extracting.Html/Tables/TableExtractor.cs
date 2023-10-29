using WebScrap.Modules.Extracting.Html.Contracts.Data;
using WebScrap.Modules.Extracting.Html.Text;

namespace WebScrap.Modules.Extracting.Html.Tables;

public class TableExtractor : ITableExtractor
{
    readonly TextExtractor textExtractor = new();
    
    public Table ExtractTable(ReadOnlySpan<char> html)
    {
        var headersRanges = new TableHeadersProcessor().ProcessHeaders(html);
        var valuesRanges = new TableValuesProcessor().ProcessValues(html);

        return new(
            ExtractRow(html.ToString(), headersRanges),
            ExtractBody(html.ToString(), valuesRanges));
    }

    string[] ExtractRow(string html, Range[] cellRanges)
        => cellRanges
            .Select(x => textExtractor.ExtractText(html[x]))
            .ToArray();

    string[][] ExtractBody(string html, Range[][] valueRowsRanges) 
        => [.. valueRowsRanges.Select(x => ExtractRow(html, x))];
}