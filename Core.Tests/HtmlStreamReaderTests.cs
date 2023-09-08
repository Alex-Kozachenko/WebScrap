using static Core.HtmlStreamReader;

namespace Core.Tests;

[TestFixture]
public class HtmlStreamReaderTests
{
    [Test]
    public void Read_ShouldReturn_InnerText()
    {
        var css = "main>div>p".AsMemory();
        var html = """
            <main>
                <div>
                    <p> One </p>
                </div>
            </main>
        """;

        var expected = " One ";
        var readResult = Read(html, css).ToString();
        Assert.That(readResult, Is.EqualTo(expected));
    }    

    [Test]
    public void Read_ShouldReturn_InnerText2()
    {
        var css = "main>div>p".AsMemory();
        var html = """
            <main>
                <div>
                    <p> One <p>Two</p> </p>
                </div>
            </main>
        """;

        var expected = " One Two ";
        var readResult = Read(html, css).ToString();
        Assert.That(readResult, Is.EqualTo(expected));
    }    
}