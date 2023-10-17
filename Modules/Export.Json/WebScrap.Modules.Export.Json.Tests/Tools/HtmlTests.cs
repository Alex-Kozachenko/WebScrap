using WebScrap.Modules.Export.Json.Tools;

namespace WebScrap.Modules.Export.Json.Tests.Tools;

public class HtmlTests
{
    [TestCase("<span>Foo</span>", ExpectedResult = "Foo")]
    public string Strip_SingleTag_ShouldWork(string html) 
        => Html.Strip(html);

    [Test]
    public void Strip_SingleTag_Nested_ShouldWork()
    {
        var html = "<div>Foo <p>Bar <span>Buzz <span>raB </p>ooF</div>";
        var expected = "Foo Bar Buzz raB ooF";
        var actual = Html.Strip(html);
        Assert.That(actual, Is.EquivalentTo(expected));
    }
}