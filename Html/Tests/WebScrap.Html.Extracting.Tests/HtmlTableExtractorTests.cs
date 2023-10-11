namespace WebScrap.Html.Extracting.Tests;

[TestFixture]
public class HtmlTableExtractorTests
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

        string[][] expected = [
            ["<th> Key </th>", "<th> Value </th>"],
            ["<td> Width </td>", "<td> 2 </td>"],
            ["<td> Height </td>", "<td> 3 </td>"]];

        var actual = new HtmlTableExtractor().ExtractTable(html);
        Assert.That(actual, Is.EquivalentTo(expected));
    }
}