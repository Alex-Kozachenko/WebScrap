namespace WebScrap.API.KnownIssues.Tests;

public class CssSelectors_KnownIssues_Tests
{
    [TestCase("*", Description="*")]
    [TestCase("div>*", Description="*")]
    [TestCase("*>div", Description="*")]
    [TestCase("div *", Description="*")]
    [TestCase("* div", Description="*")]
    [TestCase("[data]", Description="[]")]
    [TestCase(".some.class, .some.other.class", Description=",")]
    public void ExtractHtml_FailsToDetect_SomeCss(string css)
    {
        var html = "<div> LoremIpsum </div>";
        Assert.That(() => Extract.Html(html, css), Throws.Exception);
    }
}