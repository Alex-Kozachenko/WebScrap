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
            <tr> <td> Width </td> 2 <td> </tr>
            <tr> <td> Height </td> 3 <td> </tr>
        </table>
        """;

        string[][] expected = [
            ["<th> Key </th>", "<th> Value </th>"],
            ["<td> Width </td>", "<td> 2 </td>"],
            ["<td> Height </td>", "<td> 3 </td>"]];

        var actual = Parse.Table(html, css);
        Assert.That(actual, Is.EquivalentTo(expected));
    }
}