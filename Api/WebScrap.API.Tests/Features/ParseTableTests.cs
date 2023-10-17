namespace WebScrap.API.Tests;

[TestFixture(Category=Categories.Features)]
public class ParseTableTests
{
    [Test]
    public void Test1()
    {
        var css = "table";
        var html = """
        <table>
            <tr> <th> Key </th> <th> Value </th> </tr>
            <tr> <td> Width </td> <td> 2 </td> </tr>
            <tr> <td> Height </td> <td> 3 </td> </tr>
        </table>
        """;

        string[][] expected = [
            ["Key", "Value"],
            ["Width", "2"],
            ["Height", "3"]];

        var extractedHtml = new Scrapper().Scrap(html, css)
            .AsHtml()
            .First();

        var actual = Parser.ParseTable(extractedHtml);
        Assert.That(actual, Is.EquivalentTo(expected));
    }
}