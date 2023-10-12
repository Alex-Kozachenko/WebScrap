namespace WebScrap.Core.Tags.Tests;

public class TagsProcessorBase_MultipleTags_Tests
{
    private TagsProcessorBase processor;

    [SetUp]
    public void Setup()
    {
        processor = new();
    }

    [Test]
    public void Process_MultipleTags()
    {
        var text = "LoremIpsum";
        var html = $"<main> <div> <p>{text}</p> </div> </main>";
        var result = processor.Process(html);

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