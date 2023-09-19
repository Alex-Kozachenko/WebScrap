using NUnit.Framework.Internal;
using static Core.Html.Reading.Tags.HtmlTagExtractor;

[TestFixture]
public class HtmlTagExtractorTests
{
    [Test]
    public void ExtractTag_ShouldWork()
    {
        var expected = """
            <div>
                <p>
                    <b> Lorem ipsum </b>
                </p>
            </div>
        """;

        var html = expected + """
            Lorem ipsum...
            <div> <p> Lorem ipsum </p> </div>            
        """;

        var actual = ExtractTag(html).ToString();
        Assert.That(actual, Is.EquivalentTo(expected));
    }
}