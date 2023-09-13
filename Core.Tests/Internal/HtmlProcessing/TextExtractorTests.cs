using NUnit.Framework.Internal;

namespace Core.Internal.HtmlProcessing.Tests;

[TestFixture]
public class TextExtractorTests
{
    [TestCase("12<p>34</p>", "1234")]
    [TestCase("<p>12</p>","12")]
    [TestCase("12</p>34", "1234")]
    [TestCase("</p>12","12")]
    [TestCase("12<p>34<p>56</p>78</p>", "12345678")]
    public void ReadBody_ShouldWork(string html, string expected)
    {
        var actual = TextExtractor.ReadBody(html).ToString();
        Assert.That(actual, Is.EquivalentTo(expected));
    }
    
}