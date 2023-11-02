using WebScrap.Core.Tags.Tests.TestHelpers;

namespace WebScrap.Core.Tags.Tests.KnownIssues;

public class TagsProvider_KnownIssues
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
    /// <summary>
    /// It could be a frequent case later, 
    /// but now the engine is not ready.
    /// </summary>
    public void Process_Ignores_SelfClosingTag()
    {
        var html = "<br />";
        tagsProvider.Process(html);
        Assert.That(listener.Messages, Has.Length.EqualTo(0));
    }
}