namespace WebScrap.Html.Extracting;

public class HtmlTableExtractor
{
    public string[][] ExtractTable(ReadOnlySpan<char> html)
    {
        var headersProcessor = new TableHeadersProcessor();
        headersProcessor.Run(html);

        var valuesProcessor = new TableValuesProcessor();
        valuesProcessor.Run(html);

        return ExtractStrings(
            html.ToString(), 
            headersProcessor.HeaderRanges, 
            valuesProcessor.ValuesRanges);
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