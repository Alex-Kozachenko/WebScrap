using WebScrap.Modules.Extracting.Html.Contracts.Data;

namespace WebScrap.Modules.Extracting.Html;

public class TableExtractor : ITableExtractor
{
    public Table ExtractTable(ReadOnlySpan<char> html)
    {
        var headersRanges = new TableHeadersProcessor().ProcessHeaders(html);
        var valuesRanges = new TableValuesProcessor().ProcessValues(html);

        return new(
            ExtractHeader(html.ToString(), headersRanges),
            ExtractValueRows(html.ToString(), valuesRanges));
    }

    private static string[] ExtractHeader(string html, Range[] headersRanges)
        => headersRanges.Select(x => html[x].Trim(' ')).ToArray();

    private static string[][] ExtractValueRows(
        string html, 
        Range[][] valueRowsRanges)
    {
        var valuesRows = new List<string[]>();
        foreach (var rowValuesRange in valueRowsRanges)
        {
            var row = new List<string>();
            foreach (var valuesRange in rowValuesRange)
            {
                row.Add(html[valuesRange].Trim(' '));
            }
            valuesRows.Add([.. row]);
        }

        return [..valuesRows];
    }
}