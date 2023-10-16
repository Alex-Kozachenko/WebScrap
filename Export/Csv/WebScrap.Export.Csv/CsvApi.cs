using WebScrap.Export.Csv.Tools;

namespace WebScrap.Export.Csv;

/// <summary>
/// Represents the API for export HTML in CSV format.
/// </summary>
public class CsvApi
{
    public static string[] Export(string header, string[] html)
        => [header, ..html.Select(x => Html.Strip(x))];
}
