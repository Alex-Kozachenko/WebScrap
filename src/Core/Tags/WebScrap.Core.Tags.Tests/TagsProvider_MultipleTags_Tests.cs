namespace WebScrap.Core.Tags.Tests;

public class TagsProvider_MultipleTags_Tests
{
    private TagsProvider tagsProvider;

    [SetUp]
    public void Setup()
    {
        tagsProvider = new();
    }

    [Test]
    public void Process_MultipleTags()
    {
        var text = "LoremIpsum";
        var html = $"<main> <div> <p>{text}</p> </div> </main>";
        var result = tagsProvider.Process(html);

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Length.EqualTo(3));
            Assert.That(result[0], Has.Length.EqualTo(html.Length));
        });

        Assert.Multiple(() => {
            Assert.That(result[2].TextLength, Is.EqualTo(text.Length));
            Assert.That(html[result[2].InnerTextRange], Is.EqualTo(text));
        });
    }
}