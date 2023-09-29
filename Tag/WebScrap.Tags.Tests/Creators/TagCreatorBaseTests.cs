using static WebScrap.Tags.Creators.TagCreatorBase;

namespace WebScrap.Tags.Tests;

[TestFixture]
public class TagCreatorBaseTests
{
    [Test]
    public void Create_ShouldWork()
    {
        var html = """
            <p id='foo' class="buzz" empty multi="quick brown fox" data-foo data-centric="bar buzz" final="final">Bar</p>
        """;
        var expected = new {
            Id = new string[] { "foo" },
            Cls = new string[] { "buzz" },
            Multi = new string[] { "quick", "brown", "fox" },
            Name = "p",
            DataCentric = new string[] { "bar", "buzz" },
            Final = new string[] { "final" },
            Keys = new string[] { 
                "id", 
                "class", 
                "empty", 
                "multi", 
                "data-foo", 
                "data-centric", 
                "final"
            }
        };
        var result = Create(html) as OpeningTag;
        Assert.That(result, Is.TypeOf<OpeningTag>());

        Assert.That(result.Name.ToString(), 
            Is.EquivalentTo(expected.Name));

        Assert.Multiple(() => 
        {
            AssertAttributeExistance(result, expected.Keys);
        });

        Assert.Multiple(() =>
        {
            Assert.That(result.Attributes["id"],
                Is.EquivalentTo(expected.Id));
            Assert.That(result.Attributes["class"], 
                Is.EquivalentTo(expected.Cls));
            Assert.That(result.Attributes["multi"],
                Is.EquivalentTo(expected.Multi));
            Assert.That(result.Attributes["data-centric"], 
                Is.EquivalentTo(expected.DataCentric));
            Assert.That(result.Attributes["final"], 
                Is.EquivalentTo(expected.Final));
        });
    }

    [Test]
    public void Create_ShouldWork_FOO()
    {
        var html = """
            <p begin data-centric="bar buzz" end>Bar</p>
        """;
        var expected = new {
            DataCentric = new string[] { "bar", "buzz" },
            Keys = new string[] { 
                "begin", 
                "data-centric", 
                "end"
            }
        };
        var result = Create(html) as OpeningTag;
        Assert.That(result, Is.TypeOf<OpeningTag>());

        Assert.Multiple(() => 
        {
            AssertAttributeExistance(result, expected.Keys);
        });

        Assert.That(result.Attributes["data-centric"], 
            Is.EquivalentTo(expected.DataCentric));
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

    [Test]
    public void Create_WithEmptyTag_ShouldWork()
    {
        var html = """
            <p></p>
        """;
        var expected = "p";
        var result = Create(html) as OpeningTag;
        Assert.That(result, Is.TypeOf<OpeningTag>());
        Assert.Multiple(() =>
        {
            Assert.That(result.Name.ToString(), Is.EquivalentTo(expected));
            Assert.That(result.Attributes, Has.Count.EqualTo(0));
        });
    }

    [Test]
    public void Create_InlineTag_ShouldWork()
    {
        var html = """
            <br />
        """;
        var expected = new { Name = "br" };
        var result = Create(html) as InlineTag;
        Assert.That(result, Is.TypeOf<InlineTag>());
        Assert.Multiple(() =>
        {
            Assert.That(result.Name.ToString(), Is.EquivalentTo(expected.Name));
            Assert.That(result.Attributes, Has.Count.EqualTo(0));
        });
    }
}