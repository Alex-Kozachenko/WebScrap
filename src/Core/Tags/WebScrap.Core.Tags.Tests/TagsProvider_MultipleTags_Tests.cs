using WebScrap.Core.Tags.Tests.TestHelpers;

namespace WebScrap.Core.Tags.Tests;

public class TagsProvider_MultipleTags_Tests
{
    [Test]
    public void Process_MultipleTags()
    {
        var html = $"<main> <div> <p>LoremIpsum</p> </div> </main>";
        
        var tagsProvider = new TagsProvider();
        var listener = new TagsProviderListener();
        tagsProvider.Subscribe(listener);
        tagsProvider.Process(html);
        var result = listener.ProcessedTags;

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Length.EqualTo(3));
            Assert.That(result[^1].TagLength, Is.EqualTo(html.Length));
            Assert.That(result[0].TagInfo.Name, Is.EqualTo("p"));
            Assert.That(result[1].TagInfo.Name, Is.EqualTo("div"));
            Assert.That(result[2].TagInfo.Name, Is.EqualTo("main"));
        });
    }

    [Test]
    public void Process_ShouldReturn_DeepestTag_Text()
    {
        var html = $"<main> <div> <p>LoremIpsum</p> </div> </main>";

        var tagsProvider = new TagsProvider();
        var listener = new TagsProviderListener();
        tagsProvider.Subscribe(listener);
        tagsProvider.Process(html);
        var result = listener.ProcessedTags;

        Assert.Multiple(() => 
        {
            Assert.That(result[0].TagInfo.Name, 
                Is.EqualTo("p"));

            Assert.That(result[0].TextLength, 
                Is.EqualTo("LoremIpsum".Length));
                
            Assert.That(html[result[0].InnerTextRange], 
                Is.EqualTo("LoremIpsum"));
        });
    }

    [Test]
    public void Process_Rootless_ShouldReturn_Tags()
    {
        var html = """
            <p> Foo <b>Bar</b> </p>
            <p> Foo <b>Bar</b> </p>
        """;

        var tagsProvider = new TagsProvider();
        var listener = new TagsProviderListener();
        tagsProvider.Subscribe(listener, "b");
        tagsProvider.Process(html);
        var result = listener.ProcessedTags;

        Assert.Multiple(() => 
        {
            Assert.That(result[0].TagInfo.Name, 
                Is.EqualTo("b"));

            Assert.That(html[result[0].InnerTextRange], 
                Is.EqualTo("Bar"));                

            Assert.That(result[1].TagInfo.Name, 
                Is.EqualTo("b"));

            Assert.That(html[result[1].InnerTextRange], 
                Is.EqualTo("Bar"));                
            
        });
    }
}