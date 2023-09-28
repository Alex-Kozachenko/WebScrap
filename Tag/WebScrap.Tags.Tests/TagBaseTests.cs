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
}