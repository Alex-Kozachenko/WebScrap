using WebScrap.Core.Tags.Tests.TestHelpers;

namespace WebScrap.Core.Tags.Tests;

public class TagsProvider_MultipleTags_Tests
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
    public void Process_MultipleTags()
    {
        var html = $"<main> <div> <p>LoremIpsum</p> </div> </main>";
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
}