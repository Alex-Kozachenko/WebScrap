namespace Core.Tests;


[TestFixture]
public class HtmlStreamReaderTests
{
    [Test]
    public void TokenizeCss_ShouldWork()
    {
        var sample = "main div#foo>p.bar";
        CssToken[] expected =
        [
            new ("main"),
            new ("div#foo", CssDescendanceKind.Deep),
            new ("p.bar", CssDescendanceKind.Child)
        ];
        var result = HtmlStreamReader.TokenizeCss(sample);
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [TestCase("main div.foo p")]
    [TestCase("main div.foo>p")]
    public void ShouldRecognize_ContainerStart(string css)
    {
        var html = """
        <main>
            <div class="foo">
                <p> One </p>
            </div>
        </main>
        """;
        
        var reader = new HtmlStreamReader();
        var readResult = reader.Read(html, css);
        Assert.That(readResult,Is.Not.EqualTo(0));
    }
    
    // [Test]
    // public void Check_ShouldRecognize_HtmlStructure_Basic()
    // {
    //     var manifest = "div#container table#exact td[0]";

    //     var checker = new TagsChecker(manifest);
    //     Assert.Multiple(() =>
    //     {

    //         Assert.That(checker.Check("div#container"), Is.EqualTo("yes"));
    //         Assert.That(checker.Check("table#exact"), Is.EqualTo("yes"));
    //         Assert.That(checker.Check("td[0]"), Is.EqualTo("finally"));
    //     });
    // }

    // [Test]
    // public void SomeCaseWhichICantTestNow()
    // {
    //     var html =
    //     """ 
    //         <div> </div>
    //         <aside> <p> Unexpected </p> </aside>
    //     """;

    //     var manifest = "div p";

    //     Assert.Fail("imagine an html which tries to fool my engine.");
    // }

    // [Test]
    // public void ThisOneCouldFail()
    // {
    //     var html =
    //     """ 
    //         <div> <div> </div> <p> Expected </p> </div>
    //     """;

    //     Assert.Fail("The engine could exit BEFORE p-tag.");
    // }
}