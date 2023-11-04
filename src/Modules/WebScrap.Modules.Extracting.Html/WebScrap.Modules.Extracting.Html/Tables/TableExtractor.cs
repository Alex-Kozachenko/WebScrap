using System.Collections.Immutable;
using WebScrap.Core.Tags;
using WebScrap.Modules.Extracting.Html.Contracts.Data;
using WebScrap.Modules.Extracting.Html.Text;

namespace WebScrap.Modules.Extracting.Html.Tables;

public class TableExtractor : ITableExtractor
{
    readonly TextExtractor textExtractor = new();
    
    public Table ExtractTable(ReadOnlySpan<char> html)
    {
        var tagsProvider = new TagsProvider();
        var tableHeadersProcessor = new TableHeadersProcessor()
            .Subscribe(tagsProvider);

        var tableValuesProcessor = new TableValuesProcessor()
            .Subscribe(tagsProvider);

        tagsProvider.Process(html);

        return new(
            ExtractRow(html.ToString(), tableHeadersProcessor.HeaderRanges),
            ExtractBody(html.ToString(), tableValuesProcessor.ValuesRanges));
    }

    string[] ExtractRow(string html, IEnumerable<Range> cellRanges)
        => cellRanges
            .Select(x => textExtractor.ExtractText(html[x]))
            .ToArray();

    string[][] ExtractBody(string html, IEnumerable<ImmutableArray<Range>> valueRowsRanges) 
        => [.. valueRowsRanges.Select(x => ExtractRow(html, x))];
}