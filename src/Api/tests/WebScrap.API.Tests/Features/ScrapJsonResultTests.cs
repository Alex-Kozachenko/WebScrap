namespace WebScrap.API.Tests.Features;

public class ExtractJsonResultTests
{
    [Test]
    public void Scarp_SingleTag_AsJson_ShouldWork()
    {
        var scrapper = new Scrapper();
        var css = "p";
        var html = """
        <main>
            <p> Preface </p>
            <div> 
                <p> Content </p>
            </div>
        </main>
        """;

        string[] expected = [
            """{"value":"Preface"}""", 
            """{"value":"Content"}"""
        ];

        var actual = scrapper
            .Scrap(html, css)
            .AsJson();

        Assert.That(actual, Is.EquivalentTo(expected));
    }
}