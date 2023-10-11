using WebScrap.Css.Data.Selectors;

namespace WebScrap.Css.Preprocessing.Tests;

[TestFixture]
public class TokensBuilderTests
{
    [Test]
    public void Build_WithDescendants_ShouldWork()
    {
        var sample = "main div>p";
        (Type, string)[] expected =
        [
            new(typeof(RootCssSelector), "main"),
            new(typeof(AnyChildCssSelector), "div"),
            new(typeof(ChildCssSelector), "p"),
        ];

        (Type, string)[] result = new TokensBuilder()
            .Build(sample)
            .Select(x => (x.Selector.GetType(), x.Tag.ToString()))
            .ToArray();
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public void Build_WithMultipleSelectors_ShouldWork()
    {
        var sample = "p#foo.bar.buzz";
        var expected = new KeyValuePair<string, string>[]
        {
            new("id", "foo"),
            new("class", "bar"),
            new("class", "buzz"),
        }.ToLookup(x => x.Key, x => x.Value);

        var result = new TokensBuilder()
            .Build(sample)
            .First()
            .Attributes;

        Assert.That(result, Is.EquivalentTo(expected));
    }
}