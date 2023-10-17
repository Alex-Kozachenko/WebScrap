namespace WebScrap.Tests.IntegrationTests;

public class Demo
{
    [Test]
    public void Demo1()
    {
        var css = "main>div>p#foo span.bar";
        var html = """
            <main>
                <br />
                <div>
                    <p> LoremIpsum </p>
                    <p id="foo"> 
                        <div>
                            <span class="bar"> Two </span>
                            <span class="bar buzz"> Three </span>
                            <span id="four" class="bar buzz"> Four </span>
                        </div>
                    </p>
                </div>
            </main>
        """;

        string[] expected = [
            """<span class="bar"> Two </span>""",
            """<span class="bar buzz"> Three </span>""",
            """<span id="four" class="bar buzz"> Four </span>"""
        ];

        var htmlEntries = new Scrapper()
            .Scrap(html, css)
            .AsHtml();

        Assert.That(htmlEntries, Is.EquivalentTo(expected));
    }
}