using WebScrap.Tags.Creators;
using WebScrap.Common.Tags;
using WebScrap.Common.Tags.Creators;

namespace WebScrap.Tags.Tests;

[TestFixture]
public class ResolvedTagFactory_OpeningTag_Tests
{
    private readonly TagFactory factory = new ResolvedTagFactory();

    [Test]
    public void Create_ShouldReturn_Name()
    {
        var html = """
            <aside></aside>
        """;

        var expected = "aside";
        var result = factory.Create(html) as OpeningTag;
        Assert.That(result, Is.TypeOf<OpeningTag>());
        
        Assert.That(result.Name,
            Is.EqualTo(expected));
        Assert.That(result.Attributes, 
           Has.Count.EqualTo(0));
    }
    [Test]
    public void Create_WithApostropheAttr_ShouldReturn_Data()
    {
        var html = """
            <p id='foo'></p>
        """;
        var expected = new string [] { "foo" };
        var result = factory.Create(html) as OpeningTag;
        Assert.That(result, Is.TypeOf<OpeningTag>());

        Assert.That(result.Attributes["id"],
            Is.EquivalentTo(expected));
    }

    [Test]
    public void Create_WithQuotedAttr_ShouldReturn_Data()
    {
        var html = """
            <p class="quick brown fox"></p>
        """;
        
        string[] expected = ["quick", "brown", "fox"];
        var result = factory.Create(html) as OpeningTag;
        Assert.That(result, Is.TypeOf<OpeningTag>());

        Assert.That(result.Attributes["class"], 
            Is.EquivalentTo(expected));
    }

    [Test]
    public void Create_WithEmptyAttr_ShouldReturn_AttrName()
    {
        var html = """
            <p some empty attributes></p>
        """;
        string[] expected = ["some", "empty", "attributes"];

        var result = factory.Create(html) as OpeningTag;
        Assert.That(result, Is.TypeOf<OpeningTag>());

        Assert.Multiple(() => 
        {
            AssertAttributeExistance(result, expected);
        });
    }

    [Test]
    public void Create_WithDataAttr_ShouldReturn_Data()
    {
        var html = """
            <p begin data-centric="bar buzz" end></p>
        """;
        var expected = new {
            DataCentric = new string[] { "bar", "buzz" },
            Keys = new string[] { 
                "begin", 
                "data-centric", 
                "end"
            }
        };
        var result = factory.Create(html) as OpeningTag;
        Assert.That(result, Is.TypeOf<OpeningTag>());

        Assert.Multiple(() => 
        {
            AssertAttributeExistance(result, expected.Keys);
        });

        Assert.That(result.Attributes["data-centric"], 
            Is.EquivalentTo(expected.DataCentric));
    }

    [Test]
    public void Create_WithEmptyTag_ShouldWork()
    {
        var html = """
            <p></p>
        """;
        var expected = "p";
        var result = factory.Create(html) as OpeningTag;
        Assert.That(result, Is.TypeOf<OpeningTag>());
        Assert.Multiple(() =>
        {
            Assert.That(result.Name.ToString(), Is.EquivalentTo(expected));
            Assert.That(result.Attributes, Has.Count.EqualTo(0));
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