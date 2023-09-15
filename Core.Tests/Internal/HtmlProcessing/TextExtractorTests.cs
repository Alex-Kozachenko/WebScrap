using NUnit.Framework.Internal;
using Core.Internal.HtmlProcessing.Extractors;

namespace Core.Internal.HtmlProcessing.Tests;

[TestFixture]
public class TextExtractorTests
{
    [TestCase("<p>12</p>","12")]
    public void ReadBody_ShouldWork(string html, string expected)
    {
        var actual = TextExtractor.ReadBody(html).ToString();
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    [TestCase("12<p>34</p>")]
    [TestCase("12</p>34")]
    [TestCase("</p>12")]
    public void ReadBody_ShouldThrow(string html)
    {
        Assert.That(() => TextExtractor.ReadBody(html), Throws.Exception);
        
    }

    [Test]
    public void TrimTag_ShouldWork()
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