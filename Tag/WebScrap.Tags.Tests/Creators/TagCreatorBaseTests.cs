using static WebScrap.Tags.Creators.TagCreatorBase;

namespace WebScrap.Tags.Tests;

public class TagCreatorBaseTests
{
    [Test]
    public void Create_ShouldWork()
    {
        var html = """
            <p id='foo'> Bar </p>
        """;
        var expected = new {
            Id = new string[] { "foo" },
            Name = "p"
        };
        var result = Create(html) as OpeningTag;
        Assert.That(result, Is.TypeOf<OpeningTag>());
        Assert.Multiple(() =>
        {
            Assert.That(result.Name.ToString(), Is.EquivalentTo(expected.Name));
            Assert.That(result.Attributes["id"], Is.EquivalentTo(expected.Id));
        });
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