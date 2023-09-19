using static Core.Html.Reading.Text.HtmlTextProcessor;

[TestFixture]
public class HtmlTextProcessorTests
{
    [TestCase("<p>12</p>","12")]
    public void Process_ShouldWork(string html, string expected)
    {
        var actual = Process(html).ToString();
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    [TestCase("12<p>34</p>")]
    [TestCase("12</p>34")]
    [TestCase("</p>12")]
    public void Process_ShouldThrow(string html)
    {
        Assert.That(() => Process(html), Throws.Exception);
        
    }
}