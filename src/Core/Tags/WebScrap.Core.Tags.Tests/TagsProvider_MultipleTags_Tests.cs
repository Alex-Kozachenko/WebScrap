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
        var text = "LoremIpsum";
        var html = $"<main> <div> <p>{text}</p> </div> </main>";

        tagsProvider.Process(html);

        var result = listener.Messages;
        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Length.EqualTo(3));
            Assert.That(result[0].CurrentTag, Has.Length.EqualTo(html.Length));
        });

        Assert.Multiple(() => {
            Assert.That(result[2].CurrentTag.TextLength, 
                Is.EqualTo(text.Length));
                
            Assert.That(html[result[2].CurrentTag.InnerTextRange], 
                Is.EqualTo(text));
        });
    }
}