namespace WebScrap.KnownIssues.Tests;

public class CssSelectors_KnownIssues
{
    [TestCase("[data]", Description="[]", Category=Categories.KnownIssues)]
    [TestCase(".some.class, .some.other.class", Description=",", Category=Categories.KnownIssues)]
    public void ExtractHtml_FailsToDetect_SomeCss(string css)
    {
        var html = "<div> LoremIpsum </div>";
        Assert.That(() => new WebScrapper(html).Run(css), Throws.Exception);
    }
}