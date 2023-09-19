using static Core.Html.Reading.Text.HtmlTextProcessor;

[TestFixture]
public class HtmlTextProcessorTests
{
    [TestCase("<p>12</p>","12")]
    public void ReadBody_ShouldWork(string html, string expected)
    {
        var actual = ReadBody(html).ToString();
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    [TestCase("12<p>34</p>")]
    [TestCase("12</p>34")]
    [TestCase("</p>12")]
    public void ReadBody_ShouldThrow(string html)
    {
        Assert.That(() => ReadBody(html), Throws.Exception);
        
    }
}