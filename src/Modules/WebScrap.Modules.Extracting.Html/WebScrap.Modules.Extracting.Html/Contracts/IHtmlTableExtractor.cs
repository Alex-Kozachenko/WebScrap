using WebScrap.Modules.Extracting.Html.Contracts.Data;

namespace WebScrap.Modules.Extracting.Html;

public interface ITableExtractor
{
    /// <summary>
    /// Extracts table row by row.
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    Table ExtractTable(ReadOnlySpan<char> html);
}
