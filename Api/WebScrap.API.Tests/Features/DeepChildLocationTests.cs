namespace WebScrap.API.Tests.Features;

public class DeepChildLocationTests
{
    
    [TestCase(Category=Categories.Features)]
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

    [TestCase(Category=Categories.Features)]
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

    [TestCase(Category=Categories.Features)]
    public void Test3()
    {
        var css ="div.container b";
        var html = """
        <main class="container">
            <p> Lorem <b> Ipsum </b> </p>
            <b> Clatu <b> Barata <b> Nictu </b> </b> </b>
        </main>
        """;

        string[] expected = [
            "<b> Ipsum </b>",
            "<b> Clatu <b> Barata <b> Nictu </b> </b> </b>"];

        var actual = Extract.Html(html, css);
        Assert.That(actual, Is.EquivalentTo(expected));
    }
}