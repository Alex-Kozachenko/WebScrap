using static WebScrap.Tags.Processors.TagStripProcessor;

namespace WebScrap.Tags.Processors.Tests;

[TestFixture]
public class TagStripProcessorTests
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