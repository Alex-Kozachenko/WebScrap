using static WebScrap.Processors.HtmlStripProcessor;

namespace WebScrap.Processors.Tests;

[TestFixture]
public class HtmlStripProcessorTests
{
    [TestCase("<p>12</p>","12")]
    public void ExtractText_ShouldWork(string html, string expected)
    {
        var actual = ExtractText(html).ToString();
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    [TestCase("12<p>34</p>")]
    [TestCase("12</p>34")]
    public void ExtractText_ShouldFail(string html)
    {
        Assert.That(() => ExtractText(html), Throws.Exception);        
    }
}