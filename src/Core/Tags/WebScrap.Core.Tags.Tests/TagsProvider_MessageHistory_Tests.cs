using WebScrap.Core.Tags.Tests.TestHelpers;

namespace WebScrap.Core.Tags.Tests;

public class TagsProvider_MessageHistory_Tests
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
    public void Process_ShouldReturn_CorrectMessageOrder()
    {
        var text = "LoremIpsum";
        var html = $"<main> <div> <p>{text}</p> </div> </main>";
        tagsProvider.Process(html);

        Assert.Multiple( () => {
            var names = listener.Messages
                .Select(x => x.CurrentTag.TagInfo.Name)
                .ToArray();
            Assert.That(names[0], Is.EqualTo("p"));
            Assert.That(names[1], Is.EqualTo("div"));
            Assert.That(names[2], Is.EqualTo("main"));
        });
    }

    [Test]
    public void Process_FirstDeepestMessage_ShouldContain_CorrectHistory()
    {
        var text = "LoremIpsum";
        var html = $"<main> <div> <p>{text}</p> </div> </main>";
        tagsProvider.Process(html);

        Assert.That(listener.Messages[0].CurrentTag.TagInfo.Name, 
            Is.EqualTo("p"));

        Assert.Multiple( () => {
            var deepestHistory = listener.Messages[0]
                .TagsHistory
                .Select(x => x.Name)
                .ToArray();
            Assert.That(deepestHistory[0], Is.EqualTo("main"));
            Assert.That(deepestHistory[1], Is.EqualTo("div"));
            Assert.That(deepestHistory[2], Is.EqualTo("p"));
        });
    }
}