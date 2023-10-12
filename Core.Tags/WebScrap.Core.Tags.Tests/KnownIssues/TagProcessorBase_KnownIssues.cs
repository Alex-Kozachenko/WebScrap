namespace WebScrap.Core.Tags.Tests.KnownIssues;

public class TagsProcessorBase_KnownIssues
{
    [Test]
    /// <summary>
    /// This issue comes from generating the result when the closing tag met,
    /// so, the order is reversed: 
    /// main aside p
    /// If you read the html from the end to beginning, you will see this order.
    /// I think it is not essential for WebScrap, since I just pick the values, I dont really care the order.
    /// </summary>
    public void Process_MultipleTags_Returns_ReversedOrder()
    {
        TagsProcessorBase processor = new();
        var text = "LoremIpsum";
        var html = $"<main> <p>{text}</p> <aside></aside> </main>";
        var result = processor.Process(html);

        Assert.Multiple(() => {
            Assert.That(result[2].Metadata.Name, Is.EqualTo("p"));
            Assert.That(result[1].Metadata.Name, Is.EqualTo("aside"));
        });
    }
}