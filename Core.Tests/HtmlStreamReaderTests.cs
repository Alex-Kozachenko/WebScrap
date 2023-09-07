namespace Core.Tests;

[TestFixture]
public class HtmlStreamReaderTests
{
    [TestCase("main div.foo p")]
    [TestCase("main div.foo>p")]
    public void Read_ShouldReturn_InnerText(string css)
    {
        var html = """
        <main>
            <div class="foo">
                <p> One </p>
            </div>
        </main>
        """;

        var expected ="main";
        // var expected = " One "; // final case
        var readResult = new HtmlStreamReader().Read(html, css);
        Assert.That(readResult, Is.EqualTo(expected));
    }    
}