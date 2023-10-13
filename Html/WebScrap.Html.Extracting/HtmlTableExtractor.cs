namespace WebScrap.Html.Extracting;

public class HtmlTableExtractor
{
    public string[][] ExtractTable(ReadOnlySpan<char> html)
    {
        var ranges = new
        {
            headers = new TableHeadersProcessor()
                .ProcessHeaders(html),
            values = new TableValuesProcessor()
                .ProcessValues(html)
        };

        return ExtractStrings(
            html.ToString(),
            ranges.headers, 
            ranges.values);
    }

    private string[][] ExtractStrings(
        string html, 
        Range[] headersRanges, 
        Range[][] rowValuesRanges)
    {
        var headers = headersRanges.Select(x => html[x].Trim(' ')).ToArray();
        var valuesRows = new List<string[]>();

        foreach (var rowValuesRange in rowValuesRanges)
        {
            var row = new List<string>();
            foreach (var valuesRange in rowValuesRange)
            {
                row.Add(html[valuesRange].Trim(' '));
            }
            valuesRows.Add([.. row]);
        }

        return [[..headers], ..valuesRows];
    }
}