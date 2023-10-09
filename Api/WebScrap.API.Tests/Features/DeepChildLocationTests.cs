namespace WebScrap.API.Tests.Features;

[TestFixture(Category=Categories.Features)]
public class DeepChildLocationTests
{
    [Test]
    public void Test1()
    {
        var css = "div.container b";
        var html = """
        <main>
            <div class='container'>
                <p> LoremIpsum </p>
                <p> Lorem <b> Ipsum </b> </p>
                <b> Important </b>
            </div>
        </main>
        """;

        string[] expected = [
            "<b> Ipsum </b>",
            "<b> Important </b>"];
        
        var actual = Extract.Html(html, css);
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    [Test]
    public void Test2()
    {
        var css ="div.container b";
        var html = """
        <main class="container">
            <div class="container">
                <p> Lorem <b> Ipsum </b> </p>
            </div>
            <div>
                <p> Lorem <b> Ipsum </b> </p>
            </div>   
        </main>
        """;

        string[] expected = [
            "<b> Ipsum </b>",
            "<b> Ipsum </b>"];

        var actual = Extract.Html(html, css);
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    [Test]
    public void Test3()
    {
        var css ="main.container b";
        var html = """
        <main class="container">
            <div class="container">
                <p> Lorem <b> Ipsum </b> </p>
                <b> Clatu <b> Barata <b> Nictu </b> </b> </b>
            </div>
        </main>
        """;

        string[] expected = [
            "<b> Ipsum </b>",
            "<b> Clatu <b> Barata <b> Nictu </b> </b> </b>",
            "<b> Barata <b> Nictu </b> </b>",
            "<b> Nictu </b>"];

        var actual = Extract.Html(html, css);
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    [Test]
    public void Test4()
    {
        var css = "main>div>p#foo span.bar";
        var html = """
            <main>
                <br />
                <div>
                    <p> <div> <span class="bar"> Ignored </span> </div> </p>
                    <p id="foo"> 
                        Important!
                        <ul>
                            <li> <span> One </span> </li>
                            <li> <span class="bar"> Two </span> </li>
                            <li> <span class="bar buzz"> Three </span> </li>
                        </ul>
                        <div>
                            <span id="four" class="bar buzz"> Four </span>
                        </div>
                    </p>
                </div>
            </main>
        """;
        var htmlEntries = Extract.Html(html, css);

        string[] expected = [
            """<span class="bar"> Two </span>""",
            """<span class="bar buzz"> Three </span>""",
            """<span id="four" class="bar buzz"> Four </span>"""
        ];
        Assert.That(htmlEntries, Is.EquivalentTo(expected));
    }
}