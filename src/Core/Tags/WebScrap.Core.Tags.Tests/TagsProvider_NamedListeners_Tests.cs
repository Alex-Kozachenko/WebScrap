using WebScrap.Core.Tags.Tests.TestHelpers;

namespace WebScrap.Core.Tags.Tests;

public class TagsProvider_NamedListeners_Tests
{

    [Test]
    public void Process_Single_Named_Tag()
    {
        var expected = new string[] { "Rock", "Paper", "Scissors" };

        var tagsProvider = new TagsProvider();
        var listener = new TagsProviderListener();
        tagsProvider.Subscribe(listener, "li");

        var html = """
                <ul> 
                    <li>Rock</li>
                    <li>Paper</li>
                    <li>Scissors</li>
                </ul> 
        """;

        tagsProvider.Process(html);
        
        var result = GetText(listener, html);
        Assert.That(result, Is.EquivalentTo(expected));
    }


    [Test]
    public void Process_Multiple_Named_Tag()
    {
        var expected = new string[] { "Terminal", "tasks", "any", "close" };

        var tagsProvider = new TagsProvider();
        var listener = new TagsProviderListener();
        tagsProvider.Subscribe(listener, "i", "b");

        var html = """
        <p>
            <b>Terminal</b> will be reused by <b>tasks</b>, press <i>any</i> key to <i>close</i> it. 
        </p>
        """;

        tagsProvider.Process(html);

        var result = GetText(listener, html);        
        Assert.That(result, Is.EquivalentTo(expected));
    }

    static string[] GetText(TagsProviderListener listener, string html)
    {
        return listener.ProcessedTags
            .Select(x => html[x.InnerTextRange]
                .ToString())
            .ToArray();
    }
}