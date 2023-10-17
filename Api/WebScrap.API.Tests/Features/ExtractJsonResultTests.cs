namespace WebScrap.API.Tests.Features;

public class ExtractJsonResultTests
{
    [Test]
    public void Extract_Json_SingleTag_ShouldWork()
    {
        var extract = new Extract(new Contracts.Config(Contracts.OutputFormatType.Json));
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
            """{"value":" Preface "}""", 
            """{"value":" Content "}"""
        ];

        var actual = extract.Html(html, css);
        Assert.That(actual, Is.EquivalentTo(expected));
    }
}