using NUnit.Framework.Internal;
using Core.Internal.HtmlProcessing.Extractors;

namespace Core.Internal.HtmlProcessing.Tests;

[TestFixture]
public class TextExtractorTests
{
    [TestCase("<p>12</p>","12")]
    public void ReadBody_ShouldWork(string html, string expected)
    {
        var actual = HtmlProcessor.ReadBody(html).ToString();
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    [TestCase("12<p>34</p>")]
    [TestCase("12</p>34")]
    [TestCase("</p>12")]
    public void ReadBody_ShouldThrow(string html)
    {
        Assert.That(() => HtmlProcessor.ReadBody(html), Throws.Exception);
        
    }
}