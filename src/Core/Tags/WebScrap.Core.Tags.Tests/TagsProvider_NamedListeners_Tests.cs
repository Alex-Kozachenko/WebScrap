using WebScrap.Core.Tags.Tests.TestHelpers;

namespace WebScrap.Core.Tags.Tests;

public class TagsProvider_NamedListeners_Tests
{
    [Test]
    public void Process_Single_Empty_Tag()
    {
        var tagsProvider = new TagsProvider();
        var listener = new TagsProviderListener();
        tagsProvider.Subscribe(listener, "li");

        var html = """
                <ul> 
                    <li>First</li>
                    <li>Second</li>
                    <li>Third</li>
                </ul> 
        """;

        tagsProvider.Process(html);

        var result = listener.ProcessedTags;

        Assert.That(result, Has.Length.EqualTo(3));
        Assert.That(html[result[0].InnerTextRange].ToString(), Is.EqualTo("First"));
        Assert.That(html[result[1].InnerTextRange].ToString(), Is.EqualTo("Second"));
        Assert.That(html[result[2].InnerTextRange].ToString(), Is.EqualTo("Third"));
    }
}