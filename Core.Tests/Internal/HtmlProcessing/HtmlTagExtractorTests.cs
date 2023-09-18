using NUnit.Framework.Internal;
using Core.Internal.HtmlProcessing.Extractors;

namespace Core.Internal.HtmlProcessing.Tests;

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

        var actual = HtmlTagExtractor.ExtractTag(html).ToString();
        Assert.That(actual, Is.EquivalentTo(expected));
    }
}