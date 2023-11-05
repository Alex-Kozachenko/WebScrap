namespace WebScrap.Modules.Extracting.Html.Text.Tests;

public class TextExtractorTests
{
    [Test]
    public void ExtractText_NestedTags_ReturnsAllText()
    {
        var html = "<p>One cup of <strong>a caffeine</strong> for a <i>good</i> start! </p>";
        var expected = "One cup of a caffeine for a good start!";
        var actual = new TextExtractor().ExtractText(html);
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    [Test]
    public void ExtractText_SpecialChars_ShouldWork()
    {
        var html = "<div>One cup of cofee for \u0026#36;300.</div>";
        var expected = "One cup of cofee for $300.";
        var actual = new TextExtractor().ExtractText(html);
        Assert.That(actual, Is.EquivalentTo(expected));
    }

}