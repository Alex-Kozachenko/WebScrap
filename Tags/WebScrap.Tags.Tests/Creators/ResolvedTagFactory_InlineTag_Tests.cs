using WebScrap.Tags.Creators;
using WebScrap.Common.Tags;
using WebScrap.Common.Tags.Creators;

namespace WebScrap.Tags.Tests;

[TestFixture]
public class ResolvedTagFactory_InlineTag_Tests
{
    private readonly TagFactory factory = new ResolvedTagFactory();

    [Test]
    public void Create_ShouldReturn_Name()
    {
        var html = """
            <br />
        """;
        var expected = new { Name = "br" };
        var result = factory.Create(html) as InlineTag;
        Assert.That(result, Is.TypeOf<InlineTag>());
        Assert.Multiple(() =>
        {
            Assert.That(result.Name.ToString(),
                Is.EquivalentTo(expected.Name));
            Assert.That(result.Attributes,
               Has.Count.EqualTo(0));
        });
    }

    [Test]
    public void Create_ShouldReturn_Values()
    {
        var html = """
            <br id="foo" class="bar buzz" />
        """;
        var expected = new
        {
            Id = new string [] { "foo" },
            Cls = new string[] { "bar", "buzz" }
        };
        var result = factory.Create(html) as InlineTag;
        Assert.That(result, Is.TypeOf<InlineTag>());
        Assert.Multiple(() =>
        {
            Assert.That(result.Attributes["id"], 
                Is.EquivalentTo(expected.Id));
            Assert.That(result.Attributes["class"], 
                Is.EquivalentTo(expected.Cls));
        });
    }

    private static void AssertAttributeExistance(OpeningTag result, string[] keys)
    {
        foreach (var key in keys)
        {
            Assert.That(result.Attributes.Contains(key),
                Is.True,
                () => $"Attribute {key} is missing.");
        }
    }
}