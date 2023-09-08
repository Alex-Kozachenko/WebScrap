namespace Core.Tests;

[TestFixture]
public class HtmlStreamReaderTests
{
    [TestCase("main div p")]
    [TestCase("main div>p")]
    public void Read_ShouldReturn_InnerText(string css)
    {
        var html = """
        <main>
            <div>
                <p> One </p>
            </div>
        </main>
        """;

        var expected ="p";
        // var expected = " One "; // final case
        var readResult = new HtmlStreamReader().Read(html, css);
        Assert.That(readResult, Is.EqualTo(expected));
    }    
}