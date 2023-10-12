namespace WebScrap.Core.Tags.Tests;

public class TagsProcessorBase_SingleTag_Tests
{
    private TagsProcessorBase processor;

    [SetUp]
    public void Setup()
    {
        processor = new();
    }

    [Test]
    public void Process_Single_Empty_Tag()
    {
        var html = "<main></main>";
        var result = processor.Process(html);

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Length.EqualTo(1));
            Assert.That(result[0], Has.Length.EqualTo(html.Length));
        });

        Assert.That(result[0].TextLength, Is.EqualTo(0));
        
        Assert.Multiple(() =>
        {
            Assert.That(result[0].Metadata.Name, Is.EqualTo("main"));
            Assert.That(result[0].Metadata.Attributes, Is.Empty);
        });
    }

    [Test]
    public void Process_Single_Filled_Tag()
    {
        var html = "<main>Lorem</main>";
        var result = processor.Process(html);

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Length.EqualTo(1));
            Assert.That(result[0], Has.Length.EqualTo(html.Length));
        });

        Assert.Multiple(() =>
        {
            Assert.That(result[0].TextLength, Is.EqualTo("Lorem".Length));
            Assert.That(html[result[0].InnerTextRange], Is.EqualTo("Lorem"));
        });
        
        Assert.That(result[0].Metadata.Name, Is.EqualTo("main"));
        Assert.That(result[0].Metadata.Attributes, Is.Empty);
    }

    [Test]
    public void Process_Single_Filled_Attributed_Tag()
    {
        var html = "<main id='main' class='bar buzz' data-id='id' >Lorem</main>";
        var result = processor.Process(html);

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Length.EqualTo(1));
            Assert.That(result[0], Has.Length.EqualTo(html.Length));
        });

        Assert.Multiple(() =>
        {
            Assert.That(result[0].TextLength, Is.EqualTo("Lorem".Length));
            Assert.That(html[result[0].InnerTextRange], Is.EqualTo("Lorem"));
        });
        
        Assert.That(result[0].Metadata.Name, Is.EqualTo("main"));

        Assert.Multiple(() =>
        {
            Assert.That(result[0].Metadata.Attributes["id"], Contains.Item("main"));
            Assert.That(result[0].Metadata.Attributes["class"], Contains.Item("bar"));
            Assert.That(result[0].Metadata.Attributes["class"], Contains.Item("buzz"));
            Assert.That(result[0].Metadata.Attributes["data-id"], Contains.Item("id"));
        });
    }
}