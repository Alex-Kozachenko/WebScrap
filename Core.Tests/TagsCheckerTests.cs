namespace Core.Tests;

[TestFixture]
public class TagsCheckerTests
{
    [Test]
    public void Check_ShouldRecognize_HtmlStructure_Basic()
    {
        var manifest = "div#container table#exact td[0]";

        var checker = new TagsChecker(manifest);
        Assert.That(checker.Check("div#container"), Is.EqualTo("yes"));
        Assert.That(checker.Check("table#exact"), Is.EqualTo("yes"));
        Assert.That(checker.Check("td[0]"), Is.EqualTo("finally"));
    }

    [Test]
    public void SomeCaseWhichICantTestNow()
    {
        var html = 
        """ 
            <div> </div>
            <aside> <p> Unexpected </p> </aside>
        """;

        var manifest = "div p";

        Assert.Fail("imagine an html which tries to fool my engine.");
    }

    [Test]
    public void ThisOneCouldFail()
    {
        var html = 
        """ 
            <div> <div> </div> <p> Expected </p> </div>
        """;
        
        Assert.Fail("The engine could exit BEFORE p-tag.");        
    }
}