using static Core.HtmlStreamReader;

namespace Core.Tests;

[TestFixture(Description = "Tests for trivial css children selector: >.")]
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
    public void Read_ShouldReturn_InnerText_Including_NestedInnerText()
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

    [Test] public void Read_ShouldReturn_Multuple_InnerText(){}
    [Test] public void Read_ShouldSkip_IrrelevantTags_Then_ReturnInnerText() {}
}