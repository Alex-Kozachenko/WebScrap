using static WebScrap.Tags.TagBase;

namespace WebScrap.Tags.Tests;

public class TagBaseTests
{
    [Test]
    public void Create_ShouldWork()
    {
        var html = """
            <p id='foo'> Bar </p>
        """;
        var expected = new {
            Id = new string[] { "foo" },
            Text = " Bar ",
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
        });
    }

    [Test]
    public void Create_WithSelfClosingTag_ShouldWork()
    {
        var html = """
            <br />
        """;
        var expected = new { Name = "br", IsSelfClosing = true };
        var result = Create(html) as OpeningTag;
        Assert.That(result, Is.TypeOf<OpeningTag>());
        Assert.Multiple(() =>
        {
            Assert.That(result.Name.ToString(), Is.EquivalentTo(expected.Name));
            Assert.That(result.IsSelfClosing, Is.EqualTo(expected.IsSelfClosing));
            Assert.That(result.Attributes, Has.Count.EqualTo(0));
        });
    }
}