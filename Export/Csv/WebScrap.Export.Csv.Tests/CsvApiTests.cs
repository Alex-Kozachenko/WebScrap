namespace WebScrap.Export.Csv.Tests;

public class CsvApiTests
{
    [Test]
    public void Export_SingleTag_ShouldWork()
    {
        string header = "Header";
        string[] html = [
            "<span>Foo</span>", 
            "<div>Bar</div>", 
            "<a>Buzz</a>"];
        string[] expected = [header, "Foo", "Bar", "Buzz"];
        var actual = CsvApi.Export(header, html);
        Assert.That(actual, Is.EquivalentTo(expected));
    }
}