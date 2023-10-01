namespace WebScrap.Tests.IntegrationTests;

public class Demo
{
    [Test]
    public void Demo1()
    {
        var css = "main>div>p.foo";
        var html = """
            <main>
                <br />
                <div>
                    <p class="foo bar">One</p>
                </div>
                <br />
                <div>
                    <p class="foo buzz">Two</p>
                </div>
            </main>
        """;
        html = html.TrimStart(' ');

        var htmlEntries = API.ExtractHtml(html, css);

        string[] expected = [
            """<p class="foo bar">One</p>""",
            """<p class="foo buzz">Two</p>"""
        ];
        Assert.That(htmlEntries, Is.EquivalentTo(expected));
    }
}