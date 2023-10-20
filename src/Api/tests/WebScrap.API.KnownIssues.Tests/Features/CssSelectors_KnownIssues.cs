namespace WebScrap.API.KnownIssues.Tests;

public class CssSelectors_KnownIssues
{
    [TestCase("[data]", Description="[]", Category=Categories.KnownIssues)]
    [TestCase(".some.class, .some.other.class", Description=",", Category=Categories.KnownIssues)]
    public void ExtractHtml_FailsToDetect_SomeCss(string css)
    {
        var html = "<div> LoremIpsum </div>";
        Assert.That(() => new Scrapper().Scrap(html, css), Throws.Exception);
    }
}