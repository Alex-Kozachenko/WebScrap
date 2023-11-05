namespace WebScrap.Modules.Extracting.Html.Tables.Tests;

[TestFixture]
public class TableExtractorTests
{
    [Test]
    public void ExtractTableTest()
    {
        var html = """
        <table>
            <tr> <th> Key </th> <th> Value </th> </tr>
            <tr> <td> Width </td> <td> 2 </td> </tr>
            <tr> <td> Height </td> <td> 3 </td> </tr>
        </table>
        """;

        string[] expectedHeader = ["Key", "Value"];
        string[][] expectedRowValues = [
            ["Width", "2"],
            ["Height", "3"]];

        var actual = new TableExtractor().ExtractTable(html);
        Assert.That(actual.Header, Is.EquivalentTo(expectedHeader));
        Assert.That(actual.ValueRows, Is.EquivalentTo(expectedRowValues));
    }

    [Test]
    public void ExtractTable_NestedHtml_Test()
    {
        var html = """
        <table>
            <tr> <th> <b>Key</b> </th> <th> <b>Value</b> </th> </tr>
            <tr> <td> <i>Width</i> </td> <td> 2 </td> </tr>
            <tr> <td> <i>Height</i> </td> <td> 3 </td> </tr>
        </table>
        """;

        string[] expectedHeader = ["Key", "Value"];
        string[][] expectedRowValues = [
            ["Width", "2"],
            ["Height", "3"]];

        var actual = new TableExtractor().ExtractTable(html);
        Assert.That(actual.Header, Is.EquivalentTo(expectedHeader));
        Assert.That(actual.ValueRows, Is.EquivalentTo(expectedRowValues));
    }
}