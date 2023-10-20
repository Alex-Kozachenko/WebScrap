namespace WebScrap.Modules.Extracting.Html.Text.Tests;

public class TextExtractorTests
{
    [Test]
    public void ExtractText_NestedTags_ReturnsAllText()
    {
        var html = "<div>Some <p>paragraph about <b>important </b</p>things</div>";
        var expected = "Some paragraph about important things";
        var actual = new TextExtractor().ExtractText(html);
        Assert.That(actual, Is.EquivalentTo(expected));
    }
}