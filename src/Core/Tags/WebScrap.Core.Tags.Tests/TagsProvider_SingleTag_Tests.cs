using WebScrap.Core.Tags.Tests.TestHelpers;

namespace WebScrap.Core.Tags.Tests;

public class TagsProvider_SingleTag_Tests
{
    private TagsProvider tagsProvider;
    private TagsProviderListener listener;
    

    [SetUp]
    public void Setup()
    {
        tagsProvider = new();
        listener = new();
        listener.Subscribe(tagsProvider);
    }

    [Test]
    public void Process_Single_Empty_Tag()
    {
        var html = "<main></main>";

        tagsProvider.Process(html);
        var result = listener.ProcessedTags;

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Length.EqualTo(1));
            Assert.That(result[0].TagLength, Is.EqualTo(html.Length));
            Assert.That(result[0].TextLength, Is.EqualTo(0));
        });
        
        Assert.Multiple(() =>
        {
            Assert.That(result[0].TagInfo.Name, Is.EqualTo("main"));
            Assert.That(result[0].TagInfo.Attributes, Is.Empty);
        });
    }

    [Test]
    public void Process_Single_Filled_Tag()
    {
        var html = "<main>Lorem</main>";

        tagsProvider.Process(html);
        var result = listener.ProcessedTags;

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Length.EqualTo(1));
            Assert.That(result[0].TagLength, Is.EqualTo(html.Length));
        });

        Assert.Multiple(() =>
        {
            Assert.That(result[0].TextLength, Is.EqualTo("Lorem".Length));
            Assert.That(html[result[0].InnerTextRange], Is.EqualTo("Lorem"));
        });
        
        Assert.That(result[0].TagInfo.Name, Is.EqualTo("main"));
        Assert.That(result[0].TagInfo.Attributes, Is.Empty);
    }

    [Test]
    public void Process_Single_Filled_Attributed_Tag()
    {
        var html = "<main id='idmain' class='bar buzz' data-id=id data-value=\"value\">Lorem</main>";

        tagsProvider.Process(html);
        var result = listener.ProcessedTags;

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Length.EqualTo(1));
            Assert.That(result[0].TagLength, Is.EqualTo(html.Length));
        });

        Assert.Multiple(() =>
        {
            Assert.That(result[0].TextLength, Is.EqualTo("Lorem".Length));
            Assert.That(html[result[0].InnerTextRange], Is.EqualTo("Lorem"));
        });
        
        Assert.That(result[0].TagInfo.Name, Is.EqualTo("main"));

        Assert.Multiple(() =>
        {
            Assert.That(result[0].TagInfo.Attributes["id"], Contains.Item("idmain"));
            Assert.That(result[0].TagInfo.Attributes["class"], Contains.Item("bar"));
            Assert.That(result[0].TagInfo.Attributes["class"], Contains.Item("buzz"));
            Assert.That(result[0].TagInfo.Attributes["data-id"], Contains.Item("id"));
            Assert.That(result[0].TagInfo.Attributes["data-value"], Contains.Item("value"));
        });
    }

    [Test]
    public void Process_MultipleTags_WithComment()
    {
        var html = "<div>\r\n<!-- <div> Ignored </div> -->\r\n</div>";

        tagsProvider.Process(html);
        var result = listener.ProcessedTags;

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Length.EqualTo(1));
            Assert.That(result[0].TagLength, Is.EqualTo(html.Length));
        });
    }
}