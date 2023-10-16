namespace WebScrap.Export.Csv.Tools.Tests;

public class HtmlTests
{
    [TestCase("<span>Foo</span>", ExpectedResult = "Foo")]
    public string Strip_SingleTag_ShouldWork(string html)
    {
        return Html.Strip(html);
    }
}